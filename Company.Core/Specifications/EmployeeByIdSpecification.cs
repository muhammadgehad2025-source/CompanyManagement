using Company.Core.Entities;

namespace Company.Core.Specifications
{
    public class EmployeeByIdSpecification : BaseSpecification<Employee>
    {
        public EmployeeByIdSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Department);
        }
    }
}