using System.Diagnostics.Contracts;
using SystemInterfaces;


namespace RevolutionEntities.Process
{
    public class User: IUser
    {

        public User(string userId)
        {
            Contract.Requires(!string.IsNullOrEmpty(userId));
           
            UserId = userId;
        }
        
        public string UserId { get; }

    }

    public class Applet : IApplet
    {
        public Applet(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class DbApplet : Applet,IDbApplet
    {
        public DbApplet(string name, string dbName) : base(name)
        {
            DbConnectionString = dbName;
        }

        public string DbConnectionString { get; }
    }
}
