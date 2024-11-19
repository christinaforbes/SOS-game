using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;

namespace SosGame {
  class GameLogic {
    // Game State
    internal const string SavedGameFileName = "SavedGame.csv";
    public Player BluePlayer { get; set; }
    public Player RedPlayer { get; set; }
    public Player CurrentPlayer { get; set; }
    public int BoardSize { get; set; }
    public char GameMode { get; set; }
    public bool GameInProgress { get; set; }
    public char[][] GameBoardContents { get; set; }
    public bool RecordingGame { get; set; }
    public bool ReplayingGame { get; set; }
    
    public GameLogic() {
      BluePlayer = new Player('B');
      RedPlayer = new Player('R');
      CurrentPlayer = BluePlayer;
      BoardSize = 3;
      GameMode = 'S';
      GameInProgress = false;
      GameBoardContents = new char[BoardSize][];
      RecordingGame = false;
      ReplayingGame = false;

      for (int i = 0; i < BoardSize; i++) {
        GameBoardContents[i] = new char[BoardSize];
      }
    }

    public GameLogic(int newBoardSize) : this() {
      BoardSize = newBoardSize;
      GameBoardContents = new char[BoardSize][];

      for (int i = 0; i < BoardSize; i++) {
        GameBoardContents[i] = new char[BoardSize];
      }
    }

    internal void CreateNewGameBoardContents(int newBoardSize) {
      BoardSize = newBoardSize;
      GameBoardContents = new char[BoardSize][];

      for (int i = 0; i < BoardSize; i++) {
        GameBoardContents[i] = new char[BoardSize];
      }
    }

    internal void UpdateGameInProgressState() {
      GameInProgress = !GameInProgress;
    }

    internal void UpdateGameBoardContents(char letter, int row, int column) {
      GameBoardContents[row][column] = letter;

      if (!GameInProgress) {
        UpdateGameInProgressState();
      }
    }

    internal void UpdateRecordingGameState(bool newRecordingGameState) {
      if (RecordingGame != newRecordingGameState) {
        RecordingGame = newRecordingGameState;
      }

      if (RecordingGame) {
        File.WriteAllText(SavedGameFileName, "");
      }
    }

    // Game Logic
    internal bool IsBoardSizeValid(string boardSizeVal) {
      if (int.TryParse(boardSizeVal, out int boardSize)) {
        return (boardSize >= 3 && boardSize <= 10);
      } else {
        return false;
      }
    }

    internal bool IsMoveValid(int row, int column) {
      return (GameBoardContents[row][column] == '\0');
    }

    internal (bool, List<Tuple<int, int>>) SMoveFormsSequence(int row, int column) {
      bool moveFormsSequence = false;
      var sequenceSquares = new List<Tuple<int, int>>();

      int i = row;
      int j = column;
      int size = BoardSize;
      Tuple<int, int> moveSquare = Tuple.Create(i, j);

      if ((i - 2 >= 0) && (j - 2 >= 0)) {
        if ((GameBoardContents[i - 1][j - 1] == 'O') && (GameBoardContents[i - 2][j - 2] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i - 1, j - 1));
          sequenceSquares.Add(Tuple.Create(i - 2, j - 2));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
      }

      if (i - 2 >= 0) {
        if ((GameBoardContents[i - 1][j] == 'O') && (GameBoardContents[i - 2][j] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i - 1, j));
          sequenceSquares.Add(Tuple.Create(i - 2, j));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
      }

      if ((i - 2 >= 0) && (j + 2 < size)) {
        if ((GameBoardContents[i - 1][j + 1] == 'O') && (GameBoardContents[i - 2][j + 2] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i - 1, j + 1));
          sequenceSquares.Add(Tuple.Create(i - 2, j + 2));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
      }

      if (j - 2 >= 0) {
        if ((GameBoardContents[i][j - 1] == 'O') && (GameBoardContents[i][j - 2] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i, j - 1));
          sequenceSquares.Add(Tuple.Create(i, j - 2));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
      }

      if (j + 2 < size) {
        if ((GameBoardContents[i][j + 1] == 'O') && (GameBoardContents[i][j + 2] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i, j + 1));
          sequenceSquares.Add(Tuple.Create(i, j + 2));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
      }

      if ((i + 2 < size) && (j - 2 >= 0)) {
        if ((GameBoardContents[i + 1][j - 1] == 'O') && (GameBoardContents[i + 2][j - 2] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i + 1, j - 1));
          sequenceSquares.Add(Tuple.Create(i + 2, j - 2));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
      }

      if (i + 2 < size) {
        if ((GameBoardContents[i + 1][j] == 'O') && (GameBoardContents[i + 2][j] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i + 1, j));
          sequenceSquares.Add(Tuple.Create(i + 2, j));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
      }

      if ((i + 2 < size) && (j + 2 < size)) {
        if ((GameBoardContents[i + 1][j + 1] == 'O') && (GameBoardContents[i + 2][j + 2] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i + 1, j + 1));
          sequenceSquares.Add(Tuple.Create(i + 2, j + 2));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
      }

      return (moveFormsSequence, sequenceSquares);
    }

    internal (bool, List<Tuple<int, int>>) OMoveFormsSequence(int row, int column) {
      bool moveFormsSequence = false;
      var sequenceSquares = new List<Tuple<int, int>>();
      
      int i = row;
      int j = column;
      int size = BoardSize;
      Tuple<int, int> moveSquare = Tuple.Create(i, j);

      if ((i - 1 >= 0) && (j - 1 >= 0) && (i + 1 < size) && (j + 1 < size)) {
        if ((GameBoardContents[i - 1][j - 1] == 'S') && (GameBoardContents[i + 1][j + 1] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i - 1, j - 1));
          sequenceSquares.Add(Tuple.Create(i + 1, j + 1));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
       
        if ((GameBoardContents[i - 1][j + 1] == 'S') && (GameBoardContents[i + 1][j - 1] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i - 1, j + 1));
          sequenceSquares.Add(Tuple.Create(i + 1, j - 1));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
      }

      if ((i - 1 >= 0) && (i + 1 < size)) {
        if ((GameBoardContents[i - 1][j] == 'S') && (GameBoardContents[i + 1][j] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i - 1, j));
          sequenceSquares.Add(Tuple.Create(i + 1, j));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
      }

      if ((j - 1 >= 0) && (j + 1 < size)) {
        if ((GameBoardContents[i][j - 1] == 'S') && (GameBoardContents[i][j + 1] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i, j - 1));
          sequenceSquares.Add(Tuple.Create(i, j + 1));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
      }

      return (moveFormsSequence, sequenceSquares);
    }

    internal virtual bool GameOver() {
      throw new NotImplementedException("GameOver() must be overridden in a derived class.");
    }

    internal virtual char DetermineWinner() {
      throw new NotImplementedException("DetermineWinner() must be overridden in a derived class.");
    }

    internal void RecordMove(char player, char letter, int row, int columm) {
      FileInfo fileInfo = new FileInfo(SavedGameFileName);

      if (fileInfo.Length == 0) {
        List<string> lines = new List<string>() {
          $"{BoardSize},{GameMode}",
          $"{player},{letter},{row},{columm}"
        };
        File.AppendAllLines(SavedGameFileName, lines);
      } else {
        File.AppendAllText(SavedGameFileName, $"{player},{letter},{row},{columm}{Environment.NewLine}");
      }
    }
  }
}
