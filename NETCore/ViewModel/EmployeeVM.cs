using NETCore.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.ViewModel
{
    public class EmployeeVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<DateTimeOffset> CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public int Department_Id { get; set; }
        public string DepartmentName { get; set; }

        public class EmployeeJson
        {
            [JsonProperty("data")]
            public IList<EmployeeVM> data { get; set; }
        }
    }
}
