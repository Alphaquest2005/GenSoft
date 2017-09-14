using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using SystemInterfaces;
using Common;
using Common.DataEntites;
using Common.Dynamic;
using EventAggregator;
using EventMessages.Commands;
using EventMessages.Events;
using GenSoft.DBContexts;
using GenSoft.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RevolutionEntities.Process;
using Entity = GenSoft.Entities.Entity;
using EntityEvents = RevolutionData.Context.Entity;


namespace EFRepository
{
    public class DynamicDataContext:BaseRepository
    {
        public static void Create(ICreateEntity msg)
        {
            throw new NotImplementedException();
        }

        public static void UpdateEntityWithChanges(IUpdateEntityWithChanges msg)
        {
            try
            {


                using (var ctx = new GenSoftDBContext())
                {
                    var viewType = ctx.EntityView.FirstOrDefault(x => x.EntityType.Type.Name == msg.EntityType.Name);
                    var ctxEntityType = ctx.EntityType.Include(x => x.Type);
                    var entityType = viewType == null
                        ? ctxEntityType.FirstOrDefault(x => x.Type.Name == msg.EntityType.Name)
                        : ctxEntityType.FirstOrDefault(x => x.Id == viewType.BaseEntityTypeId);


                    if (entityType == null)
                    {
                        PublishProcesError(msg,
                            new ApplicationException($"EntityType - '{msg.Entity.EntityType.Name}' not Found."),
                            typeof(EntityWithChangesUpdated));
                        return;
                    }



                    var entity = msg.Entity.Id == 0
                        ? ctx.Entity.Add(new Entity() {EntityTypeId = entityType.Id, EntityAttribute = new List<EntityAttribute>()}).Entity
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
                                    Value = change.Value.ToString()
                                }).Entity;
                            entity.EntityAttribute.Add(entityAttribute);
                        }
                        else
                        {
                            data.Value = change.Value.ToString();
                        }
                    }
                    ctx.SaveChanges(true);
                    foreach (var change in msg.Changes.Where(x => x.Key != nameof(IDynamicEntity.Id)))
                    {
                        var entityTypes = ctx.EntityView.Include(x => x.EntityType).ThenInclude(x => x.Type)
                            .Where(x => x.BaseEntityTypeId == entityType.Id &&
                                        x.EntityType.EntityTypeAttributes.Any(z => z.Attributes.Name == change.Key));
                           // .Select(x => x.EntityType).ToList();
                            

                        foreach (var et in entityTypes)
                        {
                            var newEntity = GetDynamicEntityWithChanges(ctx, DynamicEntityType.DynamicEntityTypes[et.EntityType.Type.Name], new Dictionary<string, object>(){{change.Key, change.Value},{nameof(IDynamicEntity.Id), entity.Id}});

                            EventMessageBus.Current.Publish(
                                new EntityWithChangesUpdated(newEntity, msg.Changes,
                                    new StateEventInfo(msg.Process.Id, EntityEvents.Events.EntityUpdated), msg.Process,
                                    Source), Source);
                        }

                    }

                    
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
            using (var ctx = new GenSoftDBContext())
            {
                var entityType = msg.EntityType;
                var changes = msg.Changes;
                var viewTypeId = ctx.EntityView
                    .FirstOrDefault(x => x.EntityType.Type.Name == entityType.Name)?.Id;

                var entityTypeId = ctx.EntityView
                    .FirstOrDefault(x => x.EntityType.Type.Name == entityType.Name)?.BaseEntityTypeId;

                var viewEntityAttributes = GetViewEntityAttributes(ctx, viewTypeId);

                var res = GetEntities(ctx, entityTypeId).Include(x => x.EntityAttribute).ThenInclude(x => x.Attributes)
                    .AsQueryable();
                res = changes.Aggregate(res,
                    (current, c) => current.Where(
                        x => x.EntityAttribute.Any(z => z.Attributes.Name == c.Key && z.Value.ToString() == c.Value.ToString())));

                var entities = GetViewEntities(entityType, res, viewEntityAttributes).ToList();
                //var entities = res.Select(x => new DynamicEntity(msg.EntityType, x.Id, x.Entity.EntityAttribute.Select(z => new {z.Attributes.Name, z.Value}).ToDictionary(q => q.Name, q=> q.Value as object)) as IDynamicEntity)
                //     .ToList();
                
                    EventMessageBus.Current.Publish(
                        new EntitySetWithChangesLoaded(msg.EntityType,entities, msg.Changes,
                            new StateEventInfo(msg.Process.Id, EntityEvents.Events.EntityFound), msg.Process,
                            Source), Source);
               
            }
        }

        public static void LoadEntitySet(ILoadEntitySet msg)
        {
            using (var ctx = new GenSoftDBContext())
            {
                //var res = ctx.EntityAttribute
                //    .Include(x => x.Attributes)
                //    .Include(x => x.Entity.EntityAttribute)
                //    .Include(x => x.Entity.EntityType.Type)
                //    .AsNoTracking()
                //    .Where(x => x.Entity.EntityType.Type.Name == msg.EntityType)
                //    .Select(x => new DynamicEntity(msg.EntityType, x.Id,
                //        x.Entity.EntityAttribute.Select(z => new EntityKeyValuePair(z.Attributes.Name, z.Value)).ToList()) as IDynamicEntity)
                //    .ToList();

                var viewTypeId = ctx.EntityView
                    .FirstOrDefault(x => x.EntityType.Type.Name == msg.EntityType.Name)?.Id;

                var entityTypeId = ctx.EntityView
                    .FirstOrDefault(x => x.EntityType.Type.Name == msg.EntityType.Name)?.BaseEntityTypeId;

                var viewEntityAttributes = GetViewEntityAttributes(ctx, viewTypeId);
                var res = GetEntities(ctx, entityTypeId);
                var viewset = GetViewEntities(msg.EntityType, res, viewEntityAttributes)
                    .ToList();



                EventMessageBus.Current.Publish(
                        new EntitySetLoaded(msg.EntityType, viewset, 
                            new StateEventInfo(msg.Process.Id, EntityEvents.Events.EntityFound), msg.Process,
                            Source), Source);
                
            }
        }

        private static IQueryable<Entity> GetEntities(GenSoftDBContext ctx, int? viewId)
        {
            return ctx.Entity.Where(x => x.EntityTypeId == viewId).Where(x => x.EntityAttribute.Any(z => z.Attributes.EntityId != null));
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
            using (var ctx = new GenSoftDBContext())
            {
                var entityType = msg.EntityType;
                var changes = msg.Changes;

                var entity = GetDynamicEntityWithChanges(ctx, entityType, changes);

                if (entity != null)
                {
                    EventMessageBus.Current.Publish(
                        new EntityWithChangesFound(entity, msg.Changes,
                            new StateEventInfo(msg.Process.Id, EntityEvents.Events.EntityFound), msg.Process,
                            Source), Source);
                }
                else
                {
                    EventMessageBus.Current.Publish(
                        new EntityWithChangesFound(DynamicEntityType.DynamicEntityTypes[msg.EntityType.Name].DefaultEntity(), msg.Changes,
                            new StateEventInfo(msg.Process.Id, EntityEvents.Events.EntityFound), msg.Process,
                            Source), Source);
                }
            }
        }

        private static IDynamicEntity GetDynamicEntityWithChanges(GenSoftDBContext ctx, IDynamicEntityType entityType, Dictionary<string, object> changes)
        {
            var viewTypeId = ctx.EntityView
                .FirstOrDefault(x => x.EntityType.Type.Name == entityType.Name)?.Id;

            var entityTypeId = ctx.EntityView
                .FirstOrDefault(x => x.EntityType.Type.Name == entityType.Name)?.BaseEntityTypeId;

            var viewEntityAttributes = GetViewEntityAttributes(ctx, viewTypeId);

            var res = GetEntities(ctx, entityTypeId).Include(x => x.EntityAttribute).ThenInclude(x => x.Attributes)
                .AsQueryable();
            res = changes.Aggregate(res,
                (current, c) => current.Where(
                    x => x.EntityAttribute.Any(z => z.Attributes.Name == c.Key && z.Value.ToString() == c.Value.ToString())));

            var entity = GetViewEntities(entityType, res, viewEntityAttributes).FirstOrDefault();
            return entity;
        }

        private static IQueryable<IDynamicEntity> GetViewEntities(IDynamicEntityType entityType, IQueryable<Entity> res, List<int> viewEntityAttributes)
        {
            return res.Select(x => new DynamicEntity(entityType, x.Id,
                x.EntityAttribute
                    .Where(z => viewEntityAttributes.Contains(z.AttributeId))
                    .OrderBy(d => viewEntityAttributes.IndexOf(d.AttributeId))
                    .Select(z => new {z.Attributes.Name, z.Value})
                    .ToDictionary(q => q.Name, q => q.Value as object)) as IDynamicEntity);
        }

        private static List<int> GetViewEntityAttributes(GenSoftDBContext ctx, int? viewId)
        {
            return ctx.EntityTypeAttributes
                .Where(x => x.EntityTypeId == viewId)
                .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                .Select(x => x.AttributeId).ToList();
        }
    }
}