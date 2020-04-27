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
                var spName = "SP_ViewEmployee";

                var employees = await connection.QueryAsync<EmployeeVM>(spName, commandType: CommandType.StoredProcedure);
                return employees;
            }
        }

        public async Task<IEnumerable<ChartVM>> GetJmlEmployee()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var spName = "SP_Chart_CountEmployee";
                var employees = await connection.QueryAsync<ChartVM>(spName, commandType: CommandType.StoredProcedure);
                return employees;
            }
        }

        public async Task<Employee> GetByEmail(string email)
        {
            return await _myContext.Set<Employee>().FindAsync(email);
        }
    }
}
