namespace SoSGame {
  class Player {
    public char PlayerType { get; set; }
    public char PlayerColor { get; set; }
    public char PlayerLetter { get; set; }
    public int PlayerPoints { get; set; }

    public Player(char playerColor) {
      PlayerType = 'H';
      PlayerColor = playerColor;
      PlayerLetter = 'S';
      PlayerPoints = 0;
    }

    internal virtual char SelectLetter() {
      throw new NotImplementedException("SelectLetter() must be overridden in a derived class.");
    }

    internal virtual Tuple<int, int> SelectSquare(GameLogic gameLogic) {
      throw new NotImplementedException("SelectSquare() must be overridden in a derived class.");
    }
  }
}
