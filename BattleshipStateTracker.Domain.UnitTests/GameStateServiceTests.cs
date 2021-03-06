using AutoFixture;
using FluentAssertions;
using Xunit;

namespace BattleshipStateTracker.Domain.UnitTests
{
    public class GameStateServiceTests
    {
        private readonly Fixture _fixture;

        private const string MissText = "Attack missed...";

        private const string HitText = "Attack hit!!!";

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

        [Fact]
        public void ValidateAddShipRequest_ShouldReturnInitError_WhenGameIsNotInitialized()
        {
            // Arrange
            var target = new GameStateService();
            var x = _fixture.Create<int>();
            var y = _fixture.Create<int>();
            var orientation = _fixture.Create<string>();
            var size = _fixture.Create<int>();

            // Act
            var actual = target.ValidateAddShipRequest(x, y, orientation, size, true);

            // Assert
            actual.ErrorCode.Should().Be("GameNotInitialized");
        }

        [Theory]
        [InlineData(1, 1, "up", 11, "InvalidShipSize")]
        [InlineData(1, 1, "up", 1, "InvalidShipSize")]
        [InlineData(0, 1, "up", 3, "InvalidShipPosition")]
        [InlineData(1, 0, "up", 3, "InvalidShipPosition")]
        [InlineData(-1, 1, "up", 3, "InvalidShipPosition")]
        [InlineData(1, 1, "up", 3, "InvalidShipPosition")]
        [InlineData(1, 1, "left", 3, "InvalidShipPosition")]
        [InlineData(10, 10, "down", 3, "InvalidShipPosition")]
        [InlineData(10, 10, "right", 3, "InvalidShipPosition")]
        public void ValidateAddShipRequest_ShouldReturnError_WhenInputsAreNotValid(int x, int y, string orientation, int size, string expectedErrorCode)
        {
            // Arrange
            var target = new GameStateService();
            var playerName = _fixture.Create<string>();
            var nbOfShips = _fixture.Create<int>();
            target.InitializeNewGame(playerName, nbOfShips);

            // Act
            var actual = target.ValidateAddShipRequest(x, y, orientation, size, true);

            // Assert
            actual.ErrorCode.Should().Be(expectedErrorCode);
        }

        [Fact]
        public void ValidateAddShipRequest_ShouldReturnError_WhenAddingShipAfterAllShipsArePlaced()
        {
            // Arrange
            var target = new GameStateService();
            var playerName = _fixture.Create<string>();
            var nbOfShips = 1;
            target.InitializeNewGame(playerName, nbOfShips);

            var x = 1;
            var y = 1;
            var orientation = "right";
            var size = 2;

            // Act
            target.AddShipOnPlayersBoard(x, y, orientation, size, true);
            var actual = target.ValidateAddShipRequest(x, y, orientation, size, true);

            // Assert
            actual.ErrorCode.Should().Be("SetupIsFinished");
            actual.Error.Should().Be("You cannot add another ship once the game has started or all ship are placed.");
        }

        [Fact]
        public void ValidateAddShipRequest_ShouldReturnError_WhenAddingShipOnAnotherOne()
        {
            // Arrange
            var target = new GameStateService();
            var playerName = _fixture.Create<string>();
            var nbOfShips = 2;
            target.InitializeNewGame(playerName, nbOfShips);

            var x = 1;
            var y = 1;
            var orientation = "right";
            var size = 2;

            // Act
            target.AddShipOnPlayersBoard(x, y, orientation, size, true);
            var actual = target.ValidateAddShipRequest(x, y, orientation, size, true);

            // Assert
            actual.ErrorCode.Should().Be("InvalidShipPosition");
            actual.Error.Should().Be("The ship is overlapping with another ship.");
        }

        [Fact]
        public void GetGameStatus_ShouldReturnShipSetup_WhenShipsRemainToBePlaced()
        {
            // Arrange
            var target = new GameStateService();
            var playerName = _fixture.Create<string>();
            var nbOfShips = 2;
            target.InitializeNewGame(playerName, nbOfShips);

            var x = 1;
            var y = 1;
            var orientation = "right";
            var size = 2;

            // Act
            target.AddShipOnPlayersBoard(x, y, orientation, size, true);
            var actual = target.GetGameStatus();

            // Assert
            actual.Should().Be("ShipSetup");
        }

