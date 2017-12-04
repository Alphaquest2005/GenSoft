using Reactive.Bindings;

namespace ViewModel.Interfaces
{
    
    public interface IMainWindowViewModel :IViewModel
    {
        ReactiveProperty<IScreenModel> ScreenModel { get; } 
    }
}
