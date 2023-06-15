using ArcTicketSystem.API.Data;
using ArcTicketSystem.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArcTicketSystem.API.Data;
using ArcTicketSystem.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArcTicketSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TicketContext context;
        public UserController(TicketContext _context)
        {
            context = _context;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginPage login)
        {
            var _user = context.Users.FirstOrDefault(o => o.Username == login.Username && o.Password == login.Password);
            if (_user == null)
            {
                return Unauthorized();
            }

            if (_user.Userrole == "Admin")
            {
                var Claims = new List<Claim>
            {
                new Claim  ("type",_user.Userrole.ToString()),
                new Claim  ("username",_user.Username.ToString()),
                new Claim  ("userid",_user.Userid.ToString())
            };

                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));

                var Token = new JwtSecurityToken(
                    "https://fbi-demo.com",
                    "https://fbi-demo.com",
                    Claims,
                    expires: DateTime.Now.AddDays(30.0),
                    signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
                );
                var tokenHandler = new JwtSecurityTokenHandler();
                var stringToken = tokenHandler.WriteToken(Token);

                return Ok(new { token = stringToken });

                //return new OkObjectResult(new JwtSecurityTokenHandler().WriteToken(Token));


            }
            else
            {
                var Claims = new List<Claim>
            {
                new Claim  ("type","User"),
                new Claim  ("username",_user.Userrole.ToString()),
                new Claim  ("userid",_user.Userid.ToString())
            };

                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));

                var Token = new JwtSecurityToken(
                    "https://fbi-demo.com",
                    "https://fbi-demo.com",
                    Claims,
                    expires: DateTime.Now.AddDays(30.0),
                    signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
                );
                var tokenHandler = new JwtSecurityTokenHandler();
                var stringToken = tokenHandler.WriteToken(Token);

                return Ok(new { token = stringToken });

                //return new OkObjectResult(new JwtSecurityTokenHandler().WriteToken(Token));

            }


        }
    }
}

