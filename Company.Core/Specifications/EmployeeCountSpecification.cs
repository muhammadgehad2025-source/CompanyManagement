using Company.Core.Entities;

namespace Company.Core.Specifications
{
    public class EmployeeCountSpecification : BaseSpecification<Employee>
    {
        public EmployeeCountSpecification(EmployeeSpecParams specParams)
            : base(x =>
                (string.IsNullOrEmpty(specParams.Search) ||
                 x.Name.ToLower().Contains(specParams.Search)) &&

                (!specParams.DepartmentId.HasValue ||
                 x.DepartmentId == specParams.DepartmentId))
        {
        }
    }
}