using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UAS.Data
{
    public interface IClaimManager
    {
        string? GetUserId();
        string? GetUserName();
        string? GetFirstName();
        string? GetLastName();
        string? GetgroupId();
        string? GetLocationId();
        string? GetIsSuperAdmin();
        string? GetClientID();
        string? GetUserAgent();
    }
    public class ClaimManager : IClaimManager
    {
        private readonly ClaimsPrincipal _claimsPrincipal;
        public ClaimManager(ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal;
        }
        public string? GetUserId()
        {
            if (_claimsPrincipal == null)
                throw new ArgumentNullException(nameof(_claimsPrincipal));
            else
            {
                return _claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();
            }
        }
        public string? GetUserName()
        {
            if (_claimsPrincipal == null)
                throw new ArgumentNullException(nameof(_claimsPrincipal));

            return _claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
        }
        public string? GetFirstName()
        {
            if (_claimsPrincipal == null)
                throw new ArgumentNullException(nameof(_claimsPrincipal));

            return _claimsPrincipal.FindFirst("firstName")?.Value;
        }
        public string? GetLastName()
        {
            if (_claimsPrincipal == null)
                throw new ArgumentNullException(nameof(_claimsPrincipal));

            return _claimsPrincipal.FindFirst("lastName")?.Value;
        }
        public string? GetgroupId()
        {
            if (_claimsPrincipal == null)
                throw new ArgumentNullException(nameof(_claimsPrincipal));

            return _claimsPrincipal.FindFirst("groupId")?.Value.ToString();
        }
        public string? GetLocationId()
        {
            if (_claimsPrincipal == null)
                throw new ArgumentNullException(nameof(_claimsPrincipal));

            return _claimsPrincipal.FindFirst("locationId")?.Value.ToString();
        }
        public string? GetIsSuperAdmin()
        {
            if (_claimsPrincipal == null)
                throw new ArgumentNullException(nameof(_claimsPrincipal));

            return _claimsPrincipal.FindFirst("isSuperAdmin")?.Value.ToString();
        }
        public string? GetClientID()
        {
            if (_claimsPrincipal == null)
                throw new ArgumentNullException(nameof(_claimsPrincipal));

            return _claimsPrincipal.FindFirst("ClientId")?.Value;
        }
        public string? GetUserAgent()
        {
            if (_claimsPrincipal == null)
                throw new ArgumentNullException(nameof(_claimsPrincipal));

            return _claimsPrincipal.FindFirst("Agent")?.Value;
        }
    }
}
