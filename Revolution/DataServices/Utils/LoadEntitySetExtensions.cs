using SystemInterfaces;
using EFRepository;

namespace DataServices.Actors
{
    public static class LoadEntitySetExtensions
    {

        public static void LoadEntitySet(this ILoadEntitySet msg)
        {

            DynamicDataContext.LoadEntitySet(msg);
            DataContext.LoadEntitySet(msg);

        }

        public static void LoadEntitySetWithChanges(this IGetEntitySetWithChanges msg)
        {

            DynamicDataContext.LoadEntitySetWithChanges(msg);
            DataContext.LoadEntitySetWithChanges(msg);
        }

        public static void LoadEntitySet(this ILoadEntitySetWithFilter msg)
        {

            DynamicDataContext.LoadEntitySetWithFilter(msg);
            DataContext.LoadEntitySetWithFilter(msg);
        }

        public static void LoadEntitySet(this ILoadEntitySetWithFilterWithIncludes msg) 
        {

            DynamicDataContext.LoadEntitySetWithFilterWithIncludes(msg);
            DataContext.LoadEntitySetWithFilterWithIncludes(msg);

        }
    }
}