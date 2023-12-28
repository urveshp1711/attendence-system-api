using UAS.Dependancies.Business;
using UAS.Dependancies.Data;
using UAS.Entity;

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

        public RS_UserInfo? getUserInfo(string userCode)
        {
            return _dUsers.getUserInfo(userCode);
        }
    }
}
