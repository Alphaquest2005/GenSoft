using SystemInterfaces;
using EFRepository;


namespace DataServices.Actors
{
    public static class GetEntityExtensions
    {
       
        public static void GetEntity(this IGetEntityById msg) 
        {

            DynamicDataContext.GetEntityById(msg);
            DataContext.GetEntityById(msg);
        }

        public static void GetEntity(this IGetEntityWithChanges msg) 
        {

            DynamicDataContext.GetEntityWithChanges(msg);
            DataContext.GetEntityWithChanges(msg);
        }


    }
}