using EmployeeImportApp.Models;
using EmployeImportCSVCrud;

namespace EmployeeImportApp.Services
{
    /// <summary>
    /// Interface defining services for managing employees.
    /// Includes CRUD operations, CSV/Excel import/export, and pagination.
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Retrieves a paginated list of employees with optional search and sorting.
        /// </summary>
        /// <param name="search">Search text to filter employees by name or surname.</param>
        /// <param name="sortColumn">Column to sort by (e.g., "Surname").</param>
        /// <param name="sortOrder">Sort direction ("asc" or "desc").</param>
        /// <param name="page">Current page number.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Paginated list of employees.</returns>
        PagedResult<Employee> GetPaged(
           string? search,
           string? sortColumn,
           string? sortOrder,
           int page,
           int pageSize);

        /// <summary>
        /// Retrieves an employee by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the employee.</param>
        /// <returns>The employee if found; otherwise, null.</returns>
        Employee? GetById(int id);

        /// <summary>
        /// Creates a new employee record.
        /// </summary>
        /// <param name="employee">The employee object to create.</param>
        void Create(Employee employee);

        /// <summary>
        /// Updates an existing employee record.
        /// </summary>
        /// <param name="employee">The employee object with updated information.</param>
        void Update(Employee employee);

        /// <summary>
        /// Deletes an employee by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Imports employees from a CSV file.
        /// </summary>
        /// <param name="fileStream">The stream of the CSV file.</param>
        /// <returns>The number of employees imported.</returns>
        int ImportFromCSV(Stream fileStream);

        /// <summary>
        /// Exports all employees to a CSV file.
        /// </summary>
        /// <returns>Byte array representing the CSV file.</returns>
        byte[] ExportToCSV();

        /// <summary>
        /// Exports all employees to an Excel file.
        /// </summary>
        /// <returns>Byte array representing the Excel file.</returns>
        byte[] ExportToExcel();
    }
}
