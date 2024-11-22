namespace SosGame {
  abstract class GameLogic {
    public const char BluePlayer = 'B';
    public const char RedPlayer = 'R';
    public const char Draw = 'D';
    public const char HumanPlayer = 'H';
    public const char ComputerPlayer = 'C';
    public const int DefaultBoardSize = 3;
    public const int MinBoardSize = 3;
    public const int MaxBoardSize = 10;
    public const char SimpleGame = 'S';
    public const char GeneralGame = 'G';
    public const char EmptySquare = '\0';

    protected readonly GameState _gameState;

    protected GameLogic(GameState gameState) {
      _gameState = gameState;
    }

    internal static bool IsBoardSizeValid(string boardSizeVal) {
      if (int.TryParse(boardSizeVal, out int boardSize)) {
        return (boardSize >= MinBoardSize && boardSize <= MaxBoardSize);
      } else {
        return false;
      }
    }

    internal bool IsMoveValid(int row, int column) {
      return (_gameState.GameBoardContents[row][column] == EmptySquare);
    }

    internal static void AddToSequenceSquares(List<Tuple<int, int>> sequenceSquares, int r1, int c1, int r2, int c2, int r3, int c3) {
      Tuple<int, int> moveSquare = Tuple.Create(r1, c1);

      if (!sequenceSquares.Contains(moveSquare)) {
        sequenceSquares.Add(moveSquare);
      }

      sequenceSquares.Add(Tuple.Create(r2, c2));
      sequenceSquares.Add(Tuple.Create(r3, c3));
    }

    internal (bool, List<Tuple<int, int>>) SMoveFormsSequence(int row, int column) {
      bool moveFormsSequence = false;
      var sequenceSquares = new List<Tuple<int, int>>();

      int r = row;
      int c = column;
      int size = _gameState.BoardSize;

      if ((r - 2 >= 0) && (c - 2 >= 0) && (_gameState.GameBoardContents[r - 1][c - 1] == 'O') && (_gameState.GameBoardContents[r - 2][c - 2] == 'S')) {
        moveFormsSequence = true;
        AddToSequenceSquares(sequenceSquares, r, c, r - 1, c - 1, r - 2, c - 2);
      }

      if ((r - 2 >= 0) && (_gameState.GameBoardContents[r - 1][c] == 'O') && (_gameState.GameBoardContents[r - 2][c] == 'S')) {
        moveFormsSequence = true;
        AddToSequenceSquares(sequenceSquares, r, c, r - 1, c, r - 2, c);
      }

      if ((r - 2 >= 0) && (c + 2 < size) && (_gameState.GameBoardContents[r - 1][c + 1] == 'O') && (_gameState.GameBoardContents[r - 2][c + 2] == 'S')) {
        moveFormsSequence = true;
        AddToSequenceSquares(sequenceSquares, r, c, r - 1, c + 1, r - 2, c + 2);
      }

      if ((c - 2 >= 0) && (_gameState.GameBoardContents[r][c - 1] == 'O') && (_gameState.GameBoardContents[r][c - 2] == 'S')) {
        moveFormsSequence = true;
        AddToSequenceSquares(sequenceSquares, r, c, r, c - 1, r, c - 2);
      }

      if ((c + 2 < size) && (_gameState.GameBoardContents[r][c + 1] == 'O') && (_gameState.GameBoardContents[r][c + 2] == 'S')) {
        moveFormsSequence = true;
        AddToSequenceSquares(sequenceSquares, r, c, r, c + 1, r, c + 2);
      }

      if ((r + 2 < size) && (c - 2 >= 0) && (_gameState.GameBoardContents[r + 1][c - 1] == 'O') && (_gameState.GameBoardContents[r + 2][c - 2] == 'S')) {
        moveFormsSequence = true;
        AddToSequenceSquares(sequenceSquares, r, c, r + 1, c - 1, r + 2, c - 2);
      }

      if ((r + 2 < size) && (_gameState.GameBoardContents[r + 1][c] == 'O') && (_gameState.GameBoardContents[r + 2][c] == 'S')) {
        moveFormsSequence = true;
        AddToSequenceSquares(sequenceSquares, r, c, r + 1, c, r + 2, c);
      }

      if ((r + 2 < size) && (c + 2 < size) && (_gameState.GameBoardContents[r + 1][c + 1] == 'O') && (_gameState.GameBoardContents[r + 2][c + 2] == 'S')) {
        moveFormsSequence = true;
        AddToSequenceSquares(sequenceSquares, r, c, r + 1, c + 1, r + 2, c + 2);
      }

      return (moveFormsSequence, sequenceSquares);
    }

    internal (bool, List<Tuple<int, int>>) OMoveFormsSequence(int row, int column) {
      bool moveFormsSequence = false;
      var sequenceSquares = new List<Tuple<int, int>>();
      
      int r = row;
      int c = column;
      int size = _gameState.BoardSize;

      if ((r - 1 >= 0) && (c - 1 >= 0) && (r + 1 < size) && (c + 1 < size)) {
        if ((_gameState.GameBoardContents[r - 1][c - 1] == 'S') && (_gameState.GameBoardContents[r + 1][c + 1] == 'S')) {
          moveFormsSequence = true;
          AddToSequenceSquares(sequenceSquares, r, c, r - 1, c - 1, r + 1, c + 1);
        }
       
        if ((_gameState.GameBoardContents[r - 1][c + 1] == 'S') && (_gameState.GameBoardContents[r + 1][c - 1] == 'S')) {
          moveFormsSequence = true;
          AddToSequenceSquares(sequenceSquares, r, c, r - 1, c + 1, r + 1, c - 1);
        }
      }

      if ((r - 1 >= 0) && (r + 1 < size) && (_gameState.GameBoardContents[r - 1][c] == 'S') && (_gameState.GameBoardContents[r + 1][c] == 'S')) {
        moveFormsSequence = true;
        AddToSequenceSquares(sequenceSquares, r, c, r - 1, c, r + 1, c);
      }

      if ((c - 1 >= 0) && (c + 1 < size) && (_gameState.GameBoardContents[r][c - 1] == 'S') && (_gameState.GameBoardContents[r][c + 1] == 'S')) {
        moveFormsSequence = true;
        AddToSequenceSquares(sequenceSquares, r, c, r, c - 1, r, c + 1);
      }

      return (moveFormsSequence, sequenceSquares);
    }

    internal abstract bool GameOver();

    internal abstract char DetermineWinner();
  }
}
