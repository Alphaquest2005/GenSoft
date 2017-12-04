using SystemInterfaces;
using EFRepository;

namespace DataServices.Actors
{
    public static class CreateEntityExtensions
    {
        
        public static void CreateEntity(this ICreateEntity msg) 
        {
            DynamicDataContext.Create(msg);
        }

    }


}