using NSubstitute;
using Rfid.Core.Interfaces.Repositories;
using Rfid.Core.Interfaces.Services;
using Rfid.Core.Models;
using Rfid.Data.Services;

namespace Rfid.Tests.ServiceTests
{
    [TestClass]
    public class RfidServiceTests
    {
        private IRfidRepository _repo;
        private ILogService _logger;
        private IRfidService _sut;

        [TestInitialize]
        public void Setup()
        {
            _repo = Substitute.For<IRfidRepository>();
            _logger = Substitute.For<ILogService>();
            _sut = new RfidService(_repo, _logger);
        }

        [TestMethod]
        public async Task AddAsync_WhenCalled_LogsInfo()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _sut.AddAsync(null));
            await _logger.Received(1).LogInfoAsync(Arg.Any<Type>(), Arg.Any<string>());
            await _logger.Received(1).LogErrorAsync(Arg.Any<Type>(), Arg.Any<string>(), Arg.Any<ArgumentNullException>());
        }

        [TestMethod]
        public async Task AddAsync_WhenCalledWithEmptyId_GeneratesNewId()
        {
            var token = new RfidTokenDTO { Id = Guid.Empty };
            await _sut.AddAsync(token);
            Assert.AreNotEqual(Guid.Empty, token.Id);
        }

        [TestMethod]
        public async Task AddAsync_WhenCalledWithNullValidFrom_SetsValidFromToToday()
        {
            var token = new RfidTokenDTO { ValidFrom = null };
            await _sut.AddAsync(token);
            Assert.IsNotNull(token.ValidFrom);
            Assert.AreEqual(DateTime.Today.ToString("yyyy-MM-dd"), token.ValidFrom.Value.ToString("yyyy-MM-dd"));
        }

        [TestMethod]
        public async Task AddAsync_WhenCalled_CallsRepoAddAsync()
        {
            var token = new RfidTokenDTO();
            await _sut.AddAsync(token);
            await _repo.Received(1).AddAsync(token);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsData()
        {
            var id = Guid.NewGuid();
            var token = new RfidTokenDTO { Id = id, ValidFrom = new DateOnly(2025, 1, 1), ValidToDate = new DateOnly(2025, 1, 31) };
            _repo.GetByIdAsync(id).Returns(token);

            var result = await _sut.GetByIdAsync(id);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(token.ValidFrom, result.ValidFrom);
            Assert.AreEqual(token.ValidToDate, result.ValidToDate);

            await _repo.Received(1).GetByIdAsync(id);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsNullWhenTokenNotFound()
        {
            var id = Guid.NewGuid();
            _repo.GetByIdAsync(id).Returns((RfidTokenDTO)null);

            var result = await _sut.GetByIdAsync(id);
            Assert.IsNull(result);
            await _repo.Received(1).GetByIdAsync(id);
        }
    }
}
