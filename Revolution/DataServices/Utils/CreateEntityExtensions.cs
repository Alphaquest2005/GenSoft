using System;
using SystemInterfaces;
using CommonMessages;
using EFRepository;
using EventAggregator;
using EventMessages;

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