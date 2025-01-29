using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCrud.Employees
{
    public record EmployeeDto(Guid Id, string Name);
}