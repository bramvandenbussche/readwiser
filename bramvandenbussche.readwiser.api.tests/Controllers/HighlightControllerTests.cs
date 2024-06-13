using AutoFixture.Xunit2;
using bramvandenbussche.readwiser.api.Controllers;
using bramvandenbussche.readwiser.domain.Interface.Business;
using FakeItEasy;
using FluentAssertions;

namespace bramvandenbussche.readwiser.api.tests.Controllers;

public class HighlightControllerTests
{

    [UnitTest, Theory]
    public async Task Calling_GetAll_WithNoTimestamp_Should_NotReturnNull([Greedy] HighlightController sut)
    {
        // Act
        var result = await sut.GetAll();

        // Assert
        result.Should().NotBeNull();
    }

    [UnitTest, Theory]
    public async Task Calling_GetAll_WithServiceThrowing_Should_NotThrow(
        [Frozen] IHighlightService service, 
        [Greedy] HighlightController sut)
    {
        // Arrange
        var serviceCall = A.CallTo(() => service.GetAll(0));
        serviceCall.Throws<Exception>();

        // Act
        var act = () => sut.GetAll();

        // Assert
        await act.Should().NotThrowAsync();
        serviceCall.MustHaveHappenedOnceExactly();
    }
}