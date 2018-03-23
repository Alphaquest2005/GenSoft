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
        public AddinAction(string action, string addin, string name)
        {
            Action = action;
            Addin = addin;
            Name = name;
        }

        public string Action { get; }
        public string Addin { get; }
        public string Name { get; }
    }
}
