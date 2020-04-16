using NETCore.Context;
using NETCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.Repository.Data
{
    public class DepartmentRepository : GeneralRepository<Department, MyContext>
    {
        public DepartmentRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
