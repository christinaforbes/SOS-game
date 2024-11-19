using Newtonsoft.Json.Linq;
using SosGame;

namespace SosGameTests {
  [TestFixture]
  public class ComputerPlayerTests {
    private GameLogic _gameLogic;

    [SetUp]
    public void Setup() {
      _gameLogic = new GameLogic(5);
      _gameLogic.BluePlayer = new ComputerPlayer('B');

      _gameLogic.GameBoardContents = new char[5][] {
        new char[] {'S', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', 'O', '\0'},
        new char[] {'\0', '\0', 'S', '\0', '\0'},
        new char[] {'\0', 'O', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', 'S'}
      };
    }

    [Test]
    public void SelectLetter_Letter_IsEqual() {
      char actual = _gameLogic.BluePlayer.SelectLetter();
      Assert.That(actual, Is.EqualTo('S').Or.EqualTo('O'));
    }

    [Test]
    public void SelectSquare_ValidSquare_IsTrue() {
      (int row, int column) = _gameLogic.BluePlayer.SelectSquare(_gameLogic.BoardSize, _gameLogic.IsMoveValid);

      bool actual = _gameLogic.IsMoveValid(row, column);

      Assert.That(actual, Is.True);
    }
  }
}
