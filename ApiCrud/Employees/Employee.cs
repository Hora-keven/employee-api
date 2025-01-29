using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCrud.Employees
{
    public class Employee
    {
        public Guid Id { get; init; }
        public string Name { get; private set; }
        public bool IsActive {get; private set;}
        public Employee(string name){
            Name = name;
            Id = Guid.NewGuid();
            IsActive = true;
        }
    }
}