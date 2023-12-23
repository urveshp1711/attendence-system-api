namespace UAS.Entity
{
   
    //public class EntityUser
    //{
    //    public string? userCode { get; set; }
    //    public string? password { get; set; }
    //}
    public class CommandResult
    {
        public bool flag { get; set; }
        public long errorCode { get; set; }
        public string? errorMessage { get; set; }
    }
}
