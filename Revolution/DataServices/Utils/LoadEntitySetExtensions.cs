using SystemInterfaces;
using CommonMessages;
using EFRepository;
using EventAggregator;
using EventMessages;

namespace DataServices.Actors
{
    public static class LoadEntitySetExtensions
    {


        public static void LoadEntitySet(this ILoadEntitySetWithChanges msg)
        {

            DynamicDataContext.LoadEntitySet(msg);

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