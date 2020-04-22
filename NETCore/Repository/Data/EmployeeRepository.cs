using Dapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly MyContext _myContext;
        public EmployeeRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            _configuration = configuration;
            _myContext = myContext;
        }

        DynamicParameters parameters = new DynamicParameters();

        IConfiguration _configuration { get; }

        public async Task<Employee> Delete(string email)
        {
            var entity = await GetByEmail(email);
            if (entity == null)
            {
                return entity;
            }
            entity.IsDelete = true;
            entity.DeleteDate = DateTimeOffset.Now;
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<EmployeeVM>> GetAllEmployee()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_ViewEmployee";

                var employees = await connection.QueryAsync<EmployeeVM>(procName, commandType: CommandType.StoredProcedure);
                return employees;
            }
        }

        public async Task<Employee> GetByEmail(string email)
        {
            return await _myContext.Set<Employee>().FindAsync(email);
        }

        //public async Task<IEnumerable<EmployeeVM>> GetEmployee(string Email)
        //{
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
        //    {
        //        var spName = "SP_DetailEmployee";
        //        parameters.Add("@Email", Email);
        //        var data = await connection.QueryAsync<EmployeeVM>(spName, parameters, commandType: CommandType.StoredProcedure);
        //        return data;
        //    }
        //}

        public async Task<IEnumerable<EmployeeVM>> GetEmailEmp(string email)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var spName = "SP_RetrieveByEmail_TB_M_Employee";
                parameters.Add("@Email", email);
                var data = await connection.QueryAsync<EmployeeVM>(spName, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }
    }
}
