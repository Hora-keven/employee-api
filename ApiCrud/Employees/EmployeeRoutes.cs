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

            route.MapPost("",async (EmployeeRequest req, AppDbContext context, CancellationToken ct)=>{
                var newEmployee = new Employee(req.Name);
                var existEmployee = await context.Employees.AnyAsync(employee=>employee.Name == req.Name,ct);
                if (existEmployee) return Results.Conflict("Employee already exist");
                await context.Employees.AddAsync(newEmployee,ct);
                await context.SaveChangesAsync(ct);
                return Results.Ok(new EmployeeDto(newEmployee.Id, newEmployee.Name));
            });

            route.MapGet("", async(AppDbContext context, CancellationToken ct)=>{
                var employees = await context.Employees.Where(employee=>employee.IsActive)
                .Select(employee=> new EmployeeDto(employee.Id, employee.Name))
                .ToListAsync(ct);
                return Results.Ok(employees);
            });

            route.MapPut("{id:guid}", async(Guid id, AppDbContext context, UpdateEmployeeRequest req, CancellationToken ct)=>{
                var employee = await context.Employees.SingleOrDefaultAsync(employee=>employee.Id == id,ct);
                if(employee == null) return Results.NotFound();
                employee.UpdateName(req.Name);

                await context.SaveChangesAsync(ct);
                return Results.Ok(new EmployeeDto(employee.Id, employee.Name));

            });

            route.MapDelete("{id:guid}", async(Guid id, AppDbContext context, CancellationToken ct)=>{
                var employee = await context.Employees.SingleOrDefaultAsync(employee=>employee.Id == id,ct);
                if(employee == null) return Results.NotFound();
                employee.Desactive();
                await context.SaveChangesAsync(ct);
                return Results.NoContent();
            });
        }
    }
}