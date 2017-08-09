using System.ComponentModel.Composition;
using System.Runtime.InteropServices.ComTypes;

namespace SystemInterfaces
{
    
    
    public interface IGetEntityById: IEntityRequest
    {
       // void Create(int entityId);
        int EntityId { get; }
    }
}