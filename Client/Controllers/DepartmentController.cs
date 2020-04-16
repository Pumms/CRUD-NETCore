using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NETCore.Model;
using NETCore.ViewModel;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class DepartmentController : Controller
    {
        readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:44375/api/")
        };

        // GET: Department
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult LoadDepartment()
        {
            DepartmentJson datadept = null;
            var responseTask = client.GetAsync("Department");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                datadept = JsonConvert.DeserializeObject<DepartmentJson>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry Server Error, Try Again");
            }

            return Json(datadept);
        }

        public JsonResult InsertorUpdate(DepartmentVM department)
        {
            var myContent = JsonConvert.SerializeObject(department);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (department.Id == 0)
            {
                var result = client.PostAsync("Department", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("Department/" + department.Id, byteContent).Result;
                return Json(result);
            }
        }

        public JsonResult GetById(int Id)
        {
            object datadept = null;
            var responseTask = client.GetAsync("Department/" + Id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                datadept = JsonConvert.SerializeObject(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry Server Error, Try Again");
            }

            return Json(datadept);
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Department/" + Id).Result;
            return Json(result);
        }

        //Export
        //public ActionResult ExportToPDF(Department department)
        //{
        //    DepartmentReport deptreport = new DepartmentReport();
        //    byte[] abytes = deptreport.PrepareReport(GetDepartment());
        //    return File(abytes, "application/pdf");
        //}

        //public List<Department> GetDepartment()
        //{
        //    IEnumerable<Department> datadept = null;
        //    var responseTask = client.GetAsync("Department");
        //    responseTask.Wait();
        //    var result = responseTask.Result;
        //    if (result.IsSuccessStatusCode)
        //    {
        //        var readTask = result.Content.ReadAsAsync<IList<Department>>();
        //        readTask.Wait();
        //        datadept = readTask.Result;
        //    }
        //    else
        //    {
        //        datadept = Enumerable.Empty<Department>();
        //        ModelState.AddModelError(string.Empty, "Sorry Server Error, Try Again");
        //    }

        //    return datadept.ToList();
        //}

        //public ActionResult ExportToExcel()
        //{
        //    var comlumHeaders = new string[]
        //    {
        //        "Id",
        //        "Name Department",
        //        "Tanggal Dibuat",
        //        "Tanggal Diubah"
        //    };

        //    byte[] result;

        //    using (var package = new ExcelPackage())
        //    {
        //        // add a new worksheet to the empty workbook

        //        var worksheet = package.Workbook.Worksheets.Add("Department List"); //Worksheet name
        //        using (var cells = worksheet.Cells[1, 1, 1, 4]) //(1,1) (1,5)
        //        {
        //            cells.Style.Font.Bold = true;
        //        }

        //        //First add the headers
        //        for (var i = 0; i < comlumHeaders.Count(); i++)
        //        {
        //            worksheet.Cells[1, i + 1].Value = comlumHeaders[i];
        //        }

        //        //Add values
        //        var j = 2;
        //        foreach (var dept in GetDepartment())
        //        {
        //            worksheet.Cells["A" + j].Value = dept.Id;
        //            worksheet.Cells["B" + j].Value = dept.Name;
        //            worksheet.Cells["C" + j].Value = dept.CreateDate.ToString();
        //            worksheet.Cells["D" + j].Value = dept.UpdateDate.ToString();
        //            j++;
        //        }
        //        result = package.GetAsByteArray();
        //    }

        //    return File(result, "application/ms-excel", $"DepartmentList.xlsx");
        //}

        //public ActionResult ExportToCSV()
        //{
        //    var comlumHeaders = new string[]
        //    {
        //        "Id",
        //        "Name Department",
        //        "Tanggal Dibuat",
        //        "Tanggal Diubah"
        //    };

        //    var deptRecords = (from dept in GetDepartment()
        //                       select new object[]
        //                       {
        //                                    dept.Id,
        //                                    dept.Name,
        //                                    dept.CreateDate.ToString(),
        //                                    dept.UpdateDate.ToString()
        //                       }).ToList();

        //    // Build the file content
        //    var deptcsv = new StringBuilder();
        //    deptRecords.ForEach(line =>
        //    {
        //        deptcsv.AppendLine(string.Join(",", line));
        //    });

        //    byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", comlumHeaders)}\r\n{deptcsv.ToString()}");
        //    return File(buffer, "text/csv", $"DepartmentList.csv");
        //}
    }
}