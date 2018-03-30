namespace SystemInterfaces
{
    public interface ICacheUpdated : IEntityRequest
    {
       
    }


    public interface IUpdateCache: IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }
}