namespace SoSGame {
  class SimpleGameLogic : GameLogic {
    public SimpleGameLogic() : base() {}

    public SimpleGameLogic(int newBoardSize) : base(newBoardSize) {}

    internal override bool GameOver() {
      bool gameBoardFilled = GameBoardContents.All(row => row.All(square => square != '\0'));
      return (BluePlayer.Points > 0 || RedPlayer.Points > 0 || gameBoardFilled);
    }

    internal override char DetermineWinner() {
      if (BluePlayer.Points > 0) {
        return 'B';
      } else if (RedPlayer.Points > 0) {
        return 'R';
      } else {
        return 'D';
      }
    }
  }
}
