using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    
    public interface ISigninViewModel: IEntityViewModel<ISignInInfo>
    {
    }
}
