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
  }
}
