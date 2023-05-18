using Application.ApiCallJobCommandQuery.Commands.Create;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateApiCallJobCommand>> _logger = null!;
    private Mock<ICurrentUserService> _currentUserService = null!;
    private CreateApiCallJobCommand justAnEntity;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateApiCallJobCommand>>();
        _currentUserService = new Mock<ICurrentUserService>();
        justAnEntity = new CreateApiCallJobCommand 
        {
            Id = 1,
            Notifications = new List<Notification>(),
            Title = "Title",
            Url = "URL",
            MonitoringInterval = 1,
            Method = Shared.Enums.ApiMethod.Get,
        } ;
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _currentUserService.Setup(x => x.UserId).Returns(Guid.NewGuid().ToString());

        var requestLogger = new LoggingBehaviour<CreateApiCallJobCommand>(_logger.Object, _currentUserService.Object);

        await requestLogger.Process(justAnEntity, new CancellationToken());

        _currentUserService.Verify(i => i.UserId, Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger = new LoggingBehaviour<CreateApiCallJobCommand>(_logger.Object, _currentUserService.Object);

        await requestLogger.Process(justAnEntity, new CancellationToken());

        _currentUserService.Verify(i => i.UserId, Times.Never);
    }
}
