using SystemInterfaces;

namespace RevolutionEntities.Process
{
    public class Process : IProcess
    {
        public Process(int id, ISystemProcess parentProcess, string name, string description, string symbol, IUser user)
        {
            Id = id;
            ParentProcess = parentProcess;
            Name = name;
            Description = description;
            Symbol = symbol;
            User = user;
        }

        public Process(ISystemProcessInfo systemProcessInfo, IUser user)
        {
            Id = systemProcessInfo.Id;
            ParentProcess = systemProcessInfo.ParentProcess;
            Name = systemProcessInfo.Name;
            Description = systemProcessInfo.Description;
            Symbol = systemProcessInfo.Symbol;
            User = user;
           
        }

 

        public int Id { get; }
        public ISystemProcess ParentProcess { get; set; }
        public string Name { get; }
        public string Description { get; }
        public string Symbol { get; }
        public IUser User { get; }
        
    }
}
