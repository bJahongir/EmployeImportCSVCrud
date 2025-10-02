using System;
using System.Collections.Generic;

namespace EmployeImportCSVCrud
{
    /// <summary>
    /// Represents a paged result for a collection of items of type <typeparamref name="T"/>.
    /// Used for pagination, sorting, and search functionality.
    /// </summary>
    /// <typeparam name="T">The type of items in the paged result.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Gets or sets the collection of items on the current page.
        /// Initialized to an empty list by default.
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the current page number (1-based index).
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the total number of pages based on <see cref="TotalCount"/> and <see cref="PageSize"/>.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);

        /// <summary>
        /// Gets or sets the search term used for filtering the results (optional).
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Gets or sets the column name used for sorting (optional).
        /// </summary>
        public string? SortColumn { get; set; }

        /// <summary>
        /// Gets or sets the sort order, e.g., "asc" or "desc" (optional).
        /// </summary>
        public string? SortOrder { get; set; }
    }
}
