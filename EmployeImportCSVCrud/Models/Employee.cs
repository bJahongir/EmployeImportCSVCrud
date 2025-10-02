using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeImportApp.Models
{
    /// <summary>
    /// Represents an employee entity with personal and work-related information.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sets the unique identifier for the employee.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the payroll number.
        /// Required field.
        /// </summary>
        [Required]
        public string PayrollNumber { get; set; }

        /// <summary>
        /// Gets or sets the employee's first names.
        /// </summary>
        public string Forenames { get; set; }

        /// <summary>
        /// Gets or sets the employee's surname/last name.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the employee's date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the employee's telephone number.
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Gets or sets the employee's mobile phone number.
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets the primary address of the employee.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the secondary address of the employee (optional).
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the postal code of the employee's address.
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the employee's personal email address.
        /// </summary>
        public string EmailHome { get; set; }

        /// <summary>
        /// Gets or sets the date when the employee started working.
        /// </summary>
        public DateTime StartDate { get; set; }
    }
}
