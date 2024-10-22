namespace SoSGame {
  class SimpleGameLogic : GameLogic {
    public SimpleGameLogic() : base() {}

    public SimpleGameLogic(int newBoardSize) : base(newBoardSize) {}

    internal override bool GameOver() {
      bool gameBoardFilled = GameBoardContents.All(row => row.All(square => square != '\0'));
      return (BluePlayerPoints > 0 || RedPlayerPoints > 0 || gameBoardFilled);
    }

    internal override char DetermineWinner() {
      if (BluePlayerPoints > 0) {
        return 'B';
      } else if (RedPlayerPoints > 0) {
        return 'R';
      } else {
        return 'D';
      }
    }
  }
}
