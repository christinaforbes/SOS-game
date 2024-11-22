namespace SosGame {
  class GeneralGameLogic : GameLogic {
    public GeneralGameLogic(GameState gameState) : base(gameState) {}

    internal override bool GameOver() {
      return Array.TrueForAll(_gameState.GameBoardContents, row => Array.TrueForAll(row, square => square != EmptySquare));
    }

    internal override char DetermineWinner() {
      if (_gameState.BluePlayer.Points > _gameState.RedPlayer.Points) {
        return BluePlayer;
      } else if (_gameState.RedPlayer.Points > _gameState.BluePlayer.Points) {
        return RedPlayer;
      } else {
        return Draw;
      }
    }
  }
}
