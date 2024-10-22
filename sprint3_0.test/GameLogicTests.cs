using SoSGame;

namespace SoSGameTests {
  [TestFixture]
  public class GameLogicTests {
    private GameLogic _gameLogic;

    [SetUp]
    public void Setup() {
      _gameLogic = new GameLogic(5);

      _gameLogic.GameBoardContents = new char[5][] {
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

      _gameLogic.CreateNewGameBoardContents(5);
      char[][] actual = _gameLogic.GameBoardContents;

      Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateGameInProgressState_GameInProgress_IsEqual(bool gameInProgress) {
      _gameLogic.GameInProgress = gameInProgress;

      _gameLogic.UpdateGameInProgressState();
      bool actual = _gameLogic.GameInProgress;

      Assert.That(actual, Is.EqualTo(!gameInProgress));
    }

    [TestCase('S', 0, 0)]
    [TestCase('O', 1, 1)]
    [TestCase('S', 2, 2)]
    public void UpdateGameBoardContents_BoardSquareContent_IsEqual(char letter, int row, int column) {
      _gameLogic.UpdateGameBoardContents(letter, row, column);

      char actual = _gameLogic.GameBoardContents[row][column];

      Assert.That(actual, Is.EqualTo(letter));
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
      bool actual = _gameLogic.IsBoardSizeValid(boardSizeVal);
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
      bool actual = _gameLogic.IsBoardSizeValid(boardSizeVal);
      Assert.That(actual, Is.False);
    }

    [TestCase(0, 1)]
    [TestCase(1, 4)]
    [TestCase(2, 0)]
    [TestCase(3, 3)]
    [TestCase(4, 2)]
    public void IsMoveValid_SquareUnoccupied_IsTrue(int row, int column) {
      bool actual = _gameLogic.IsMoveValid(row, column);
      Assert.That(actual, Is.True);
    }

    [TestCase(0, 0)]
    [TestCase(1, 3)]
    [TestCase(2, 2)]
    [TestCase(3, 1)]
    [TestCase(4, 4)]
    public void IsMoveValid_SquareOccupied_IsFalse(int row, int column) {
      bool actual = _gameLogic.IsMoveValid(row, column);
      Assert.That(actual, Is.False);
    }

    [Test]
    public void SMoveFormsSequence_SequenceFormed_IsEqual() {
      var sequenceSquares = new List<Tuple<int, int>>();
      sequenceSquares.Add(Tuple.Create(1, 3));
      sequenceSquares.Add(Tuple.Create(2, 2));
      sequenceSquares.Add(Tuple.Create(0, 4));
      (bool, List<Tuple<int, int>>) expected = (true, sequenceSquares);

      (bool, List<Tuple<int, int>>) actual = _gameLogic.SMoveFormsSequence(0, 4);

      Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void SMoveFormsSequence_SequenceNotFormed_IsEqual() {
      (bool, List<Tuple<int, int>>) expected = (false, new List<Tuple<int, int>>());

      (bool, List<Tuple<int, int>>) actual = _gameLogic.SMoveFormsSequence(0, 1);

      Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OMoveFormsSequence_SequenceFormed_IsEqual() {
      var sequenceSquares = new List<Tuple<int, int>>();
      sequenceSquares.Add(Tuple.Create(0, 0));
      sequenceSquares.Add(Tuple.Create(2, 2));
      sequenceSquares.Add(Tuple.Create(1, 1));
      (bool, List<Tuple<int, int>>) expected = (true, sequenceSquares);

      (bool, List<Tuple<int, int>>) actual = _gameLogic.OMoveFormsSequence(1, 1);

      Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OMoveFormsSequence_SequenceNotFormed_IsEqual() {
      (bool, List<Tuple<int, int>>) expected = (false, new List<Tuple<int, int>>());

      (bool, List<Tuple<int, int>>) actual = _gameLogic.OMoveFormsSequence(1, 0);

      Assert.That(actual, Is.EqualTo(expected));
    }
  }
}
