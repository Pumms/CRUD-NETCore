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
    public class AccountRepository : GeneralRepository<Employee, MyContext>
    {
        IConfiguration _configuration { get; }
        private readonly MyContext _myContext;
        DynamicParameters parameters = new DynamicParameters();
        public AccountRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            _myContext = myContext;
            _configuration = configuration;
        }

        public async Task<IEnumerable<EmployeeVM>> InsertEmployee(EmployeeVM model)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_InsertEmployee";
                parameters.Add("@Email", model.Email);
                parameters.Add("@FirstName", model.FirstName);
                parameters.Add("@LastName", model.LastName);
                parameters.Add("@BirthDate", model.BirthDate);
                parameters.Add("@PhoneNumber", model.PhoneNumber);
                parameters.Add("@Address", model.Address);
                parameters.Add("@Department_Id", model.Department_Id);
                var create = await connection.QueryAsync<EmployeeVM>(procName, parameters, commandType: CommandType.StoredProcedure);
                return create;
            }
        }
        public async Task<IEnumerable<UserVM>> GetRole(EmployeeVM model)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var spName = "SP_GetRoleName";
                parameters.Add("@Email", model.Email);
                var data = await connection.QueryAsync<UserVM>(spName, parameters, commandType: CommandType.StoredProcedure);
               
                return data;
            }
        }
    }
}
