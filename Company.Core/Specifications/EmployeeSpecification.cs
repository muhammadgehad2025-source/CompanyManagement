using Company.Core.Entities;

namespace Company.Core.Specifications
{
    public class EmployeeSpecification : BaseSpecification<Employee>
    {
        public EmployeeSpecification(EmployeeSpecParams specParams)
            : base(x =>
                (string.IsNullOrEmpty(specParams.Search) ||
                 x.Name.ToLower().Contains(specParams.Search.ToLower())) &&

                (!specParams.DepartmentId.HasValue ||
                 x.DepartmentId == specParams.DepartmentId))
        {
            AddInclude(x => x.Department);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "salaryAsc":
                        AddOrderBy(x => x.Salary);
                        break;

                    case "salaryDesc":
                        AddOrderByDescending(x => x.Salary);
                        break;

                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(x => x.Name);
            }

            ApplyPaging(
                specParams.PageSize * (specParams.PageIndex - 1),
                specParams.PageSize
            );
        }
    }
}