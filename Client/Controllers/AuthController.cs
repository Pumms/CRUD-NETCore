using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using NETCore.ViewModel;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class AuthController : Controller
    {
        private HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:44375/api/")
        };

        public IActionResult Not_Found()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RegisterEmp(EmployeeVM model)
        {
            var myContent = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PostAsync("Account/Register", byteContent).Result;
            return Json(result);
        }

        [HttpPost]
        public IActionResult Login(EmployeeVM model)
        {
            var myContent = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PostAsync("Account/Login", byteContent).Result;

            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync().Result;

                var handler = new JwtSecurityTokenHandler();
                var tokens = handler.ReadJwtToken(data);
                var token = "Bearer " + data;

                string role = tokens.Claims.First(claim => claim.Type == "Role").Value;
                string email = tokens.Claims.First(claim => claim.Type == "Email").Value;
                string FName = tokens.Claims.First(claim => claim.Type == "FullName").Value;

                HttpContext.Session.SetString("FullName", FName);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("Email", email);
                HttpContext.Session.SetString("JWToken", token);

                if (role == "Admin")
                {
                    return RedirectToAction("Index", "Department");
                }
                else if(role == "User")
                {
                    return RedirectToAction("Index", "User");
                }

                return View(result);
            }
            else
            {
                return View(result);
            }

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("JWToken");

            return RedirectToAction("Login", "Auth");
        }
    }
}