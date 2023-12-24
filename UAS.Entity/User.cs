namespace UAS.Entity
{
    public enum JWTType
    {
        Access = 1,
        Refresh = 2
    }

    public class RQ_ValidateUser
    {
        public required string userCode { get; set; } = "1043";
        public required string password { get; set; } = "051050054053";
    }

    public class RS_ValidateUser
    {
        public string userCode { get; set; }
        public string jwtToken { get; set; }
        public string error { get; set; }
    }

}
