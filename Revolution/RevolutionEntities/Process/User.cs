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
}
