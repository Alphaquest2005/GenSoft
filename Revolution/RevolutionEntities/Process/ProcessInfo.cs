using SystemInterfaces;

namespace RevolutionEntities.Process
{
    public class SystemProcessInfo : ISystemProcessInfo
    {
        public SystemProcessInfo(){}
        public SystemProcessInfo(int id, ISystemProcess parentProcess, string name, string description, string symbol, string userId, IApplet applet)
        {
            Id = id;
            ParentProcess = parentProcess;
            Name = name;
            Description = description;
            Symbol = symbol;
            UserId = userId;
            Applet = applet;
        }

        public int Id { get; }
        public ISystemProcess ParentProcess { get; set; }
        public string Name { get; }
        public string Description { get; }
        public string Symbol { get; }
        public string UserId { get; }
        public IApplet Applet { get; }
    }

  
}