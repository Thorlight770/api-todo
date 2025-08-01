using api.todo.Model;
using api.todo.Model.Enum;
using api.todo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        private readonly ILogger<UserController> _logger;

        public UserController(IConfiguration config, ILogger<UserController> logger, IServiceUsers service)
        {
            _service = service;
            _logger = logger;
            _config = config;
        }

        [Authorize]
        [HttpGet("InquiryUserDetails/{id}")]
        [ProducesResponseType(typeof(ApiModel<User>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiModel<object>), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiModel<object>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> InquiryUserDetails(string id)
        {
            _logger.LogInformation("[Request] => {0}", JsonConvert.SerializeObject(id));
            ApiModel<User> response = new ApiModel<User>();
            response.Data = new User();
            response.Data = await _service.GetById(id);
            if (response.Data != null)
            {
                response.TotalPage = 1;
                response.RowPerPage = 1;
                response.PageIndex = 0;
                response.LogReff = Guid.NewGuid().ToString();
            }
            else
            {
                response.Messages.Add(new AdditionalMessage("200", MessageType.ERROR, "Data tidak ditemukan !", ""));
                _logger.LogError("[Error Response] => {0}", JsonConvert.SerializeObject(response.Messages));
            }
            _logger.LogInformation("[Response] => {0}", JsonConvert.SerializeObject(response));
            return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, response));
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(ApiModel<Auth>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiModel<object>), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiModel<object>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] AuthRq request)
        {
            _logger.LogInformation("[Request] => {0}", JsonConvert.SerializeObject(request));
            ApiModel<Auth> response = new ApiModel<Auth>();
            response.Data = new Auth();

            var user = await _service.Login(request.Username ?? "", request.Password ?? "");

            if (user != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? ""));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  null,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
                response.Data.Token = token;
            }
            else
            {
                response.Messages?.Add(new AdditionalMessage(HttpStatusCode.Unauthorized.ToString(), MessageType.ERROR, "Data Tidak Ada Di Database !", "Login"));
                _logger.LogError("[Error Response] => {0}", JsonConvert.SerializeObject(response.Messages));
            }
            _logger.LogInformation("[Response] => {0}", JsonConvert.SerializeObject(user));
            return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, response));
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiModel<User>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiModel<object>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiModel<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] User request)
        {
            Console.WriteLine("[Add Logger][Start] : ", request);
            ApiModel<User> response = new ApiModel<User>();
            response.Data = new User();
            response.Data = await _service.Add(request);
            if (response.Data != null)
            {
                response.TotalPage = 1;
                response.RowPerPage = 1;
                response.PageIndex = 0;
                response.LogReff = Guid.NewGuid().ToString();
            }
            Console.WriteLine("[Add Logger][End] : ", request);
            return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, response));
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(ApiModel<User>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiModel<object>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiModel<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] User request)
        {
            ApiModel<User> response = new ApiModel<User>();
            response.Data = new User();
            response.Data = await _service.Update(request);
            if (response.Data != null)
            {
                response.TotalPage = 1;
                response.RowPerPage = 1;
                response.PageIndex = 0;
                response.LogReff = Guid.NewGuid().ToString();
            }

            return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, response));
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType(typeof(ApiModel<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiModel<object>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiModel<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            ApiModel<bool> response = new ApiModel<bool>();
            response.Data = await _service.Delete(id);
            if (response.Data != null)
            {
                response.TotalPage = 0;
                response.RowPerPage = 0;
                response.PageIndex = 0;
                response.LogReff = Guid.NewGuid().ToString();
            }

            return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, response));
        }

        [HttpGet("list-track")]
        [ProducesResponseType(typeof(ApiModel<List<TrackStep>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiModel<object>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiModel<object>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListTrack([FromQuery] long requestMasterID)
        {
            ApiModel<List<TrackStep>> response = new ApiModel<List<TrackStep>>();
            response.Data = await _service.ListTrack(requestMasterID);
            if (response.Data != null)
            {
                response.TotalPage = 0;
                response.RowPerPage = 0;
                response.PageIndex = 0;
                response.LogReff = Guid.NewGuid().ToString();
            }

            return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, response));
        }
    }
}
