using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Employees
{
    public static class EmployeeRoutes
    {
        public static void AddEmployeeRoutes(this WebApplication app){
            var route = app.MapGroup("employee");
            
            route.MapPost("",async (EmployeeRequest req, AppDbContext context)=>{
                var newEmployee = new Employee(req.Name);
                await context.Employees.AddAsync(newEmployee);
                await context.SaveChangesAsync();
                return Results.Created();
            });

            route.MapGet("", async(AppDbContext context)=>{
                var employees = await context.Employees.ToListAsync();
                return Results.Ok(employees);
            });
        }
    }
}