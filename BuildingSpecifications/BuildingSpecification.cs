using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actor.Interfaces;
using Common.Dynamic;
using RevolutionEntities.Process;

namespace BuildingSpecifications
{
    public class BuildingSpecification<T>: Expando where T : new()
    {
        public T Build()
        {
            var res = new T();
            return res;
        }

        
    }
}
