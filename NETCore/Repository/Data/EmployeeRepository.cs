using Dapper;
using Microsoft.Extensions.Configuration;
using NETCore.Context;
using NETCore.Model;
using NETCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, MyContext>
    {
        public EmployeeRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            _configuration = configuration;
        }

        DynamicParameters parameters = new DynamicParameters();

        IConfiguration _configuration { get; }

        public async Task<IEnumerable<EmployeeVM>> GetAllEmployee()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_ViewEmployee";

                var employees = await connection.QueryAsync<EmployeeVM>(procName, commandType: CommandType.StoredProcedure);
                return employees;
            }

        }
    }
}
