using MySql.Data.MySqlClient;
using System.Data;
using UAS.Entity;

namespace UAS.Data
{
    public class User
    {
        public class dUser 
        {
            private readonly IMySQLHelper _sqlHelper;

            public dUser(IMySQLHelper sqlHelper)
            {
                _sqlHelper = sqlHelper;
                //_claim = claim;
            }

            public bool isValidUser(string? userName, string? password)
            {
                try
                {
                    MySqlParameter p_userCode = new MySqlParameter();
                    p_userCode.ParameterName = "@usercode";
                    p_userCode.Value = userName;

                    MySqlParameter p_password = new MySqlParameter();
                    p_password.ParameterName = "@pass";
                    p_password.Value = userName;

                    int res = (int)_sqlHelper.ReturnScalarValue([p_userCode, p_password], "validateUser");
                    return res != 0;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}


