using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Core.Specifications
{
    public class EmployeeSpecParams
    {
        public int? DepartmentId { get; set; }

        public string? Search { get; set; }

        public string? Sort { get; set; }

        private const int MaxPageSize = 50;

        public int PageIndex { get; set; } = 1;

        private int pageSize = 5;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
