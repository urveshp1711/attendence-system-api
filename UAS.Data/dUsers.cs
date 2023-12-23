//using MySql.Data.MySqlClient;
using System.Data;
using UAS.Database;
using UAS.Dependancies.Data;
using UAS.Dependancies.Database;

namespace UAS.Data
{
    public class dUsers : IdUsers
    {
        private readonly IMySQLHelper _mySqlHelper;
        public dUsers(IMySQLHelper mySqlHelper)
        {
            _mySqlHelper = mySqlHelper;
        }

        public bool validateUser(string userName, string password)
        {
            try
            {
                //MySqlParameter pUserName = new MySqlParameter();
                //pUserName.ParameterName = "username";
                //pUserName.Value = userName.ToString();

                //MySqlParameter pPassword = new MySqlParameter();
                //pPassword.ParameterName = "password";
                //pPassword.Value = password.ToString();

                _mySqlHelper.AddMySQLParameter("@userName", userName);
                _mySqlHelper.AddMySQLParameter("@pass", password);

                long res = (long)_mySqlHelper.ExecuteScalar("validateUser", []);
                return res.ToString() == "1";

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
