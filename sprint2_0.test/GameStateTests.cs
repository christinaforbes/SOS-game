using SoSGame;

namespace SoSGameTests {
  [TestFixture]
  public class GameStateTests {
    [SetUp]
    public void Setup() {
      GameState.ResetGameState();
    }

    [Test]
    public void CreateNewGameBoardContents_GameBoardContents_IsEqual() {
      char[][] expected = new char[5][] {
        new char[] {'\0', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', '\0' }
      };

      GameState.CreateNewGameBoardContents(5);
      char[][] actual  = GameState.GameBoardContents;

      Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase('S', 0, 0)]
    [TestCase('O', 1, 1)]
    [TestCase('S', 2, 2)]
    public void UpdateGameBoardContents_BoardSquareContent_IsEqual(char letter, int row, int column) {
      GameState.UpdateGameBoardContents(letter, row, column);

      char actual = GameState.GameBoardContents[row][column];

      Assert.That(actual, Is.EqualTo(letter));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateGameInProgressState_GameInProgress_IsTrue(bool gameInProgress) {
      GameState.GameInProgress = gameInProgress;
      
      GameState.UpdateGameInProgressState();
      bool actual = GameState.GameInProgress;

      Assert.That(actual, Is.EqualTo(!gameInProgress));
    }
  }
}