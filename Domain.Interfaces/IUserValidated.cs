using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;
using Common.DataEntites;


namespace Domain.Interfaces
{
    public interface IUserValidated : IProcessSystemMessage
    {
        IDynamicEntity UserSignIn { get; }
    }
}
