namespace SoSGame {
  class GeneralGameLogic : GameLogic {
    public GeneralGameLogic() : base() {
      GameMode = 'G';
    }

    public GeneralGameLogic(int newBoardSize) : base(newBoardSize) {
      GameMode = 'G';
    }

    internal override bool GameOver() {
      return GameBoardContents.All(row => row.All(square => square != '\0'));
    }

    internal override char DetermineWinner() {
      if (BluePlayerPoints > RedPlayerPoints) {
        return 'B';
      } else if (RedPlayerPoints > BluePlayerPoints) {
        return 'R';
      } else {
        return 'D';
      }
    }
  }
}
