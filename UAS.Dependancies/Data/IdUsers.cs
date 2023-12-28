
using UAS.Entity;

namespace UAS.Dependancies.Data
{
    public interface IdUsers
    {
        bool validateUser(string userName, string password);
        RS_UserInfo? getUserInfo(string userCode);
    }
}
