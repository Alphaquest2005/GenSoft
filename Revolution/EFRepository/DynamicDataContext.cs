using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reactive.Linq;
using SystemInterfaces;
using Common;
using Common.DataEntites;
using Common.Dynamic;
using DomainUtilities;
using EventAggregator;
using EventMessages.Events;
using GenSoft.DBContexts;
using GenSoft.Entities;
using Microsoft.EntityFrameworkCore;
using Process.WorkFlow;
using RevolutionEntities.Process;
using Utilities;
using Entity = GenSoft.Entities.Entity;
using EntityEvents = RevolutionData.Context.Entity;
using StateEventInfo = GenSoft.Entities.StateEventInfo;


namespace EFRepository
{
    public class DynamicDataContext:BaseRepository<DynamicDataContext>
    {
        
        private static readonly DynamicDataContext instance = new DynamicDataContext();
        public static DynamicDataContext Instance => instance;
        static DynamicDataContext() { }
        DynamicDataContext()
         {
             
            var processStateInfo = new RevolutionEntities.Process.StateEventInfo(Processes.IntialSystemProcess, new StateEvent("CurrentApplicationChanged", "Current Application Changed", "notes","Process","DataContext"), Guid.NewGuid());

        }

        

      

        public  void Create(ICreateEntity msg)
        {
            throw new NotImplementedException();
        }

