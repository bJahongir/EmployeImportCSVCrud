using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EmployeeImportApp.Models;
using EmployeeImportApp.Repositories;
using EmployeImportCSVCrud;
using EmployeImportCSVCrud.Mapper;
using OfficeOpenXml;
using System.Globalization;
using System.Linq.Expressions;

namespace EmployeeImportApp.Services
{
    /// <summary>
    /// Provides implementation of <see cref="IEmployeeService"/> for managing employees.
    /// Includes CRUD operations, pagination, CSV/Excel import/export.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;

        /// <summary>
        /// Constructor to inject employee repository dependency.
        /// </summary>
        /// <param name="repo">Employee repository instance.</param>
        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Retrieves paginated employees with optional search and sorting.
        /// </summary>
        /// <param name="search">Text to filter employees by multiple fields.</param>
        /// <param name="sortColumn">Column name to sort by.</param>
        /// <param name="sortOrder">Sort direction ("asc" or "desc").</param>
        /// <param name="page">Current page number.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Paginated result of employees.</returns>
        public PagedResult<Employee> GetPaged(
           string? search,
           string? sortColumn,
           string? sortOrder,
           int page,
           int pageSize)
        {
            var query = _repo.Query();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(e =>
                    (e.PayrollNumber ?? "").ToLower().Contains(search) ||
                    (e.Forenames ?? "").ToLower().Contains(search) ||
                    (e.Surname ?? "").ToLower().Contains(search) ||
                    (e.Telephone ?? "").ToLower().Contains(search) ||
                    (e.Mobile ?? "").ToLower().Contains(search) ||
                    (e.Address ?? "").ToLower().Contains(search) ||
                    (e.Address2 ?? "").ToLower().Contains(search) ||
                    (e.Postcode ?? "").ToLower().Contains(search) ||
                    (e.EmailHome ?? "").ToLower().Contains(search) ||
                    e.DateOfBirth.ToString().Contains(search) ||
                    e.StartDate.ToString().Contains(search)
                );
            }

            // Apply sorting if provided, otherwise default sort by Surname
            if (!string.IsNullOrEmpty(sortColumn))
            {
                bool descending = sortOrder?.ToLower() == "desc";
                query = ApplySorting(query, sortColumn, descending);
            }
            else
            {
                query = query.OrderBy(e => e.Surname);
            }

            int totalCount = query.Count();

            // Apply paging
            var employees = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Employee>
            {
                Items = employees,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Dynamically applies sorting to a query based on a column name and direction.
        /// </summary>
        /// <param name="query">Queryable of employees.</param>
        /// <param name="sortColumn">Column to sort by.</param>
        /// <param name="descending">True for descending, false for ascending.</param>
        /// <returns>Sorted queryable.</returns>
        private IQueryable<Employee> ApplySorting(IQueryable<Employee> query, string sortColumn, bool descending)
        {
            var parameter = Expression.Parameter(typeof(Employee), "e");
            var property = typeof(Employee).GetProperty(sortColumn);
            if (property == null)
                return query.OrderBy(e => e.Surname); // fallback

            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            string method = descending ? "OrderByDescending" : "OrderBy";
            var resultExp = Expression.Call(typeof(Queryable), method,
                new Type[] { typeof(Employee), property.PropertyType },
                query.Expression, Expression.Quote(orderByExp));

            return query.Provider.CreateQuery<Employee>(resultExp);
        }

        /// <summary>
        /// Gets an employee by ID.
        /// </summary>
        /// <param name="id">Employee ID.</param>
        /// <returns>Employee or null if not found.</returns>
        public Employee? GetById(int id) => _repo.GetById(id);

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">Employee object to create.</param>
        public void Create(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.PayrollNumber))
                throw new ArgumentException("PayrollNumber is required.");
            _repo.Add(employee);
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="employee">Employee object with updated data.</param>
        public void Update(Employee employee)
        {
            _repo.Update(employee);
        }

        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        /// <param name="id">ID of the employee to delete.</param>
        public void Delete(int id)
        {
            var dbEmployee = _repo.GetById(id);
            if (dbEmployee == null)
                throw new KeyNotFoundException("Employee not found.");
            _repo.Delete(id);
        }

        /// <summary>
        /// Imports employees from a CSV stream.
        /// </summary>
        /// <param name="fileStream">CSV file stream.</param>
        /// <returns>Number of imported employees.</returns>
        public int ImportFromCSV(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                BadDataFound = null
            };

            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<EmployeeMap>();

            var employees = csv.GetRecords<Employee>().ToList();

            if (!employees.Any())
                throw new InvalidOperationException("CSV file is empty or invalid.");

            _repo.BulkInsert(employees);
            return employees.Count;
        }



        /// <summary>
        /// Exports all employees to a CSV file.
        /// </summary>
        /// <returns>Byte array of CSV data.</returns>
        public byte[] ExportToCSV()
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(_repo.GetAll());
            writer.Flush();
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Exports all employees to an Excel file.
        /// </summary>
        /// <returns>Byte array of Excel file data.</returns>
        public byte[] ExportToExcel()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employees");

            var employees = _repo.GetAll().ToList();

            // Header row
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "PayrollNumber";
            worksheet.Cell(1, 3).Value = "Forenames";
            worksheet.Cell(1, 4).Value = "Surname";
            worksheet.Cell(1, 5).Value = "DateOfBirth";
            worksheet.Cell(1, 6).Value = "Telephone";
            worksheet.Cell(1, 7).Value = "Mobile";
            worksheet.Cell(1, 8).Value = "Address";
            worksheet.Cell(1, 9).Value = "Address2";
            worksheet.Cell(1, 10).Value = "Postcode";
            worksheet.Cell(1, 11).Value = "EmailHome";
            worksheet.Cell(1, 12).Value = "StartDate";

            // Fill data rows
            int row = 2;
            foreach (var emp in employees)
            {
                worksheet.Cell(row, 1).Value = emp.Id;
                worksheet.Cell(row, 2).Value = emp.PayrollNumber;
                worksheet.Cell(row, 3).Value = emp.Forenames;
                worksheet.Cell(row, 4).Value = emp.Surname;
                worksheet.Cell(row, 5).Value = emp.DateOfBirth;
                worksheet.Cell(row, 6).Value = emp.Telephone;
                worksheet.Cell(row, 7).Value = emp.Mobile;
                worksheet.Cell(row, 8).Value = emp.Address;
                worksheet.Cell(row, 9).Value = emp.Address2;
                worksheet.Cell(row, 10).Value = emp.Postcode;
                worksheet.Cell(row, 11).Value = emp.EmailHome;
                worksheet.Cell(row, 12).Value = emp.StartDate;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
