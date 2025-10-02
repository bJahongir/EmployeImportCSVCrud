using EmployeeImportApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeImportApp.Repositories
{
    /// <summary>
    /// Defines repository operations for the <see cref="Employee"/> entity.
    /// Provides methods for querying, CRUD operations, and bulk insert.
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Returns an <see cref="IQueryable{Employee}"/> for flexible queries.
        /// </summary>
        /// <returns>IQueryable of Employee entities.</returns>
        IQueryable<Employee> Query();

        /// <summary>
        /// Retrieves all employees, ordered by surname.
        /// </summary>
        /// <returns>Collection of all employees.</returns>
        IEnumerable<Employee> GetAll();

        /// <summary>
        /// Retrieves an employee by its unique identifier.
        /// </summary>
        /// <param name="id">Employee ID.</param>
        /// <returns>Employee if found; otherwise null.</returns>
        Employee? GetById(int id);

        /// <summary>
        /// Adds a new employee to the repository.
        /// </summary>
        /// <param name="employee">Employee to add.</param>
        void Add(Employee employee);

        /// <summary>
        /// Updates an existing employee in the repository.
        /// </summary>
        /// <param name="employee">Employee with updated data.</param>
        void Update(Employee employee);

        /// <summary>
        /// Deletes an employee by its ID.
        /// </summary>
        /// <param name="id">ID of the employee to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Inserts multiple employees in bulk for efficiency.
        /// </summary>
        /// <param name="employees">Collection of employees to insert.</param>
        void BulkInsert(IEnumerable<Employee> employees);
    }
}
