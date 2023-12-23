using System.Reflection.Metadata.Ecma335;
using UAS.Data;
using UAS.Dependancies.Business;
using UAS.Dependancies.Data;

namespace UAS.Business
{
    public class Users : IUsers
    {
        private readonly IdUsers _dUsers;
        public Users(IdUsers users)
        {
            _dUsers = users;
        }

        public bool validateUser(string userName, string password)
        {
            return _dUsers.validateUser(userName, password);
        }
    }
}
