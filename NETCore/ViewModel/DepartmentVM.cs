using NETCore.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.ViewModel
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<DateTimeOffset> CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
    }

    public class DepartmentJson
    {
        [JsonProperty("data")]
        public IList<DepartmentVM> data { get; set; }
    }
}
