using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using SystemInterfaces;
using Common.DataEntites;
using DomainUtilities;
using EventAggregator;
using EventMessages.Events;
using GenSoft.DBContexts;
using GenSoft.Entities;
using Microsoft.EntityFrameworkCore;
using Process.WorkFlow;
using RevolutionEntities.Process;
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

                using (var ctx = new GenSoftDBContext())
                {
                    var entityType = ctx.EntityTypes.Include(x => x.Type).First(x => x.Type.Name == msg.EntityType.Name);


                    var entity = msg.Entity.Id == 0
                        ? ctx.Entities.Add(new Entity()
                        {
                            EntityTypeId = entityType.Id,
                            EntityAttributes = new List<EntityAttribute>(),
                            DateTimeCreated = DateTime.Now
                        }).Entity
                        : ctx.Entities.FirstOrDefault(x => x.Id == msg.Entity.Id && x.EntityTypeId == entityType.Id);
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
                        var data = ctx.EntityAttributes.FirstOrDefault(
                            x => x.EntityId == entity.Id && x.Attribute.Name == change.Key);
                        if (data == null)
                        {
                            var attibute = ctx.Attributes.FirstOrDefault(x => x.Name == change.Key);
                            if (attibute == null)
                            {
                                var dataType = ctx.DataTypes.First(x => x.Type.Name == "string");
                                attibute = ctx.Attributes
                                    .Add(new GenSoft.Entities.Attribute() {DataTypeId = dataType.Id, Name = change.Key}).Entity;
                            }
                            var entityAttribute = ctx.EntityAttributes
                                .Add(new EntityAttribute()
                                {
                                    AttributeId = attibute.Id,
                                    EntityId = entity.Id,
                                    Value = change.Value is IEntityKeyValuePair?((IEntityKeyValuePair)change.Value).Value.ToString():change.Value.ToString(),
                                    DateTime = DateTime.Now
                                }).Entity;
                            entity.EntityAttributes.Add(entityAttribute);
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
                                RevolutionData.Context.EventFunctions.UpdateEventData(msg.EntityType.Name, EntityEvents.Events.EntityUpdated)), msg.Process,
                            Source));


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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

                var entities = GetViewEntities(entityType, res, viewEntityAttributes).ToList();
                
                
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
                var viewset = GetViewEntities(msg.EntityType, res, viewEntityAttributes)
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
                var cres = changes.Where(x => x.Key == "Id").Aggregate(res,
                    (current, c) => current.Where(x => x.Id.ToString() == c.Value.ToString() || (x.EntityAttributes.Any(z =>
                                                           z.Attribute.Name == c.Key && z.Value.ToString() == c.Value.ToString()))));

                var ccres = changes.Where(x => x.Key != "Id").Aggregate(cres,
                    (current, c) => current.Where(
                        x => x.EntityAttributes.Any(z =>
                            z.Attribute.Name == c.Key && z.Value.ToString() == c.Value.ToString())));

                var entity = GetViewEntities(entityType, ccres, viewEntityAttributes).FirstOrDefault();
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

        private  List<int> GetViewEntityAttributes(GenSoftDBContext ctx, int? viewId)
        {
            return ctx.EntityTypeAttributes.AsNoTracking()
                .Where(x => x.EntityTypeId == viewId)
                .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                .Select(x => x.AttributeId).ToList();
        }
    }
}