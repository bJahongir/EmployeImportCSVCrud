using EmployeeImportApp.Data;
using EmployeeImportApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeImportApp.Repositories
{
    /// <summary>
    /// Repository implementation for Employee entity.
    /// Provides CRUD operations and bulk insert functionality.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor to inject database context.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns an <see cref="IQueryable{Employee}"/> for flexible querying.
        /// </summary>
        /// <returns>IQueryable of Employee entities.</returns>
        public IQueryable<Employee> Query()
        {
            // Return queryable to allow filtering, sorting, paging outside repository
            return _context.Employees.AsQueryable();
        }

        /// <summary>
        /// Retrieves all employees ordered by surname.
        /// </summary>
        /// <returns>List of all employees.</returns>
        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.OrderBy(e => e.Surname).ToList();
        }

        /// <summary>
        /// Retrieves an employee by its ID.
        /// </summary>
        /// <param name="id">Employee unique identifier.</param>
        /// <returns>Employee if found; otherwise null.</returns>
        public Employee? GetById(int id)
        {
            return _context.Employees.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Adds a new employee to the database.
        /// </summary>
        /// <param name="employee">Employee to add.</param>
        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges(); // Save immediately
        }

        /// <summary>
        /// Updates an existing employee in the database.
        /// </summary>
        /// <param name="employee">Employee with updated data.</param>
        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges(); // Save changes immediately
        }

        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        /// <param name="id">ID of the employee to delete.</param>
        public void Delete(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Inserts multiple employees in bulk.
        /// </summary>
        /// <param name="employees">Collection of employees to insert.</param>
        public void BulkInsert(IEnumerable<Employee> employees)
        {
            // AddRange is more efficient than multiple Add calls
            _context.Employees.AddRange(employees);
            _context.SaveChanges(); // Commit all changes at once
        }
    }
}
