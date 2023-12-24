using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAS.Entity
{
    public class JwtTokens
    {
        public string accessJwt { get; set; }
        public string refreshJwt { get; set; }
    }

    public class UserTokenEntity
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string groupId { get; set; }
        public string locationId { get; set; }
        public long tokenExpireInMin { get; set; }
        public string isSuperAdmin { get; set; }
    }
}
