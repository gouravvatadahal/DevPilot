using DevPilot.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace DevPilot.API
{
    public class UsersController : ApiController
    {
        public IHttpActionResult Get()
        {
            var users = new List<Users>
            {
                new Users { Id = 1, Name = "Alice", AccessLevel = "Admin" },
                new Users { Id = 2, Name = "Bob", AccessLevel = "Editor" },
                new Users { Id = 3, Name = "Charlie", AccessLevel = "Viewer" }
            };

            return Ok(users);
        }
    }
}