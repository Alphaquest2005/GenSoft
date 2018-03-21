using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;

namespace RevolutionEntities
{
    public class AddinAction:IAddinAction
    {
        public AddinAction(string action, string addin)
        {
            Action = action;
            Addin = addin;
        }

        public string Action { get; }
        public string Addin { get; }
    }
}
