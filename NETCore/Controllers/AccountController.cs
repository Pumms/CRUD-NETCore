using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCore.Context;
using NETCore.Model;
using NETCore.Repository.Data;
using NETCore.ViewModel;

namespace NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly MyContext _myContext;
        private readonly AccountRepository _repository;
        public IConfiguration _configuration;

        public AccountController(
            IConfiguration config,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            MyContext myContext,
            AccountRepository repository)
        {
            _configuration = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _myContext = myContext;
            this._repository = repository;
        }

        [Route("Register")]
        [HttpPost]
        public async Task<ActionResult> Register(EmployeeVM model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            
            if (user == null)
            {
                if (ModelState.IsValid)
                {
                    var users = new User
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    var result = await _userManager.CreateAsync(users, model.Password);

                    if (result.Succeeded) //Kalo Insert ke AspNetUsers berhasil
                    {
                        await _userManager.AddToRoleAsync(users, "User"); //Merelasikan iduser dengan idrole
                        await _repository.InsertEmployee(model); //Input Data Employee
                    }
                }
                return Ok("Register Success!"); //Jika berhasil akan menampilkan email yang diinput
            }
            else
            {
                return BadRequest("Email already Register!"); //Jika Email telah terdaftar
            }
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult> Login(EmployeeVM model)
        {
            var user = await _userManager.FindByNameAsync(model.Email); //Cek Email on Tbl AspNetUser
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password)) //Cek user null atau tidak dan cek password benar atau tidak
            {
                var users = await GetUser(model.Email); //Cek Email on Tbl Employee
                if (user != null)
                {
                    UserVM role = null;
                    IEnumerable<UserVM> roles = await _repository.GetRole(model); //Cek Email on Tbl Employee
                    foreach (UserVM get in roles)
                    {
                        role = get;
                    }
                    string FullName = users.FirstName + ' ' + users.LastName;
                    //JWT
                    var claims = new[] {
                    new Claim("FullName", FullName),
                    new Claim("Email", users.Email),
                    new Claim("Role", role.Role)
                   };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn); //To Create Token JWT
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token)); //Return token after login
                    //End JWT
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<Employee> GetUser(string email)
        {
            return await _myContext.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}
