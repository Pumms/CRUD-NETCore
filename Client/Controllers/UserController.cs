using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class UserController : Controller
    {
        private HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:44375/api/")
        };

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == "Admin")
            {
                ViewData["Name_User"] = HttpContext.Session.GetString("FullName");
                ViewData["Email"] = HttpContext.Session.GetString("Email");
                return View();
            }
            else if (role == "User")
            {
                ViewData["Name_User"] = HttpContext.Session.GetString("FullName");
                ViewData["Email"] = HttpContext.Session.GetString("Email");
                return View();
            }
            else
            {
                return RedirectToAction("Not_Found", "Auth");
            }
        }

        public JsonResult LoadDataUser()
        { 
            string email = HttpContext.Session.GetString("Email");
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            object data = null;
            var responseTask = client.GetAsync("Employee/" + email);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                data = JsonConvert.SerializeObject(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry Server Error, Try Again");
            }

            return Json(data);
        }
    }
}