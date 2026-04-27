using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

public class CreateEmployeeDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Range(1, 100000)]
    public decimal Salary { get; set; }

    [Required]
    public int DepartmentId { get; set; }
}