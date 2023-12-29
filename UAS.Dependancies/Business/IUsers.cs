using UAS.Entity;

namespace UAS.Dependancies.Business
{
    public interface IUsers
    {
        RS_UserProfile validateUser(string userName, string password);
        RS_UserInfo? getUserInfo(string userCode);
        void updateUserInfo(RQ_UserProfile userProfile);
    }
}
