using System.Collections.Generic;
using System.ComponentModel;

namespace UAS.Business
{
    public interface IUser
    {
        public bool isValidUser(string user, string password);
    }


    public class User : IUser
    {
        private readonly IUser _user;
        public User(IUser user)
        {
            _user = user;
        }

        public bool isValidUser(string userCode, string password)
        {
            Console.Write("Hellow");
            return true;
        }
    }
}
