using EmployeeImportApp.Controllers;
using EmployeeImportApp.Models;
using EmployeeImportApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace EmployeeImportApp.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<IEmployeeService> _serviceMock;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            // Create a mock of the employee service
            _serviceMock = new Mock<IEmployeeService>();

            // Inject the mock service into the controller
            _controller = new HomeController(_serviceMock.Object);

            // Setup TempData for the controller using a mock provider
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _controller.TempData = tempData;
        }

        [Fact]
        public void Import_ShouldReturnRedirect_WhenFileIsNull()
        {
            // Act: call the Import action with null file
            var result = _controller.Import(null) as RedirectToActionResult;

            // Assert: verify that the action redirects to Index
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Create_InvalidModel_ShouldSetTempDataMessage()
        {
            // Arrange: add a model error to simulate invalid model state
            _controller.ModelState.AddModelError("PayrollNumber", "Required");
            var emp = new Employee();

            // Act: call the Create action
            var result = _controller.Create(emp) as RedirectToActionResult;

            // Assert: verify that the action redirects to Index
            Assert.Equal("Index", result.ActionName);

            // Assert: verify that TempData contains a message
            Assert.True(_controller.TempData.ContainsKey("Message"));
        }
    }
}
