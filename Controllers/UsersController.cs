using WebBlogApi.Data.DTO;
using WebBlogApi.Data.Interfaces;
using WebBlogApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace WebBlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;

        public UsersController(IUnitOfWork uow, IConfiguration configuration)
        {
            this.uow = uow;
            this.configuration = configuration;
        }

        [HttpGet("fetchBlogs")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await uow.BlogRepository.GetAllBlogsAsync();
            return Ok(blogs);
        }

        //[HttpGet("importBlogs")]
        //public UrlBlogs ImportBlogs(string url)
        //{
        //    var blogs = uow.BlogRepository.GetUrlBlogs(url);
        //    return blogs;
        //}

        [HttpGet("importBlogs")]
        public async Task<IActionResult> ImportBlogs(string url)
        {
            UrlBlogs urlBlogs = new UrlBlogs();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    urlBlogs = JsonConvert.DeserializeObject<UrlBlogs>(apiResponse);
                }
            }
            return Ok(urlBlogs);
        }

        [HttpGet("fetchBlogsById/{UserId}")]
        public async Task<IActionResult> FetchClientBlogs(int UserId)
        {
            var client = await uow.BlogRepository.FindClientBlogs(UserId);
            return Ok(client);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginReqDTO loginReq)
        {
            var user = await uow.UserRepository.Authenticate(loginReq.UserName, loginReq.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            var loginRes = new LoginResDTO();
            loginRes.UserName = user.Username;
            loginRes.Token = CreateJWT(user);
            return Ok(loginRes);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginReqDTO loginReq)
        {
            if (await uow.UserRepository.UserAlreadyExist(loginReq.UserName))
                return BadRequest("Username Already exist");
            uow.UserRepository.Register(loginReq.UserName, loginReq.Password);
            await uow.SaveAsync();
            return StatusCode(201); 
        }

        [HttpGet]
        public string CreateJWT(User user)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = signingCreds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);


        }
    }
}
