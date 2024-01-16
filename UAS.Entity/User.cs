#pragma warning disable CS8618
using System.Diagnostics.Metrics;
using System.Net;

namespace UAS.Entity
{
    public enum JWTType
    {
        Access = 1,
        Refresh = 2
    }

    public class RQ_UserLogin
    {
        public required string userCode { get; set; }
        public required string password { get; set; }
        public string email { get; set; }
    }

    public class RQ_UserProfile
    {
        public required string userCode { get; set; }
        public required string profilePic { get; set; }
    }

    public class RQ_UserAttendance
    {
        public required string userCode { get; set; }

        public required string attendancePic { get; set; }

        public required string latitude { get; set; }
        public required string longitude { get; set; }

        public string? address { get; set; }
        public string? city { get; set; }
        public string? country { get; set; }
        public DateTime attendanceDateTime { get; set; }
    }

    public class RS_UserAttendance
    {
        public string userCode { get; set; }

        public bool isSuccess { get; set; }
    }

    public class RS_UserProfile
    {
        public string userCode { get; set; }
        public string? userName { get; set; }
        public string? profilePic { get; set; }
    }

    public class RS_UserLogin
    {
        public string userCode { get; set; }
        public string? userName { get; set; }
        public string? profilePic { get; set; }
        public string jwtToken { get; set; }
        public bool isSuccess { get; set; }
    }

    public class RS_UserInfo
    {
        public string? userCode { get; set; }
        public string? userName { get; set; }
        public string? mobile { get; set; }
        public string? email { get; set; }
        public string? profilePic { get; set; }
    }

    public class RS_UserAttendanceSummary
    {
        public string? userCode { get; set; }
        public string? attendancePic{ get; set; }
        public string? latitude{ get; set; }
        public string? longitude{ get; set; }
        public string? address { get; set; }
        public string? city{ get; set; }
        public string? country { get; set; }
        public DateTime? attendanceDateTime { get; set; }
    }

}
