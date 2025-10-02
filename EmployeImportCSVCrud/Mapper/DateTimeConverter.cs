using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;

namespace EmployeImportCSVCrud
{
    /// <summary>
    /// A custom CSV type converter to handle multiple date formats when reading CSV files.
    /// </summary>
    public class MultiFormatDateTimeConverter : DefaultTypeConverter
    {
        /// <summary>
        /// Supported date formats for parsing.
        /// </summary>
        private readonly string[] _formats = new[]
        {
            "dd/MM/yyyy",  // e.g., 31/12/2025
            "d/M/yyyy",    // e.g., 1/1/2025
            "yyyy-MM-dd",  // e.g., 2025-12-31
            "MM/dd/yyyy"   // e.g., 12/31/2025
        };

        /// <summary>
        /// Converts a CSV string value to a DateTime using multiple supported formats.
        /// </summary>
        /// <param name="text">The CSV string value.</param>
        /// <param name="row">The CSV reader row.</param>
        /// <param name="memberMapData">Member mapping data.</param>
        /// <returns>Parsed <see cref="DateTime"/> value.</returns>
        /// <exception cref="TypeConverterException">Thrown if the date format is invalid.</exception>
        public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            // Return default DateTime if input is null or empty
            if (string.IsNullOrWhiteSpace(text))
                return default(DateTime);

            // Try each format until one succeeds
            foreach (var format in _formats)
            {
                if (DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out var date))
                {
                    return date;
                }
            }

            // Throw exception if no format matched
            throw new TypeConverterException(this, memberMapData, text, row.Context,
                $"Invalid date format: '{text}'. Supported formats: {string.Join(", ", _formats)}");
        }
    }
}
