//using MySql.Data.MySqlClient;
using System.Data;
using UAS.Database;
using UAS.Dependancies.Business;
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

        public RS_UserAttendance doUserAttendance(RQ_UserAttendance userAttendance)
        {
            try
            {
                _mySqlHelper.AddMySQLParameter("@userCode", userAttendance.userCode);
                _mySqlHelper.AddMySQLParameter("@attendancePic", userAttendance.attendancePic);
                _mySqlHelper.AddMySQLParameter("@latitude", userAttendance.latitude);
                _mySqlHelper.AddMySQLParameter("@longitude", userAttendance.longitude);
                _mySqlHelper.AddMySQLParameter("@address", userAttendance.address);
                _mySqlHelper.AddMySQLParameter("@city", userAttendance.city);
                _mySqlHelper.AddMySQLParameter("@country", userAttendance.country);
                _mySqlHelper.AddMySQLParameter("@attendanceDateTime", userAttendance.attendanceDateTime);


                _mySqlHelper.ExecuteNonQuery("uas_createAttendance", []);
                return new RS_UserAttendance
                {
                    userCode = userAttendance.userCode,
                    isSuccess = true
                };

            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<RS_UserInfo> getAllUsers()
        {
            try
            {
                IEnumerable<RS_UserInfo> users = new List<RS_UserInfo>();
                _mySqlHelper.AddMySQLParameter("@userCode", null);
                var dtResponse = _mySqlHelper.ReturnWithDataTable("uas_getUserInfo", []);

                if (dtResponse.Rows.Count > 0)
                {
                    foreach (DataRow item in dtResponse.Rows)
                    {
                        users = users.Append(new RS_UserInfo
                        {
                            userCode = Convert.ToString(item["ECODE"]),
                            userName = Convert.ToString(item["ENAME"]),
                            mobile = Convert.ToString(item["MOBILENO"]),
                            email = Convert.ToString(item["EMAIL"]),
                            profilePic = Convert.ToString(item["profilePic"])
                        });
                    }
                    return users;
                }
                else
                {
                    return users;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<RS_UserAttendanceSummary> getAttendanceSummary(string userCode)
        {
            try
            {
                IEnumerable<RS_UserAttendanceSummary> userAttendance = new List<RS_UserAttendanceSummary>();

                _mySqlHelper.AddMySQLParameter("@userCode", userCode);
                var response = _mySqlHelper.ReturnWithDataTable("uas_getUserAttendance", []);
                if (response.Rows.Count > 0)
                {
                    foreach (DataRow item in response.Rows)
                    {

                        userAttendance = userAttendance.Append(new RS_UserAttendanceSummary
                        {
                            userCode = userCode,
                            address = Convert.ToString(item["address"]),
                            latitude = Convert.ToString(item["latitude"]),
                            longitude = Convert.ToString(item["longitude"]),
                            attendanceDateTime = Convert.ToDateTime(item["attendanceDateTime"]),
                            attendancePic = Convert.ToString(item["attendancePic"]),
                            city = Convert.ToString(item["city"]),
                            country = Convert.ToString(item["country"])
                        });
                    }

                    return userAttendance;
                }
                else
                {
                    return userAttendance;
                }
            }
            catch (Exception)
            {

                throw;
            }
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
                        userName = Convert.ToString(response.Rows[0]["ENAME"]),
                        mobile = Convert.ToString(response.Rows[0]["MOBILENO"]),
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
                    userName = Convert.ToString(response.Rows[0]["ename"]),
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
