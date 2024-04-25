using api.todo.Model;
using api.todo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace api.todo.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IServiceUsers _service;

        public UserController(IConfiguration config, IServiceUsers service)
        {
            _service = service;
            _config = config;
        }

        [Authorize]
        [HttpGet("InquiryUserDetails/{id}")]
        [ProducesResponseType(typeof(ApiModel<User>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiModel<object>), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiModel<object>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> InquiryUserDetails()
        {
            ApiModel<User> data = new ApiModel<User>();

            return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, data));
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(ApiModel<Auth>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiModel<object>), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiModel<object>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] AuthRq request)
        {
            ApiModel<Auth> response = new ApiModel<Auth>();
            response.Data = new Auth();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(5),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
            response.Data.Token = token;
            return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, response));
        }
    }
}
