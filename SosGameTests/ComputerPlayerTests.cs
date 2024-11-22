using SosGame;

namespace SosGameTests {
  [TestFixture]
  public class ComputerPlayerTests {
    private GameState _gameState;
    private GameLogic _gameLogic;

    [SetUp]
    public void Setup() {
      _gameState = new GameState(GameLogic.SimpleGame, 5);
      _gameLogic = new SimpleGameLogic(_gameState);

      _gameState.BluePlayer = new ComputerPlayer(GameLogic.BluePlayer);
      _gameState.GameBoardContents = new char[5][] {
        new char[] {'S', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', 'O', '\0'},
        new char[] {'\0', '\0', 'S', '\0', '\0'},
        new char[] {'\0', 'O', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', 'S'}
      };
    }

    [Test]
    public void SelectLetter_Letter_IsEqual() {
      char actual = _gameState.BluePlayer.SelectLetter();
      Assert.That(actual, Is.EqualTo('S').Or.EqualTo('O'));
    }

    [Test]
    public void SelectSquare_ValidSquare_IsTrue() {
      (int row, int column) = _gameState.BluePlayer.SelectSquare(_gameState.BoardSize, _gameLogic.IsMoveValid);
      bool actual = _gameLogic.IsMoveValid(row, column);

      Assert.That(actual, Is.True);
    }
  }
}
