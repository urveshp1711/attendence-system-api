using UAS.Entity;

namespace UAS.Dependancies.Business
{
    public interface IUsers
    {
        bool validateUser(string userName, string password);
        RS_UserInfo? getUserInfo(string userCode);
    }
}
