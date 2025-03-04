using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDoWorksListAPI.Models;

namespace ToDoWorksListAPI.Controllers
{
    /// <summary>
    /// Аутентификация/авторизация пользователей
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : Controller
    { 
        private IConfiguration _configuration; 
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManage;
        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManage, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManage = roleManage;
        }
        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("get_users")]
        public IEnumerable<User> GetUsers()
        {
            return _userManager.Users.ToList();
        }
        /// <summary>
        /// Получение списка ролей
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("get_roles")]
        public IEnumerable<IdentityRole> GetRoles()
        {
            return _roleManage.Roles.ToList();
        }
        /// <summary>
        /// Получение токена для авторизации (доступно для любого пользователя)
        /// </summary>
        /// <returns>Возвращает bearer token</returns>s
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginUserModel loginUserModel)
        {
            var user = await _userManager.FindByNameAsync(loginUserModel.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginUserModel.Password))
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName), new Claim(ClaimTypes.Email, user.Email), new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault()) };
                var tokenSettings = _configuration.GetSection("tokenSettings");
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    issuer: tokenSettings["Issuer"],
                    audience: tokenSettings["Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }
            else
                return Unauthorized();
        }
        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <param name="Name">Имя пользователя</param>
        /// <param name="Email">Почта</param>
        /// <param name="Password">Пароль</param>
        /// <returns></returns>
        [HttpPost, Route("add_user")]
        [Authorize(Roles = "admin")]
        public async Task Create(string Name, string Email, string Password, string Role)
        {
            User user = new User { Email = Email, UserName = Name};

            if (await _roleManage.FindByNameAsync(Role) == null)
            {
                await _roleManage.CreateAsync(new IdentityRole(Role));
            }

            if (await _userManager.FindByNameAsync(user.UserName) == null)
            {
                IdentityResult result = await _userManager.CreateAsync(user, Password); 
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Role);
                }
            }
        }
    }
}
