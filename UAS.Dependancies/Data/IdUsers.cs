
using UAS.Entity;

namespace UAS.Dependancies.Data
{
    public interface IdUsers
    {
        RS_UserProfile validateUser(string userName, string password);
        RS_UserInfo? getUserInfo(string userCode);
        void updateUserInfo(RQ_UserProfile userInfo);
        RS_UserAttendance doUserAttendance(RQ_UserAttendance userAttendance);
    }
}
