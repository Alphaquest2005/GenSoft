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
using EventMessages.Events;
using GenSoft.DBContexts;
using Microsoft.EntityFrameworkCore;
using RevolutionData.Context;
using RevolutionEntities.Process;


namespace EFRepository
{
    public class DynamicDataContext:BaseRepository
    {
        public static void Create(ICreateEntity msg)
        {
            throw new NotImplementedException();
        }

        public static void UpdateEntity(IUpdateEntityWithChanges msg)
        {
            throw new NotImplementedException();
        }

        public static void AddEntity(IAddOrGetEntityWithChanges msg)
        {
            throw new NotImplementedException();
        }

        public static void LoadEntitySetWithChanges(ILoadEntitySetWithChanges msg)
        {
            using (var ctx = new GenSoftDBContext())
            {
                var res = ctx.EntityAttribute
                    .Include(x => x.Attributes)
                    .Include(x => x.Entity.EntityAttribute)
                    .AsNoTracking()
                    .Where(x => x.Entity.Type.Name == msg.EntityType);

                res = msg.Changes.Aggregate(res, (current, c) => current.Where(x => x.Entity.EntityAttribute.Any(z => z.Attributes.Name == c.Key && z.Value == (string)c.Value)));

                var entities = res.Select(x => new DynamicEntity(msg.EntityType, x.Id,
                        x.Entity.EntityAttribute.Select(z => new EntityKeyValuePair(z.Attributes.Name, z.Value)).ToList()) as IDynamicEntity)
                     .ToList();
                if (entities.Any())
                {
                    EventMessageBus.Current.Publish(
                        new EntitySetWithChangesLoaded(msg.EntityType,entities, msg.Changes,
                            new StateEventInfo(msg.Process.Id, EntityView.Events.EntityViewFound), msg.Process,
                            Source), Source);
                }
                else
                {
                    // not found
                }
            }
        }

        public static void LoadEntitySet(ILoadEntitySet msg)
        {
            using (var ctx = new GenSoftDBContext())
            {
                var res = ctx.EntityAttribute
                    .Include(x => x.Attributes)
                    .Include(x => x.Entity.EntityAttribute).AsNoTracking()
                    .Where(x => x.Entity.Type.Name == msg.EntityType)
                    .Select(x => new DynamicEntity(msg.EntityType, x.Id,
                        x.Entity.EntityAttribute.Select(z => new EntityKeyValuePair(z.Attributes.Name, z.Value)).ToList()) as IDynamicEntity)
                    .ToList();
                if (res != null)
                {
                    EventMessageBus.Current.Publish(
                        new EntitySetLoaded(msg.EntityType, res, 
                            new StateEventInfo(msg.Process.Id, EntityView.Events.EntityViewFound), msg.Process,
                            Source), Source);
                }
                else
                {
                    // not found
                }
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
            using (var ctx = new GenSoftDBContext())
            {
                
                var res = ctx.EntityAttribute
                    .Include(x => x.Attributes)
                    .Include(x => x.Entity.EntityAttribute)
                    .AsNoTracking()
                    .Where(x => x.Entity.Type.Name == msg.EntityType);

                res = msg.Changes.Aggregate(res, (current, c) => current.Where(x => x.Entity.EntityAttribute.Any(z => z.Attributes.Name == c.Key && z.Value == (string) c.Value)));


               var entity = res.Select(x => new DynamicEntity(msg.EntityType, x.Id,
                        x.Entity.EntityAttribute.Select(z => new EntityKeyValuePair(z.Attributes.Name, z.Value)).ToList()))
                    .FirstOrDefault();
                if (entity != null)
                {
                    EventMessageBus.Current.Publish(
                        new EntityWithChangesFound(entity, msg.Changes,
                            new StateEventInfo(msg.Process.Id, EntityView.Events.EntityViewFound), msg.Process,
                            Source), Source);
                }
                else
                {
                    // not found
                }
            }
        }


    }
}