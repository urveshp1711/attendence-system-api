//using MySql.Data.MySqlClient;
using System.Data;
using UAS.Database;
using UAS.Dependancies.Data;
using UAS.Dependancies.Database;
using UAS.Entity;

namespace UAS.Data
{
    public class dUsers : IdUsers
    {
        private readonly IMySQLHelper _mySqlHelper;
        public dUsers(IMySQLHelper mySqlHelper)
        {
            _mySqlHelper = mySqlHelper;
        }

        public RS_UserInfo? getUserInfo(string userCode)
        {
            try
            {
                _mySqlHelper.AddMySQLParameter("@userCode", userCode);
                var response = _mySqlHelper.ReturnWithDataTable("uas_getUserInfo", []);
                if (response.Rows.Count > 0)
                {
                    return new RS_UserInfo
                    {
                        userCode = userCode,
                        userName = Convert.ToString(response.Rows[0]["ECODE"]),
                        mobile = Convert.ToString(response.Rows[0]["ENAME"]),
                        email = Convert.ToString(response.Rows[0]["EMAIL"]),
                        profilePic = Convert.ToString(response.Rows[0]["profilePic"])
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void updateUserInfo(RQ_UserProfile userInfo)
        {
            try
            {
                _mySqlHelper.AddMySQLParameter("@userCode", userInfo.userCode);
                _mySqlHelper.AddMySQLParameter("@profilePic", userInfo.profilePic);

                _mySqlHelper.ExecuteNonQuery("uas_updateUserProfile", []);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public RS_UserProfile validateUser(string userCode, string password)
        {
            try
            {
                _mySqlHelper.AddMySQLParameter("@userCode", userCode);
                _mySqlHelper.AddMySQLParameter("@pass", password);

                DataTable response = _mySqlHelper.ReturnWithDataTable("validateUser", []);
                return new RS_UserProfile()
                {
                    userCode = userCode,
                    profilePic = Convert.ToString(response.Rows[0]["profilePic"])
                };

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
