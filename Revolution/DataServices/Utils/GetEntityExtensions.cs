using System;
using System.Linq;
using SystemInterfaces;
using CommonMessages;
using EFRepository;
using EventAggregator;
using EventMessages;


namespace DataServices.Actors
{
    public static class GetEntityExtensions
    {
       
        public static void GetEntity(this IGetEntityById msg) 
        {

          //  EF7DataContext.GetEntityById(msg);
        }

        public static void GetEntity(this IGetEntityWithChanges msg) 
        {

            //EF7DataContext.GetEntityWithChanges(msg);
        }


    }
}