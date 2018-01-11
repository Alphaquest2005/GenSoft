using SystemInterfaces;
using EFRepository;

namespace DataServices.Actors
{
    public static class DeleteEntityExtensions
    {
        
        public static void DeleteEntity(this IDeleteEntity msg) 
        {
            DynamicDataContext.DeleteEntity(msg);
            DataContext.DeleteEntity(msg);
        }

    }
}