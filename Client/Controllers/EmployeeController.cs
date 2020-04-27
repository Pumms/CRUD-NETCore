using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.ViewModel;
using Newtonsoft.Json;
using static NETCore.ViewModel.EmployeeVM;

namespace Client.Controllers
{
    public class EmployeeController : Controller
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
                return RedirectToAction("Not_Found", "Auth");
            }
            else
            {
                return RedirectToAction("Not_Found", "Auth");
            }
        }

        public JsonResult LoadEmployee()
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            EmployeeJson data = null;
            var responseTask = client.GetAsync("Employee");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                data = JsonConvert.DeserializeObject<EmployeeJson>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry Server Error, Try Again");
            }

            return Json(data);
        }

        public JsonResult Update(EmployeeVM employee)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var myContent = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PutAsync("Employee/" + employee.Email, byteContent).Result;
            return Json(result);
        }

        public JsonResult GetByEmail(string email)
        {
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

        public JsonResult Delete(string email)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var result = client.DeleteAsync("Employee/" + email).Result;
            return Json(result);
        }

        public JsonResult GetChart()
        {
            IEnumerable<ChartVM> chartInfo = null;
            List<ChartVM> chartData = new List<ChartVM>();
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var responseTask = client.GetAsync("Employee/Chart");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<ChartVM>>(); //Get all the data from the API
                readTask.Wait();
                chartInfo = readTask.Result;
                foreach (var item in chartInfo)
                {
                    ChartVM data = new ChartVM();
                    data.label = item.label;
                    data.value = item.value;
                    chartData.Add(data);
                }
                var json = JsonConvert.SerializeObject(chartData, Formatting.Indented);
                return Json(json);
            }
            return Json("internal server error");
        }
    }
}