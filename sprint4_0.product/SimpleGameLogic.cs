namespace SoSGame {
  class SimpleGameLogic : GameLogic {
    public SimpleGameLogic() : base() {}

    public SimpleGameLogic(int newBoardSize) : base(newBoardSize) {}

    internal override bool GameOver() {
      bool gameBoardFilled = GameBoardContents.All(row => row.All(square => square != '\0'));
      return (BluePlayer.PlayerPoints > 0 || RedPlayer.PlayerPoints > 0 || gameBoardFilled);
    }

    internal override char DetermineWinner() {
      if (BluePlayer.PlayerPoints > 0) {
        return 'B';
      } else if (RedPlayer.PlayerPoints > 0) {
        return 'R';
      } else {
        return 'D';
      }
    }
  }
}