        public  void UpdateEntityWithChanges(IUpdateEntityWithChanges msg)
        {
            try
            {
                if (msg.ProcessInfo.Process.Applet is IDbApplet) return;
                var entityTypeName = msg.EntityType.Name;
                var entityId = (int) (msg.Entity.Properties.ContainsKey("Id")?msg.Entity.Properties["Id"]: msg.Entity.Id);
                var msgChanges = msg.Changes;
                var res = UpdateEntityWithChanges(entityTypeName, entityId, msgChanges);
                if (res == null)
                {
                    PublishProcesError(msg,
                        new ApplicationException(
                            $"Entity with Type - '{entityTypeName}' & Id:{res.Id} not Found."),
                        typeof(EntityWithChangesUpdated));
                    
                }
                else
                {
                    EventMessageBus.Current.Publish(
                        new EntityWithChangesUpdated(res, msgChanges,
                            new RevolutionEntities.Process.StateEventInfo(msg.Process,
                                RevolutionData.Context.EventFunctions.UpdateEventData(entityTypeName,
                                    EntityEvents.Events.EntityUpdated)), msg.Process,
                            Source));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private IDynamicEntity UpdateEntityWithChanges(string entityTypeName, int entityId, Dictionary<string, object> msgChanges)
        {
            using (var ctx = new GenSoftDBContext())
            {
                var entityType = ctx.EntityTypes.Include(x => x.Type).First(x => x.Type.Name == entityTypeName);


                var entity = entityId == 0
                    ? ctx.Entities.Add(new Entity()
                    {
                        EntityTypeId = entityType.Id,
                        EntityAttributes = new List<EntityAttribute>(),
                        DateTimeCreated = DateTime.Now
                    }).Entity
                    : ctx.Entities.FirstOrDefault(x => x.Id == entityId && x.EntityTypeId == entityType.Id);
                if (entity == null)
                {
                    return null;
                }


                if (entityId == 0)
                {
                    ctx.SaveChanges(true);
                    msgChanges.Add(nameof(IDynamicEntity.Id), entity.Id);
                }

                foreach (var change in msgChanges.Where(x => x.Value != null))
                {
                    if (change.Value is DynamicType d)
                    {
                        SaveDynamicType(entityTypeName, d, entity);
                        continue;
                    }
                    if (change.Value is List<DynamicType> dlst)
                    {
                        foreach (var itm in dlst)
                        {
                            SaveDynamicType(entityTypeName, itm, entity);
                        }
                        continue;
                    }
                    var data = ctx.EntityAttributes.FirstOrDefault(
                        x => x.EntityId == entity.Id && x.Attribute.Name == change.Key);
                    
                    if (data == null)
                    {
                        var attibute = ctx.Attributes.FirstOrDefault(x => x.Name == change.Key);
                        if (attibute == null)
                        {
                            var dataType = ctx.DataTypes.First(x => x.Type.Name == "System.string");
                            attibute = ctx.Attributes
                                .Add(new GenSoft.Entities.Attribute() {DataTypeId = dataType.Id, Name = change.Key}).Entity;
                        }
                        var entityAttribute = ctx.EntityAttributes
                            .Add(new EntityAttribute()
                            {
                                AttributeId = attibute.Id,
                                EntityId = entity.Id,
                                Value = change.Value is IEntityKeyValuePair
                                    ? ((IEntityKeyValuePair) change.Value).Value.ToString()
                                    : change.Value.ToString(),
                                DateTime = DateTime.Now
                            }).Entity;
                        entity.EntityAttributes.Add(entityAttribute);
                    }
                    else
                    {
                        data.Value = change.Value.ToString();
                        ctx.Update(data);
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

                if (!msgChanges.ContainsKey("Id") && entity.EntityAttributes.Any(x => x.Attribute.Name == "Id")) msgChanges.Add("Id", entityId);

                return GetDynamicEntityWithChanges(ctx,
                    DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(entityType.Type.Name), msgChanges);

                
            }
            
        }

        private void SaveDynamicType(string entityTypeName, DynamicType d, Entity entity)
        {
            var r = DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(d.Type).ParentEntities
                .FirstOrDefault(x => DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(x.Type).Name == entityTypeName);
            if (r == null) return;
            d.Properties[r.Key] = entity.Id;
            UpdateEntityWithChanges(d.Type, d.Id, d.Properties);
        }

        public  void AddEntity(IAddOrGetEntityWithChanges msg)
        {
            throw new NotImplementedException();
        }

        public  void LoadEntitySetWithChanges(IGetEntitySetWithChanges msg)
        {
            if (msg.ProcessInfo.Process.Applet is IDbApplet) return;
            using (var ctx = new GenSoftDBContext())
            {
                var entityType = msg.EntityType;
                var changes = msg.Changes;
                var entityTypeId = ctx.EntityTypes.FirstOrDefault(x => x.Type.Name == entityType.Name)?.Id;

                
                var viewEntityAttributes = GetViewEntityAttributes(ctx, entityTypeId);

                var res = GetEntities(ctx, entityTypeId).Include(x => x.EntityAttributes).ThenInclude(x => x.Attribute).AsQueryable();
                res = changes.Aggregate(res,
                    (current, c) => current.Where(
                        x => x.EntityAttributes.Any(z => z.Attribute.Name == c.Key && z.Value.ToString() == c.Value.ToString())));

                var entities = GetViewEntities(entityType, res, viewEntityAttributes.Keys.ToList()).ToList();
                
                
                    EventMessageBus.Current.Publish(
                        new EntitySetWithChangesLoaded(msg.EntityType,entities, msg.Changes,
                            new RevolutionEntities.Process.StateEventInfo(msg.Process, RevolutionData.Context.EventFunctions.UpdateEventData(msg.EntityType.Name, EntityEvents.Events.EntitySetLoaded)), msg.Process,
                            Source));
               
            }
        }

        public  void LoadEntitySet(ILoadEntitySet msg)
        {
            if (msg.ProcessInfo.Process.Applet is IDbApplet) return;
            using (var ctx = new GenSoftDBContext())
            {
               var entityTypeId = ctx.EntityTypes
                    .FirstOrDefault(x => x.Type.Name == msg.EntityType.Name)?.Id;

                

                var viewEntityAttributes = GetViewEntityAttributes(ctx, entityTypeId);
                var res = GetEntities(ctx, entityTypeId);
                var viewset = GetViewEntities(msg.EntityType, res, viewEntityAttributes.Keys.ToList())
                    .ToList();



                EventMessageBus.Current.Publish(
                        new EntitySetLoaded(msg.EntityType, viewset, 
                            new RevolutionEntities.Process.StateEventInfo(msg.Process, RevolutionData.Context.EventFunctions.UpdateEventData(msg.EntityType.Name, EntityEvents.Events.EntitySetLoaded)), msg.Process,
                            Source));
                
            }
        }

        private  IQueryable<Entity> GetEntities(GenSoftDBContext ctx, int? viewId)
        {
            
            var entities = ctx.Entities.AsNoTracking().Include(x => x.EntityAttributes)
                .ThenInclude(x => x.Attribute).ThenInclude(x => x.EntityId)
                .ThenInclude(x => x.Attribute);
           
            var res = entities
                    .Where(x => x.EntityTypeId == viewId)
                    .Where(x => x.EntityAttributes.Any(z => z.Attribute.EntityId != null));
            return res;
        }

        public  void LoadEntitySetWithFilter(ILoadEntitySetWithFilter msg)
        {
            throw new NotImplementedException();
        }

        public  void LoadEntitySetWithFilterWithIncludes(ILoadEntitySetWithFilterWithIncludes msg)
        {
            throw new NotImplementedException();
        }

        public  void DeleteEntity(IDeleteEntity msg)
        {
            throw new NotImplementedException();
        }

        public  void GetEntityById(IGetEntityById msg)
        {
            throw new NotImplementedException();
        }

        public  void GetEntityWithChanges(IGetEntityWithChanges msg)
        {
            if (msg.ProcessInfo.Process.Applet is IDbApplet) return;
            using (var ctx = new GenSoftDBContext())
            {
                var entityType = msg.EntityType;
                var changes = msg.Changes;

                var entity = GetDynamicEntityWithChanges(ctx, entityType, changes);

                if (entity != null)
                {
                    EventMessageBus.Current.Publish(
                        new EntityWithChangesFound(entity, msg.Changes,
                            new RevolutionEntities.Process.StateEventInfo(msg.Process, RevolutionData.Context.EventFunctions.UpdateEventData(msg.EntityType.Name, EntityEvents.Events.EntityFound)), msg.Process,
                            Source));
                }
                else
                {
                    EventMessageBus.Current.Publish(
                        new EntityWithChangesFound(DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(msg.EntityType.Name).DefaultEntity(), msg.Changes,
                            new RevolutionEntities.Process.StateEventInfo(msg.Process, RevolutionData.Context.EventFunctions.UpdateEventData(msg.EntityType.Name, EntityEvents.Events.EntityFound)), msg.Process,
                            Source));
                }
            }
        }

        private  IDynamicEntity GetDynamicEntityWithChanges(GenSoftDBContext ctx, IDynamicEntityType entityType,
            Dictionary<string, object> changes)
        {
            try
            {
                
                var entityTypeId = ctx.EntityTypes
                    .FirstOrDefault(x => x.Type.Name == entityType.Name)?.Id;

                var viewEntityAttributes = GetViewEntityAttributes(ctx, entityTypeId);

                var res = GetEntities(ctx, entityTypeId).AsQueryable();
                // include entity.Id and Attribute "ID" to avoid confusion

                var cres = changes.Any(x => x.Key == "Id" && viewEntityAttributes.ContainsValue(x.Key)) ? changes.Where(x => x.Key == "Id" && x.Value != null).Aggregate(res,
                    (current, c) => current.Where(x => x.Id.ToString() == c.Value.ToString() || (x.EntityAttributes.Any(z =>
                                                           z.Attribute.Name == c.Key && z.Value.ToString() == c.Value.ToString())))) : res;

                var ccres = changes.Where(x => x.Key != "Id" &&  viewEntityAttributes.ContainsValue(x.Key) && x.Value != null).Aggregate(cres,
                    (current, c) => current.Where(
                        x => x.EntityAttributes.Any(z =>
                            z.Attribute.Name == c.Key && z.Value.ToString() == c.Value.ToString())));

                var entity = GetViewEntities(entityType, ccres, viewEntityAttributes.Keys.ToList()).FirstOrDefault();
                return entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private  IQueryable<IDynamicEntity> GetViewEntities(IDynamicEntityType entityType, IQueryable<Entity> res, List<int> viewEntityAttributes)
        {
            
            return res.Select(x => new DynamicEntity(entityType, x.Id,
                x.EntityAttributes
                    .Where(z => viewEntityAttributes.Contains(z.AttributeId))
                    .OrderBy(d => viewEntityAttributes.IndexOf(d.AttributeId))
                    .Select(z => new {z.Attribute.Name, z.Value})
                    .ToDictionary(q => q.Name, q => q.Value as object)) as IDynamicEntity);
        }

        private  Dictionary<int, string> GetViewEntityAttributes(GenSoftDBContext ctx, int? viewId)
        {
            return ctx.EntityTypeAttributes.AsNoTracking()
                .Where(x => x.EntityTypeId == viewId)
                .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                .Select(x => new {x.AttributeId, x.Attribute.Name}).ToDictionary(x => x.AttributeId, x => x.Name);
        }
    }
}