using Microsoft.EntityFrameworkCore;
using EmployeeModel = Employee.API.Models.Employee;

namespace Employee.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<EmployeeModel> Employees { get; set; }
    }
}