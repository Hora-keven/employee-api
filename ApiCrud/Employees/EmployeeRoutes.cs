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
                var existEmployee = await context.Employees.AnyAsync(employee=>employee.Name == req.Name);
                if (existEmployee) return Results.Conflict("Employee already exist");
                await context.Employees.AddAsync(newEmployee);
                await context.SaveChangesAsync();
                return Results.Created();
            });

            route.MapGet("", async(AppDbContext context)=>{
                var employees = await context.Employees.Where(employee=>employee.IsActive).ToListAsync();
                return Results.Ok(employees);
            });

            route.MapPut("{id:guid}", async(Guid id, AppDbContext context, UpdateEmployeeRequest req)=>{
                var employee = await context.Employees.SingleOrDefaultAsync(employee=>employee.Id == id);
                if(employee == null) return Results.NotFound();
                employee.updateName(req.Name);

                await context.SaveChangesAsync();
                return Results.Ok(employee);

            });
        }
    }
}