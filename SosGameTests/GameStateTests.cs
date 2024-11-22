using SosGame;

namespace SosGameTests {
  [TestFixture]
  public class GameStateTests {
    private GameState _gameState;
    private GameLogic _gameLogic;

    [SetUp]
    public void Setup() {
      _gameState = new GameState(GameLogic.SimpleGame, 5);
      _gameLogic = new SimpleGameLogic(_gameState);

      _gameState.GameBoardContents = new char[5][] {
        new char[] {'S', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', 'O', '\0'},
        new char[] {'\0', '\0', 'S', '\0', '\0'},
        new char[] {'\0', 'O', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', 'S'}
      };
    }

    [Test]
    public void CreateNewGameBoardContents_GameBoardContents_IsEqual() {
      char[][] expected = new char[5][] {
        new char[] {'\0', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', '\0'}
      };

      _gameState.CreateNewGameBoardContents(5);
      char[][] actual = _gameState.GameBoardContents;

      Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase('S', 0, 0)]
    [TestCase('O', 1, 1)]
    [TestCase('S', 2, 2)]
    public void UpdateGameBoardContents_BoardSquareContent_IsEqual(char letter, int row, int column) {
      _gameState.UpdateGameBoardContents(letter, row, column);

      char actual = _gameState.GameBoardContents[row][column];

      Assert.That(actual, Is.EqualTo(letter));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateGameInProgressState_GameInProgress_IsEqual(bool gameInProgress) {
      _gameState.GameInProgress = gameInProgress;

      _gameState.UpdateGameInProgressState();
      bool actual = _gameState.GameInProgress;

      Assert.That(actual, Is.EqualTo(!gameInProgress));
    }

    [Test]
    public void UpdateRecordingGameState_RecordingGame_IsTrue() {
      _gameState.UpdateRecordingGameState(true);

      bool actual = _gameState.RecordingGame;

      Assert.That(actual, Is.True);
    }

    [Test]
    public void UpdateRecordingGameState_RecordingGame_IsFalse() {
      _gameState.UpdateRecordingGameState(false);

      bool actual = _gameState.RecordingGame;

      Assert.That(actual, Is.False);
    }

    [Test]
    public void UpdateRecordingGameState_SavedGameFileLength_IsEqual() {
      _gameState.UpdateRecordingGameState(true);

      FileInfo fileInfo = new FileInfo(GameSave.SavedGameFileName);
      long actual = fileInfo.Length;

      Assert.That(actual, Is.EqualTo(0));
    }
  }
}
