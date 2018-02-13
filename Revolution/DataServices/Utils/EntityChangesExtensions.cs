using SystemInterfaces;
using EFRepository;

namespace DataServices.Actors
{
    public static class EntityChangesExtensions
    {
        public static void UpdateEntity(this IUpdateEntityWithChanges msg)
        {

            DynamicDataContext.Instance.UpdateEntityWithChanges(msg);
            DataContext.Instance.UpdateEntityWithChanges(msg);
        }

        public static void AddEntity(this IAddOrGetEntityWithChanges msg)
        {

            DynamicDataContext.Instance.AddEntity(msg);
            DataContext.Instance.AddEntity(msg);
        }
    }
}