using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace RPI.Core.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Devices = new HashSet<IOTDevice>();
        }

        public ICollection<IOTDevice> Devices { get; set; }

        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}
