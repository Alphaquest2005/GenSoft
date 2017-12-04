namespace ViewModel.Interfaces
{
    
    public interface IDynamicViewModel<out TViewModel> : IViewModel where TViewModel : IViewModel
    {
        TViewModel ViewModel { get; }
    }
}
