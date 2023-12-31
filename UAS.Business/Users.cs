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

        public RS_UserProfile validateUser(string userName, string password)
        {
            return _dUsers.validateUser(userName, password);
        }

        public RS_UserInfo? getUserInfo(string userCode)
        {
            return _dUsers.getUserInfo(userCode);
        }

        public void updateUserInfo(RQ_UserProfile userProfile)
        {
            _dUsers.updateUserInfo(userProfile);
        }

        public RS_UserAttendance doUserAttendance(RQ_UserAttendance userAttendance)
        {
            return _dUsers.doUserAttendance(userAttendance);
        }
    }
}
