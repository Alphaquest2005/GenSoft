using SystemInterfaces;
using EFRepository;


namespace DataServices.Actors
{
    public static class GetEntityExtensions
    {
       
        public static void GetEntity(this IGetEntityById msg) 
        {

            DynamicDataContext.Instance.GetEntityById(msg);
            DataContext.Instance.GetEntityById(msg);
        }

        public static void GetEntity(this IGetEntityWithChanges msg) 
        {

            DynamicDataContext.Instance.GetEntityWithChanges(msg);
            DataContext.Instance.GetEntityWithChanges(msg);
        }


    }
}