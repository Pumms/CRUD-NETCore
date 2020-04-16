using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NETCore.ViewModel;
using Newtonsoft.Json;
using static NETCore.ViewModel.EmployeeVM;

namespace Client.Controllers
{
    public class EmployeeController : Controller
    {
        readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:44375/api/")
        };

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult LoadEmployee()
        {
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

        public JsonResult InsertorUpdate(EmployeeVM employee)
        {
            var myContent = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (employee.Id == 0)
            {
                var result = client.PostAsync("Employee", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("Employee/" + employee.Id, byteContent).Result;
                return Json(result);
            }
        }

        public JsonResult GetById(int Id)
        {
            object data = null;
            var responseTask = client.GetAsync("Employee/" + Id);
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

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Employee/" + Id).Result;
            return Json(result);
        }
    }
}