        [Fact]
        public void GetGameStatus_ShouldReturnShipSetupComplete_WhenAllShipsArePlaced()
        {
            // Arrange
            var target = new GameStateService();
            var playerName = _fixture.Create<string>();
            var nbOfShips = 1;
            target.InitializeNewGame(playerName, nbOfShips);

            var x = 1;
            var y = 1;
            var orientation = "right";
            var size = 2;

            // Act
            target.AddShipOnPlayersBoard(x, y, orientation, size, true);
            var actual = target.GetGameStatus();

            // Assert
            actual.Should().Be("ShipSetupCompleted");
        }

        [Fact]
        public void ValidateAttackPosition_ShouldReturnInitError_WhenGameIsNotInitialized()
        {
            // Arrange
            var target = new GameStateService();
            var x = _fixture.Create<int>();
            var y = _fixture.Create<int>();

            // Act
            var actual = target.ValidateAttackPosition(x, y, 1);

            // Assert
            actual.ErrorCode.Should().Be("GameNotInitialized");
        }

        [Fact]
        public void ValidateAttackPosition_ShouldReturnShipsSetupIncomplete_WhenShipsTotalCountIsNotReached()
        {
            // Arrange
            var target = new GameStateService();
            var playerName = _fixture.Create<string>();
            var nbOfShips = 2;
            target.InitializeNewGame(playerName, nbOfShips);

            var x = 1;
            var y = 1;
            var orientation = "right";
            var size = 2;

            target.AddShipOnPlayersBoard(x, y, orientation, size, true);

            // Act
            var actual = target.ValidateAttackPosition(x, y, 1);

            // Assert
            actual.ErrorCode.Should().Be("ShipsSetupIncomplete");
        }

        [Fact]
        public void ValidateAttackPosition_ShouldReturnGameFinished_WhenAttackingAfterGameEnded()
        {
            // Arrange
            var target = new GameStateService();
            var playerName = _fixture.Create<string>();
            var nbOfShips = 1;
            target.InitializeNewGame(playerName, nbOfShips);

            var x = 1;
            var y = 1;
            var orientation = "right";
            var size = 2;

            target.AddShipOnPlayersBoard(x, y, orientation, size, true);

            target.AttackPlayersPosition(x, y, 1);
            target.AttackPlayersPosition(x, y + 1, 1);

            // Act
            var actual = target.ValidateAttackPosition(x, y, 1);

            // Assert
            actual.ErrorCode.Should().Be("GameFinished");
        }

        [Fact]
        public void AttackPlayersPosition_ShouldReturnHitText_WhenAShipIsHit()
        {
            // Arrange
            var target = new GameStateService();
            var playerName = _fixture.Create<string>();
            var nbOfShips = 1;
            target.InitializeNewGame(playerName, nbOfShips);

            var x = 1;
            var y = 1;
            var orientation = "right";
            var size = 2;

            target.AddShipOnPlayersBoard(x, y, orientation, size, true);

            // Act
            var actual = target.AttackPlayersPosition(x, y, 1);

            // Assert
            actual.Should().Be(HitText);
        }

        [Fact]
        public void AttackPlayersPosition_ShouldReturnMissText_WheAttackMiss()
        {
            // Arrange
            var target = new GameStateService();
            var playerName = _fixture.Create<string>();
            var nbOfShips = 1;
            target.InitializeNewGame(playerName, nbOfShips);

            var x = 1;
            var y = 1;
            var orientation = "right";
            var size = 2;
            var missX = x + 1;
            var missY = y + 1;

            target.AddShipOnPlayersBoard(x, y, orientation, size, true);

            // Act
            var actual = target.AttackPlayersPosition(missX, missY, 1);

            // Assert
            actual.Should().Be(MissText);
        }

        [Fact]
        public void GetGameStatus_ShouldReturnGameFinished_WhenAllShipsOfAPlayerAreDestroyed()
        {
            // Arrange
            var target = new GameStateService();
            var playerName = _fixture.Create<string>();
            var nbOfShips = 1;
            target.InitializeNewGame(playerName, nbOfShips);

            var x = 1;
            var y = 1;
            var orientation = "right";
            var size = 2;

            target.AddShipOnPlayersBoard(x, y, orientation, size, true);
            target.AttackPlayersPosition(x, y, 1);
            target.AttackPlayersPosition(x, y + 1, 1);

            // Act
            var actual = target.GetGameStatus();

            // Assert
            actual.Should().Be("Finished");
        }
    }
}