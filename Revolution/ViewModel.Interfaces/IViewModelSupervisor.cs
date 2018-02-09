using SystemInterfaces;

namespace ViewModel.Interfaces
{
    
    public interface IViewModelSupervisor : IService<IViewModelSupervisor>
    {
    }

    public interface IEntityDataServiceSupervisor : IService<IEntityDataServiceSupervisor>
    {
        IDynamicEntityType EntityType { get; }
    }
}
