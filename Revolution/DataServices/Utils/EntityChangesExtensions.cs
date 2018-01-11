using SystemInterfaces;
using EFRepository;

namespace DataServices.Actors
{
    public static class EntityChangesExtensions
    {
        public static void UpdateEntity(this IUpdateEntityWithChanges msg)
        {

            DynamicDataContext.UpdateEntityWithChanges(msg);
            DataContext.UpdateEntityWithChanges(msg);
        }

        public static void AddEntity(this IAddOrGetEntityWithChanges msg)
        {

            DynamicDataContext.AddEntity(msg);
            DataContext.AddEntity(msg);
        }
    }
}