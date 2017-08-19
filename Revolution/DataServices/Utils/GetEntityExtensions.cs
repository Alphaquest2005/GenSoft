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

            DynamicDataContext.GetEntityById(msg);
        }

        public static void GetEntity(this IGetEntityWithChanges msg) 
        {

            DynamicDataContext.GetEntityWithChanges(msg);
        }


    }
}