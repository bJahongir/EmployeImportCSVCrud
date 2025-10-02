using EmployeeImportApp.Models;
using EmployeeImportApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeImportApp.Controllers
{
    /// <summary>
    /// Controller for managing Employee operations such as CRUD, CSV/Excel import/export, and paging.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Initializes a new instance of <see cref="HomeController"/>.
        /// </summary>
        /// <param name="employeeService">Injected employee service for data operations.</param>
        public HomeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Displays the employee list with optional search, sorting, and pagination.
        /// </summary>
        /// <param name="search">Search term for filtering employees.</param>
        /// <param name="sortColumn">Column to sort by.</param>
        /// <param name="sortDirection">Sort direction: "asc" or "desc".</param>
        /// <param name="page">Current page number.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>A view or partial view with paged employee data.</returns>
        public IActionResult Index(string search = "", string sortColumn = "Surname", string sortDirection = "asc", int page = 1, int pageSize = 5)
        {
            var result = _employeeService.GetPaged(search, sortColumn, sortDirection, page, pageSize);

            ViewBag.Search = search;
            ViewBag.SortColumn = sortColumn;
            ViewBag.SortDirection = sortDirection;

            // Return partial view if AJAX request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EmployeeTablePartial", result);
            }

            return View(result);
        }

        /// <summary>
        /// Imports employees from a CSV file.
        /// </summary>
        /// <param name="file">The uploaded CSV file.</param>
        /// <returns>Redirects to Index with a success or error message.</returns>
        [HttpPost]
        public IActionResult Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Message"] = "Please select a file.";
                TempData["MessageType"] = "warning";
                return RedirectToAction("Index");
            }

            using var stream = file.OpenReadStream();
            try
            {
                var count = _employeeService.ImportFromCSV(stream);
                TempData["Message"] = $"{count} records imported successfully!";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">Employee object from form submission.</param>
        /// <returns>Redirects to Index with a success or error message.</returns>
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                // Collect validation errors
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["Message"] = "Validation failed: " + string.Join("; ", errors);
                TempData["MessageType"] = "warning";
                return RedirectToAction("Index");
            }

            try
            {
                _employeeService.Create(employee);
                TempData["Message"] = "Employee added successfully!";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="employee">Employee object from form submission.</param>
        /// <returns>Redirects to Index with a success or error message.</returns>
        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["Message"] = "Validation failed: " + string.Join("; ", errors);
                TempData["MessageType"] = "warning";
                return RedirectToAction("Index");
            }

            try
            {
                _employeeService.Update(employee);
                TempData["Message"] = "Employee updated successfully!";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Deletes an employee by Id.
        /// </summary>
        /// <param name="id">Employee Id to delete.</param>
        /// <returns>Redirects to Index with a success or error message.</returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _employeeService.Delete(id);
                TempData["Message"] = "Employee deleted successfully!";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Exports all employees to a CSV file.
        /// </summary>
        /// <returns>CSV file download or redirects to Index on error.</returns>
        public IActionResult ExportCsv()
        {
            try
            {
                var fileBytes = _employeeService.ExportToCSV();
                var fileName = $"employees_{DateTime.Now:yyyyMMddHHmmss}.csv";
                return File(fileBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Exports all employees to an Excel file.
        /// </summary>
        /// <returns>Excel file download or redirects to Index on error.</returns>
        public IActionResult ExportExcel()
        {
            try
            {
                var fileBytes = _employeeService.ExportToExcel();
                var fileName = $"employees_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
                TempData["MessageType"] = "error";
                return RedirectToAction("Index");
            }
        }
    }
}
