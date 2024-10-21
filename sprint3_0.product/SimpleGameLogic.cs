namespace SoSGame {
  class SimpleGameLogic : GameLogic {
    internal static bool GameOver() {
      bool gameBoardFilled = GameState.GameBoardContents.All(row => row.All(square => square != '\0'));
      return (GameState.BluePlayerPoints > 0 || GameState.RedPlayerPoints > 0 || gameBoardFilled);
    }

    internal static char DetermineWinner() {
      if (GameState.BluePlayerPoints > 0) {
        return 'B';
      } else if (GameState.RedPlayerPoints > 0) {
        return 'R';
      } else {
        return 'D';
      }
    }
  }
}
