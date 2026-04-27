using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Company.Core.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public decimal Salary { get; set; }

        public string Department { get; set; }
    }
}
