using CsvHelper.Configuration;
using EmployeeImportApp.Models;

namespace EmployeImportCSVCrud.Mapper
{
    /// <summary>
    /// Maps CSV columns to the <see cref="Employee"/> properties.
    /// Handles multi-format date conversion for DateOfBirth and StartDate.
    /// </summary>
    public sealed class EmployeeMap : ClassMap<Employee>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EmployeeMap"/> and defines CSV column mappings.
        /// </summary>
        public EmployeeMap()
        {
            // Map PayrollNumber column
            Map(m => m.PayrollNumber).Name("Personnel_Records.Payroll_Number");

            // Map employee names
            Map(m => m.Forenames).Name("Personnel_Records.Forenames");
            Map(m => m.Surname).Name("Personnel_Records.Surname");

            // Map DateOfBirth with custom MultiFormatDateTimeConverter
            Map(m => m.DateOfBirth)
                .Name("Personnel_Records.Date_of_Birth")
                .TypeConverter<MultiFormatDateTimeConverter>();

            // Map contact information
            Map(m => m.Telephone).Name("Personnel_Records.Telephone");
            Map(m => m.Mobile).Name("Personnel_Records.Mobile");

            // Map addresses
            Map(m => m.Address).Name("Personnel_Records.Address");
            Map(m => m.Address2).Name("Personnel_Records.Address_2");
            Map(m => m.Postcode).Name("Personnel_Records.Postcode");

            // Map email
            Map(m => m.EmailHome).Name("Personnel_Records.EMail_Home");

            // Map StartDate with custom MultiFormatDateTimeConverter
            Map(m => m.StartDate)
                .Name("Personnel_Records.Start_Date")
                .TypeConverter<MultiFormatDateTimeConverter>();
        }
    }
}
