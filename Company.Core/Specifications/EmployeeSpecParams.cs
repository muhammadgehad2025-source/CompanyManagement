namespace Company.Core.Specifications
{
    public class EmployeeSpecParams
    {
        public int? DepartmentId { get; set; }

        public string? Search { get; set; }

        public string? Sort { get; set; }

        private const int MaxPageSize = 50;

        private int pageIndex = 1;
        public int PageIndex
        {
            get => pageIndex;
            set => pageIndex = value < 1 ? 1 : value;
        }

        private int pageSize = 5;
        public int PageSize
        {
            get => pageSize;
            set
            {
                if (value < 1)
                    pageSize = 5;
                else if (value > MaxPageSize)
                    pageSize = MaxPageSize;
                else
                    pageSize = value;
            }
        }
    }
}