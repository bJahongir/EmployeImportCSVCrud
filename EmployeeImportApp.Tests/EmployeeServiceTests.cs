using CsvHelper.Configuration;
using EmployeeImportApp.Models;
using EmployeeImportApp.Repositories;
using EmployeeImportApp.Services;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace EmployeeImportApp.Tests
{
    public class EmployeeServiceTests
    {
        private readonly EmployeeService _service;
        private readonly Mock<IEmployeeRepository> _repoMock;

        public EmployeeServiceTests()
        {
            // Initialize the repository mock
            _repoMock = new Mock<IEmployeeRepository>();

            // Inject the mock repository into the service
            _service = new EmployeeService(_repoMock.Object);
        }

        [Fact]
        public void Create_ShouldCallRepoAdd()
        {
            // Arrange: create a new employee instance
            var emp = new Employee { PayrollNumber = "123" };

            // Act: call the service's Create method
            _service.Create(emp);

            // Assert: verify that Add was called exactly once on the repository
            _repoMock.Verify(r => r.Add(emp), Times.Once);
        }

        [Fact]
        public void GetPaged_ShouldReturnPagedResult()
        {
            // Arrange: prepare a sample dataset
            var data = new List<Employee>
            {
                new Employee { Id=1, Surname="A" },
                new Employee { Id=2, Surname="B" },
            }.AsQueryable();

            // Setup the repository Query method to return the sample dataset
            _repoMock.Setup(r => r.Query()).Returns(data);

            // Act: call GetPaged with paging parameters
            var result = _service.GetPaged("", "Surname", "asc", 1, 1);

            // Assert: check that only one item is returned in the current page
            Assert.Single(result.Items);

            // Assert: check the total count of items
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public void ImportFromCSV_ShouldReturnCorrectCount()
        {
            // Arrange: create a repository mock and setup BulkInsert method
            var repoMock = new Mock<IEmployeeRepository>();
            repoMock.Setup(r => r.BulkInsert(It.IsAny<List<Employee>>()));

            // Prepare a CSV string with full employee data
            string csv = @"Personnel_Records.Payroll_Number,Personnel_Records.Forenames,Personnel_Records.Surname,Personnel_Records.Date_of_Birth,Personnel_Records.Telephone,Personnel_Records.Mobile,Personnel_Records.Address,Personnel_Records.Address_2,Personnel_Records.Postcode,Personnel_Records.EMail_Home,Personnel_Records.Start_Date
123,John,Doe,1990-01-01,123456789,987654321,Addr1,Addr2,1000,john@example.com,2020-01-01";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

            // Act: create the service with mock repository and import CSV
            var service = new EmployeeService(repoMock.Object);
            int count = service.ImportFromCSV(stream);

            // Assert: verify that only one record was imported
            Assert.Equal(1, count);

            // Optionally, you could verify individual fields by modifying ImportFromCSV to return the list
            // var employees = service.ImportFromCSV(stream);
            // Assert.Equal("John", employees[0].Forenames);
            // Assert.Equal("Doe", employees[0].Surname);

            // Assert: verify that BulkInsert was called once
            repoMock.Verify(r => r.BulkInsert(It.IsAny<List<Employee>>()), Times.Once);
        }
    }
}
