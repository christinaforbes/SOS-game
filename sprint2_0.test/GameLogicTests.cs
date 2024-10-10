using SoSGame;

namespace SoSGameTests {
  [TestFixture]
  public class GameLogicTests {
    [SetUp]
    public void Setup() {
      GameState.GameBoardContents = new char[5][] {
        new char[] {'S', '\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', 'O', '\0'},
        new char[] {'\0', '\0', 'S', '\0', '\0'},
        new char[] {'\0', 'O', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0', 'S'}
      };
    }

    [TestCase(0, 1)]
    [TestCase(1, 4)]
    [TestCase(2, 0)]
    [TestCase(3, 3)]
    [TestCase(4, 2)]
    public void IsMoveValid_SquareUnoccupied_IsTrue(int row, int column) {
      bool actual = GameLogic.IsMoveValid(row, column);
      Assert.That(actual, Is.True);
    }

    [TestCase(0, 0)]
    [TestCase(1, 3)]
    [TestCase(2, 2)]
    [TestCase(3, 1)]
    [TestCase(4, 4)]
    public void IsMoveValid_SquareOccupied_IsFalse(int row, int column) {
      bool actual = GameLogic.IsMoveValid(row, column);
      Assert.That(actual, Is.False);
    }

    [TestCase("3")]
    [TestCase("4")]
    [TestCase("5")]
    [TestCase("6")]
    [TestCase("7")]
    [TestCase("8")]
    [TestCase("9")]
    [TestCase("10")]
    public void IsBoardSizeValid_ValidInput_IsTrue(string boardSizeVal) {
      bool actual = GameLogic.IsBoardSizeValid(boardSizeVal);
      Assert.That(actual, Is.True);
    }

    [TestCase("-1")]
    [TestCase("0")]
    [TestCase("1")]
    [TestCase("2")]
    [TestCase("11")]
    [TestCase("O")]
    [TestCase("l")]
    [TestCase("#")]
    [TestCase("%")]
    [TestCase("*")]
    public void IsBoardSizeValid_InvalidInput_IsFalse(string boardSizeVal) {
      bool actual = GameLogic.IsBoardSizeValid(boardSizeVal);
      Assert.That(actual, Is.False);
    }
  }
}