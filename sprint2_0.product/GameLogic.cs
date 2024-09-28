namespace SoSGame {
  class GameLogic {
    public static bool IsMoveValid(int row, int column) {
      return (GameState.GameBoardContents[row][column] == '\0');
    }

    public static bool IsBoardSizeValid(string boardSizeVal) {
      if (int.TryParse(boardSizeVal, out int boardSize)) {
        return (boardSize >= 3 && boardSize <= 10);
      }
      else {
        return false;
      }
    }
  }
}