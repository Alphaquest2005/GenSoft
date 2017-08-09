using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemInterfaces;


namespace EFRepository
{
    public class DynamicDataContext
    {
        public static void Create(ICreateEntity msg)
        {
            throw new NotImplementedException();
        }

        public static void UpdateEntity(IUpdateEntityWithChanges msg)
        {
            throw new NotImplementedException();
        }

        public static void AddEntity(IAddOrGetEntityWithChanges msg)
        {
            throw new NotImplementedException();
        }

        public static void LoadEntitySet(ILoadEntitySetWithChanges msg)
        {
            throw new NotImplementedException();
        }

        public static void LoadEntitySetWithFilter(ILoadEntitySetWithFilter msg)
        {
            throw new NotImplementedException();
        }

        public static void LoadEntitySetWithFilterWithIncludes(ILoadEntitySetWithFilterWithIncludes msg)
        {
            throw new NotImplementedException();
        }

        public static void DeleteEntity(IDeleteEntity msg)
        {
            throw new NotImplementedException();
        }
    }
}