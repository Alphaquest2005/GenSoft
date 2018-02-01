using System;
using System.Collections.Generic;
using System.Linq;
using SystemInterfaces;
using Common.DataEntites;
using DomainUtilities;
using EventAggregator;
using EventMessages.Events;
using GenSoft.DBContexts;
using GenSoft.Entities;
using Microsoft.EntityFrameworkCore;
using Utilities;
using Entity = GenSoft.Entities.Entity;
using EntityEvents = RevolutionData.Context.Entity;
using System.Linq.Dynamic;


namespace EFRepository
{
    public class DataContext:BaseRepository
    {
        private static bool IsRealDatabase = true;
        private static string entityNamespace = "GenSoft.Entities.";

        static DataContext()
        {
            EventMessageBus.Current.GetEvent<ICurrentApplicationChanged>(Source).Subscribe(OnCurrentApplicationChanged);
            //GenSoft-Creator is the default database and because its real data i need not do this any where else
            SetCurrentApplication(1);
        }

        private static Application CurrentApplication { get;  set; }
        private static void OnCurrentApplicationChanged(ICurrentApplicationChanged currentEntityChanged)
        {
            if(currentEntityChanged.Entity == null) return;
            if (CurrentApplication?.Id == currentEntityChanged.Entity.Id) return;
            SetCurrentApplication(currentEntityChanged.Entity.Id);
        }

        private static void SetCurrentApplication(int appId)
        {
            using (var ctx = new GenSoftDBContext())
            {
                CurrentApplication = ctx.Application.Include(x => x.DatabaseInfo)
                    .First(x => x.Id == appId);
            }
        }

        public static void Create(ICreateEntity msg)
        {
            throw new NotImplementedException();
        }

