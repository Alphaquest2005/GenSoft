using System.Linq;
using SystemInterfaces;

namespace Common.DataEntites
{
    public static class DynamicEntityTypeExtenstions
    {
        public static IDynamicEntity DefaultEntity(this IDynamicEntityType dt)
        {
            return new DynamicEntity(dt, 0, dt.Properties.ToDictionary(x => x.Key, x => x.Value)); 
        }
    }
}