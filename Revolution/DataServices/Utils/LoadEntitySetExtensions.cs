using SystemInterfaces;
using EFRepository;

namespace DataServices.Actors
{
    public static class LoadEntitySetExtensions
    {

        public static void LoadEntitySet(this ILoadEntitySet msg)
        {

            DynamicDataContext.Instance.LoadEntitySet(msg);
            DataContext.Instance.LoadEntitySet(msg);

        }

        public static void LoadEntitySetWithChanges(this IGetEntitySetWithChanges msg)
        {

            DynamicDataContext.Instance.LoadEntitySetWithChanges(msg);
            DataContext.Instance.LoadEntitySetWithChanges(msg);
        }

        public static void LoadEntitySet(this ILoadEntitySetWithFilter msg)
        {

            DynamicDataContext.Instance.LoadEntitySetWithFilter(msg);
            DataContext.Instance.LoadEntitySetWithFilter(msg);
        }

        public static void LoadEntitySet(this ILoadEntitySetWithFilterWithIncludes msg) 
        {

            DynamicDataContext.Instance.LoadEntitySetWithFilterWithIncludes(msg);
            DataContext.Instance.LoadEntitySetWithFilterWithIncludes(msg);

        }
    }
}