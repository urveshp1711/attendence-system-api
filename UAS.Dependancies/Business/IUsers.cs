using UAS.Entity;

namespace UAS.Dependancies.Business
{
    public interface IUsers
    {
        RS_UserProfile validateUser(string userName, string password);
        RS_UserInfo? getUserInfo(string userCode);
        IEnumerable<RS_UserAttendanceSummary> getAttendanceSummary(string userCode);
        IEnumerable<RS_UserInfo> getAllUsers();
        void updateUserInfo(RQ_UserProfile userProfile);
        RS_UserAttendance doUserAttendance(RQ_UserAttendance userAttendance);
    }
}
