using SystemInterfaces;
using EFRepository;

namespace DataServices.Actors
{
    public static class LoadEntitySetExtensions
    {

        public static void LoadEntitySet(this ILoadEntitySet msg)
        {

            DynamicDataContext.LoadEntitySet(msg);

        }

        public static void LoadEntitySetWithChanges(this IGetEntitySetWithChanges msg)
        {

            DynamicDataContext.LoadEntitySetWithChanges(msg);

        }

        public static void LoadEntitySet(this ILoadEntitySetWithFilter msg)
        {

            DynamicDataContext.LoadEntitySetWithFilter(msg);

        }

        public static void LoadEntitySet(this ILoadEntitySetWithFilterWithIncludes msg) 
        {

            DynamicDataContext.LoadEntitySetWithFilterWithIncludes(msg);

        }
    }
}