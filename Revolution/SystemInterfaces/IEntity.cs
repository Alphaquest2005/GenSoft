using System.ComponentModel.Composition;

namespace SystemInterfaces
{

    public interface IDynamicEntity:IEntity
    {
        string EntityType { get; }
    }
    public interface IEntity:IEntityId
    {
        RowState RowState { get; set; }

        
    }
}