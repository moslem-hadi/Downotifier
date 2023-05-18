using Application.ApiCallJobCommandQuery.Commands.Create;
using Application.ApiCallJobCommandQuery.Commands.Delete;
using Application.Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace IntegrationTests;

using static Testing;

public class ApiCallJobTests : BaseTestFixture
{
    private CreateApiCallJobCommand createCommand;
    public ApiCallJobTests()
    {
        createCommand = new CreateApiCallJobCommand
        {
            Notifications = new List<Notification>(),
            Title = "Title",
            Url = "URL",
            MonitoringInterval = 1,
            Method = Shared.Enums.ApiMethod.Get,
        };
    }
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateApiCallJobCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateCorrectEntity()
    {
        var id = await SendAsync(createCommand);

        var list = await FindAsync<ApiCallJob>(id);

        list.Should().NotBeNull();
        list!.Title.Should().Be(createCommand.Title);
        list.CreatedBy.Should().Be(Guid.Empty.ToString());
        list.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }



    [Test]
    public async Task ShouldRequireValidId()
    {
        var invalidId = -1;
        var invalidCommand = new DeleteApiCallJobCommand(invalidId);
        await FluentActions.Invoking(() => SendAsync(invalidCommand)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteEntity()
    {
        var id = await SendAsync(createCommand);

        await SendAsync(new DeleteApiCallJobCommand(id));

        var list = await FindAsync<ApiCallJob>(id);

        list.Should().BeNull();
    }
}
