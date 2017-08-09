using System;
using SystemInterfaces;
using CommonMessages;
using EFRepository;
using EventAggregator;
using EventMessages;

namespace DataServices.Actors
{
    public static class DeleteEntityExtensions
    {
        
        public static void DeleteEntity(this IDeleteEntity msg) 
        {
            DynamicDataContext.DeleteEntity(msg);
        }

    }
}