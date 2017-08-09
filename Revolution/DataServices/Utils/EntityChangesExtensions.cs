using System;
using SystemInterfaces;
using CommonMessages;
using EFRepository;
using EventAggregator;
using EventMessages;

namespace DataServices.Actors
{
    public static class EntityChangesExtensions
    {
        public static void UpdateEntity(this IUpdateEntityWithChanges msg)
        {

            DynamicDataContext.UpdateEntity(msg);
        }

        public static void AddEntity(this IAddOrGetEntityWithChanges msg)
        {

            DynamicDataContext.AddEntity(msg);
        }
    }
}