        public static void UpdateEntityWithChanges(IUpdateEntityWithChanges msg)
        {
            try
            {
                if (CurrentApplication?.DatabaseInfo.IsRealDatabase != IsRealDatabase) return;
                if (TypeNameExtensions.EntityTypesLkp.TryGetValue(entityNamespace + msg.EntityType.Name,out var _) == false) return;
                using (var ctx = new GenSoftDBContext())
                {
                    var entityType = ctx.EntityType.Include(x => x.Type).First(x => x.Type.Name == msg.EntityType.Name);


                    var entity = msg.Entity.Id == 0
                        ? ctx.Entity.Add(new Entity()
                        {
                            EntityTypeId = entityType.Id,
                            EntityAttribute = new List<EntityAttribute>()
                        }).Entity
                        : ctx.Entity.FirstOrDefault(x => x.Id == msg.Entity.Id && x.EntityTypeId == entityType.Id);
                    if (entity == null)
                    {
                        PublishProcesError(msg,
                            new ApplicationException(
                                $"Entity with Type - '{msg.Entity.EntityType.Name}' & Id:{msg.Entity.Id} not Found."),
                            typeof(EntityWithChangesUpdated));
                        return;
                    }

                    if (msg.Entity.Id == 0)
                    {
                        ctx.SaveChanges(true);
                        msg.Changes.Add(nameof(IDynamicEntity.Id), entity.Id);
                    }

                    foreach (var change in msg.Changes)
                    {
                        var data = ctx.EntityAttribute.FirstOrDefault(
                            x => x.EntityId == entity.Id && x.Attributes.Name == change.Key);
                        if (data == null)
                        {
                            var attibute = ctx.Attributes.FirstOrDefault(x => x.Name == change.Key);
                            if (attibute == null)
                            {
                                var dataType = ctx.DataType.First(x => x.Type.Name == "string");
                                attibute = ctx.Attributes
                                    .Add(new Attributes() {DataTypeId = dataType.Id, Name = change.Key}).Entity;
                            }
                            var entityAttribute = ctx.EntityAttribute
                                .Add(new EntityAttribute()
                                {
                                    AttributeId = attibute.Id,
                                    EntityId = entity.Id,
                                    Value = change.Value is IEntityKeyValuePair?((IEntityKeyValuePair)change.Value).Value.ToString():change.Value.ToString()
                                }).Entity;
                            entity.EntityAttribute.Add(entityAttribute);
                        }
                        else
                        {
                            data.Value = change.Value.ToString();
                        }
                    }
                    ctx.SaveChanges(true);

                    //ToDo: update dependent properties
                    //foreach (var change in msg.Changes.Where(x => x.Key != nameof(IDynamicEntity.Id)))
                    //{
                    //    var entityTypes = ctx.EntityType.Include(x => x.Type)
                    //        .Where(x => x.EntityTypeAttributes.Any(z => z.Attributes.Name == change.Key));
                    //       // .Select(x => x.EntityType).ToList();


                    //    foreach (var et in entityTypes)
                    //    {
                    //        var newEntity = GetDynamicEntityWithChanges(ctx, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(et.Type.Name], new Dictionary<string, object>(){{change.Key, change.Value},{nameof(IDynamicEntity.Id), entity.Id}});

                    //        EventMessageBus.Current.Publish(
                    //            new EntityWithChangesUpdated(newEntity, msg.Changes,
                    //                new RevolutionEntities.Process.StateEventInfo(msg.Process.Id, EntityEvents.Events.EntityUpdated), msg.Process,
                    //                Source), Source);
                    //    }

                    //}
                    //}
                    var newEntity = GetDynamicEntityWithChanges(ctx,
                        DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(entityType.Type.Name), msg.Changes);

                    EventMessageBus.Current.Publish(
                        new EntityWithChangesUpdated(newEntity, msg.Changes,
                            new RevolutionEntities.Process.StateEventInfo(msg.Process,
                                EntityEvents.Events.EntityUpdated), msg.Process,
                            Source), Source);


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void AddEntity(IAddOrGetEntityWithChanges msg)
        {
            throw new NotImplementedException();
        }

        public static void LoadEntitySetWithChanges(IGetEntitySetWithChanges msg)
        {
            
            if (CurrentApplication?.DatabaseInfo.IsRealDatabase != IsRealDatabase) return;
            if(TypeNameExtensions.EntityTypesLkp.TryGetValue(entityNamespace + msg.EntityType.Name, out var _) == false) return;
            using (var ctx = new GenSoftDBContext())
            {
                var entityType = msg.EntityType;
                
                var changes = msg.Changes;
                var entityTypeId = ctx.EntityType.FirstOrDefault(x => x.Type.Name == entityType.Name)?.Id;

                
                var viewEntityAttributes = GetViewEntityAttributes(ctx, entityTypeId);
                var whereStr = GetWhereStr(msg.Changes);
               
                    var res = GetEntities<IEntity>(ctx, msg.EntityType.Name).Where(whereStr).AsNoTracking();


                    var entities = GetViewEntities(entityType, res, viewEntityAttributes).ToList();


                    EventMessageBus.Current.Publish(
                        new EntitySetWithChangesLoaded(msg.EntityType, entities, msg.Changes,
                            new RevolutionEntities.Process.StateEventInfo(msg.Process,
                                EntityEvents.Events.EntityFound), msg.Process,
                            Source), Source);
                
            }
        }

        private static string GetWhereStr(Dictionary<string, object> changes)
        {
            var whereStr = changes.Aggregate("", (str, itm) => str + ($"{itm.Key} == \"{itm.Value}\" &&"));
            whereStr = whereStr.TrimEnd('&');
            return whereStr;
        }

        public static void LoadEntitySet(ILoadEntitySet msg)
        {
            if (CurrentApplication?.DatabaseInfo.IsRealDatabase != IsRealDatabase) return;
            if (TypeNameExtensions.EntityTypesLkp.TryGetValue(entityNamespace + msg.EntityType.Name, out var _) == false) return;

            using (var ctx = new GenSoftDBContext())
            {
                var viewset = GetEntities<IEntity>(ctx, msg.EntityType.Name).AsNoTracking().ToList()
                    .Select(x => x.ToDynamicEntity(msg.EntityType)).ToList();
            
            EventMessageBus.Current.Publish(
                        new EntitySetLoaded(msg.EntityType, viewset, 
                            new RevolutionEntities.Process.StateEventInfo(msg.Process, EntityEvents.Events.EntityFound), msg.Process,
                            Source), Source);
             }   
            
        }

        private static IQueryable<TEntity> GetEntities<TEntity>(DbContext ctx,string type) where TEntity : class, IEntity
        {
            try
            {
                var dbType = TypeNameExtensions.GetTypeByName(entityNamespace + type).First();
                
                    return (IQueryable<TEntity>) ctx.GetType().GetMethod("Set")?.MakeGenericMethod(dbType)
                        .Invoke(ctx, new object[] { });
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public static void LoadEntitySetWithFilter(ILoadEntitySetWithFilter msg)
        {
            throw new NotImplementedException();
        }

        public static void LoadEntitySetWithFilterWithIncludes(ILoadEntitySetWithFilterWithIncludes msg)
        {
            throw new NotImplementedException();
        }

        public static void DeleteEntity(IDeleteEntity msg)
        {
            throw new NotImplementedException();
        }

        public static void GetEntityById(IGetEntityById msg)
        {
            throw new NotImplementedException();
        }

        public static void GetEntityWithChanges(IGetEntityWithChanges msg)
        {
            if (CurrentApplication?.DatabaseInfo.IsRealDatabase != IsRealDatabase) return;
            if (TypeNameExtensions.EntityTypesLkp.TryGetValue(entityNamespace + msg.EntityType.Name, out var _) == false) return;
            using (var ctx = new GenSoftDBContext())
            {
                var entityType = msg.EntityType;
                var changes = msg.Changes;

                var entity = GetDynamicEntityWithChanges(ctx, entityType, changes);

                if (entity != null)
                {
                    EventMessageBus.Current.Publish(
                        new EntityWithChangesFound(entity, msg.Changes,
                            new RevolutionEntities.Process.StateEventInfo(msg.Process, EntityEvents.Events.EntityFound), msg.Process,
                            Source), Source);
                }
                else
                {
                    EventMessageBus.Current.Publish(
                        new EntityWithChangesFound(DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(msg.EntityType.Name).DefaultEntity(), msg.Changes,
                            new RevolutionEntities.Process.StateEventInfo(msg.Process, EntityEvents.Events.EntityFound), msg.Process,
                            Source), Source);
                }
            }
        }

        private static IDynamicEntity GetDynamicEntityWithChanges(GenSoftDBContext ctx, IDynamicEntityType entityType,
            Dictionary<string, object> changes)
        {
            try
            {
                
                var entityTypeId = ctx.EntityType
                    .FirstOrDefault(x => x.Type.Name == entityType.Name)?.Id;

                var viewEntityAttributes = GetViewEntityAttributes(ctx, entityTypeId);
               
                    var res = GetEntities<IEntity>(ctx,entityType.Name);
                    // include entity.Id and Attribute "ID" to avoid confusion
                    var cres = res.Where(GetWhereStr(changes.Where(x => x.Key == "Id")
                        .ToDictionary(x => x.Key, x => x.Value)));

                    var ccres = cres.Where(GetWhereStr(changes.Where(x => x.Key != "Id")
                        .ToDictionary(x => x.Key, x => x.Value)));

                    var entity = GetViewEntities(entityType, ccres, viewEntityAttributes).FirstOrDefault();
                    return entity;
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private static IQueryable<IDynamicEntity> GetViewEntities(IDynamicEntityType entityType, IQueryable<IEntity> res, List<int> viewEntityAttributes)
        {
            return res.Select(x => new DynamicEntity(entityType, x.Id,
                entityType.Properties
                    .Select(z => new {Name = z.Key, Value = x.GetType().GetProperty(z.Key).GetValue(x)})
                    .ToDictionary(q => q.Name, q => q.Value as object)) as IDynamicEntity);
        }

        private static List<int> GetViewEntityAttributes(GenSoftDBContext ctx, int? viewId)
        {
            return ctx.EntityTypeAttributes.AsNoTracking()
                .Where(x => x.EntityTypeId == viewId)
                .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                .Select(x => x.AttributeId).ToList();
        }
    }
}