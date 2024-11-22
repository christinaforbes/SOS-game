namespace SosGame {
  class SimpleGameLogic : GameLogic {
    public SimpleGameLogic(GameState gameState) : base(gameState) {}

    internal override bool GameOver() {
      bool gameBoardFilled = Array.TrueForAll(_gameState.GameBoardContents, row => Array.TrueForAll(row, square => square != EmptySquare));
      return (_gameState.BluePlayer.Points > 0 || _gameState.RedPlayer.Points > 0 || gameBoardFilled);
    }

    internal override char DetermineWinner() {
      if (_gameState.BluePlayer.Points > 0) {
        return BluePlayer;
      } else if (_gameState.RedPlayer.Points > 0) {
        return RedPlayer;
      } else {
        return Draw;
      }
    }
  }
}
