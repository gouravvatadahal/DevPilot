using DevPilot.Models;
using LaunchDarkly.Client;
using Microsoft.FeatureManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DevPilot.API
{
    public class UsersController : ApiController
    {
        public async Task<IHttpActionResult> Get()
        {
            var featureManager = (FeatureManager)HttpContext.Current.Application["FeatureManager"];
            if (await featureManager.IsEnabledAsync("NewFeature"))
            {
                var users = new List<Users>
                        {
                            new Users { Id = 1, Name = "Alice", AccessLevel = "Admin" },
                            new Users { Id = 2, Name = "Bob", AccessLevel = "Editor" },
                            new Users { Id = 3, Name = "Charlie", AccessLevel = "Viewer" }
                        };
                return Ok(users);
            }
            else
            {
                return Ok(new List<Users>());
            }
        }
    }
}