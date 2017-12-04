using SystemInterfaces;
using JB.Collections.Reactive;

namespace ViewModel.Interfaces
{
    
    public interface IAutoViewModel<T> where T : IEntity
    {
       // VirtualList<T> VirtualData { get; set; }
        ObservableCollection<T> SelectedItems { get; set; }
        void ViewAll();
        void SelectAll();

    }
}
