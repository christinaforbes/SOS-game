namespace SoSGame {
  class GameLogic {
    internal static bool IsBoardSizeValid(string boardSizeVal) {
      if (int.TryParse(boardSizeVal, out int boardSize)) {
        return (boardSize >= 3 && boardSize <= 10);
      } else {
        return false;
      }
    }

    internal static bool IsMoveValid(int row, int column) {
      return (GameState.GameBoardContents[row][column] == '\0');
    }

    internal static (bool, List<Tuple<int, int>>) SMoveFormsSequence(int row, int column) {
      bool moveFormsSequence = false;
      var sequenceSquares = new List<Tuple<int, int>>();

      int i = row;
      int j = column;
      int size = GameState.BoardSize;
      Tuple<int, int> moveSquare = Tuple.Create(i, j);

      if ((i - 2 >= 0) && (j - 2 >= 0)) {
        if ((GameState.GameBoardContents[i - 1][j - 1] == 'O') && (GameState.GameBoardContents[i - 2][j - 2] == 'S')) {
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
        if ((GameState.GameBoardContents[i - 1][j] == 'O') && (GameState.GameBoardContents[i - 2][j] == 'S')) {
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
        if ((GameState.GameBoardContents[i - 1][j + 1] == 'O') && (GameState.GameBoardContents[i - 2][j + 2] == 'S')) {
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
        if ((GameState.GameBoardContents[i][j - 1] == 'O') && (GameState.GameBoardContents[i][j - 2] == 'S')) {
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
        if ((GameState.GameBoardContents[i][j + 1] == 'O') && (GameState.GameBoardContents[i][j + 2] == 'S')) {
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
        if ((GameState.GameBoardContents[i + 1][j - 1] == 'O') && (GameState.GameBoardContents[i + 2][j - 2] == 'S')) {
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
        if ((GameState.GameBoardContents[i + 1][j] == 'O') && (GameState.GameBoardContents[i + 2][j] == 'S')) {
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
        if ((GameState.GameBoardContents[i + 1][j + 1] == 'O') && (GameState.GameBoardContents[i + 2][j + 2] == 'S')) {
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

    internal static (bool, List<Tuple<int, int>>) OMoveFormsSequence(int row, int column) {
      bool moveFormsSequence = false;
      var sequenceSquares = new List<Tuple<int, int>>();
      
      int i = row;
      int j = column;
      int size = GameState.BoardSize;
      Tuple<int, int> moveSquare = Tuple.Create(i, j);

      if ((i - 1 >= 0) && (j - 1 >= 0) && (i + 1 < size) && (j + 1 < size)) {
        if ((GameState.GameBoardContents[i - 1][j - 1] == 'S') && (GameState.GameBoardContents[i + 1][j + 1] == 'S')) {
          if (!moveFormsSequence) {
            moveFormsSequence = true;
          }

          sequenceSquares.Add(Tuple.Create(i - 1, j - 1));
          sequenceSquares.Add(Tuple.Create(i + 1, j + 1));

          if (!sequenceSquares.Contains(moveSquare)) {
            sequenceSquares.Add(moveSquare);
          }
        }
       
        if ((GameState.GameBoardContents[i - 1][j + 1] == 'S') && (GameState.GameBoardContents[i + 1][j - 1] == 'S')) {
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
        if ((GameState.GameBoardContents[i - 1][j] == 'S') && (GameState.GameBoardContents[i + 1][j] == 'S')) {
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
        if ((GameState.GameBoardContents[i][j - 1] == 'S') && (GameState.GameBoardContents[i][j + 1] == 'S')) {
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
  }
}
