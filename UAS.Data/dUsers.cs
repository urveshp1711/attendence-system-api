using MySql.Data.MySqlClient;
using System.Data;
using UAS.Database;
using UAS.Dependancies.Data;
using UAS.Dependancies.Database;

namespace UAS.Data
{
    public class dUsers : IdUsers
    {
        IMySQLHelper _mySQLHelper;
        dUsers(IMySQLHelper mySQLHelper)
        {
            _mySQLHelper = mySQLHelper;
        }

        public bool validateUser(string userName, string password)
        {
            try
            {
                MySqlParameter pUserName = new MySqlParameter();
                pUserName.ParameterName = "username";
                pUserName.Value = userName.ToString();

                MySqlParameter pPassword = new MySqlParameter();
                pPassword.ParameterName = "password";
                pPassword.Value = password.ToString();

                int res = (int)_mySQLHelper.ExecuteNonQuery([pUserName, pPassword], "validateUser");
                return res == 1;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
