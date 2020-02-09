using AutoFixture;
using FluentAssertions;
using Xunit;

namespace BattleshipStateTracker.Domain.UnitTests
{
    public class GameStateServiceTests
    {
        private readonly Fixture _fixture;

        public GameStateServiceTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void GetGameState_ShouldReturnNoGame_WhenNoGameIsInitialized()
        {
            // Arrange
            var target = new GameStateService();

            // Act
            var actual = target.GetGameStatus();

            // Assert
            actual.Should().Be("NoGame");
        }

        [Fact]
        public void GetGameState_ShouldReturnInitiated_WhenCalledAfterNewGameInitialization()
        {
            // Arrange
            var target = new GameStateService();
            var playerName = _fixture.Create<string>();
             var nbOfShips = _fixture.Create<int>();

            // Act
            target.InitializeNewGame(playerName, nbOfShips);
            var actual = target.GetGameStatus();

            // Assert
            actual.Should().Be("Initiated");
        }
    }
}