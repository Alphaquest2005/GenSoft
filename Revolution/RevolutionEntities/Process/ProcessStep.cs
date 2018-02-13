using SystemInterfaces;
using GenSoft.Entities;

namespace RevolutionEntities.Process
{
    public class Process : IProcess
    {
        public Process(int id, ISystemProcess parentProcess, string name, string description, string symbol, IUser user, IApplet applet)
        {
            Id = id;
            ParentProcess = parentProcess;
            Name = name;
            Description = description;
            Symbol = symbol;
            User = user;
            Applet = applet;
        }

        public Process(ISystemProcessInfo systemProcessInfo, IUser user, IApplet applet)
        {
            Id = systemProcessInfo.Id;
            ParentProcess = systemProcessInfo.ParentProcess;
            Name = systemProcessInfo.Name;
            Description = systemProcessInfo.Description;
            Symbol = systemProcessInfo.Symbol;
            User = user;
            Applet = applet;
        }

 

        public int Id { get; }
        public ISystemProcess ParentProcess { get; set; }
        public string Name { get; }
        public string Description { get; }
        public string Symbol { get; }
        public IUser User { get; }
        public IApplet Applet { get; }
    }
}
