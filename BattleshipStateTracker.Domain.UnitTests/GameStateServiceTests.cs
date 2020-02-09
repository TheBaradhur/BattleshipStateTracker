using FluentAssertions;
using Xunit;

namespace BattleshipStateTracker.Domain.UnitTests
{
    public class GameStateServiceTests
    {
        [Fact]
        public void GetGameState_ReturnsNoGame_WhenNoGameIsInitialized()
        {
            // Arrange
            var target = new GameStateService();

            // Act
            var actual = target.GetGameState();

            // Assert
            actual.Should().Be("NoGame");
        }
    }
}