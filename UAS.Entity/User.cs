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
    }

    public class RS_UserLogin
    {
        public string userCode { get; set; }
        public string jwtToken { get; set; }
        public bool isSuccess { get; set; }
    }

}
