namespace SoSGame {
  class GeneralGameLogic : GameLogic {
    internal static bool GameOver() {
      return GameState.GameBoardContents.All(row => row.All(square => square != '\0'));
    }

    internal static char DetermineWinner() {
      if (GameState.BluePlayerPoints > GameState.RedPlayerPoints) {
        return 'B';
      } else if (GameState.RedPlayerPoints > GameState.BluePlayerPoints) {
        return 'R';
      } else {
        return 'D';
      }
    }
  }
}
