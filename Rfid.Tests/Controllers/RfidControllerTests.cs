using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Rfid.Api.Controllers;
using Rfid.Core.Interfaces.Services;
using Rfid.Core.Models;

namespace Rfid.Tests.Controllers;

[TestClass]
public class RfidControllerTests
{
    private IRfidService _service;
    private RfidController _sut;

    [TestInitialize]
    public void Setup()
    {
        _service = Substitute.For<IRfidService>();
        _sut = new RfidController(_service);
    }



    [TestMethod]
    public async Task GetAsync_ShouldReturnOkWhenResultIsFound()
    {
        var id = Guid.NewGuid();
        _service.GetByIdAsync(id).Returns(new RfidTokenDTO { Id = id, ValidFrom = new DateOnly(2024, 01, 01 ), ValidToDate = new DateOnly(2024, 01, 31) });
        var result = await _sut.GetAsync(id);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        Assert.AreEqual(200, (result as OkObjectResult)?.StatusCode);

        var token = (result as OkObjectResult)?.Value as RfidTokenDTO;
        Assert.IsNotNull(token);
        Assert.AreEqual(id, token.Id);
        Assert.AreEqual(new DateOnly(2024, 01, 01), token.ValidFrom);
        Assert.AreEqual(new DateOnly(2024, 01, 31), token.ValidToDate);
    }

    [TestMethod]
    public async Task GetAsync_ShouldReturnInternalServerErrorWhenExceptionIsThrown()
    {
        var id = Guid.NewGuid();
        _service.GetByIdAsync(id).Throws(new Exception("Test exception"));
        var result = await _sut.GetAsync(id);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        Assert.AreEqual(500, (result as ObjectResult)?.StatusCode);
        Assert.AreEqual("Test exception", (result as ObjectResult)?.Value);
    }

    [TestMethod]
    public async Task GetAsync_ShouldReturn204WhenResultNotFound()
    {
        var id = Guid.NewGuid();
        _service.GetByIdAsync(id).Returns((RfidTokenDTO)null);

        var result = await _sut.GetAsync(id);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        Assert.AreEqual(204, (result as NoContentResult)?.StatusCode);
    }
}
