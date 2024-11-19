namespace SosGame {
  class Player {
    public char Type { get; set; }
    public char Color { get; set; }
    public char Letter { get; set; }
    public int Points { get; set; }

    public Player(char playerColor) {
      Type = 'H';
      Color = playerColor;
      Letter = 'S';
      Points = 0;
    }

    internal virtual char SelectLetter() {
      throw new NotImplementedException("SelectLetter() must be overridden in a derived class.");
    }

    internal virtual Tuple<int, int> SelectSquare(int boardSize, Func<int, int, bool> isMoveValid) {
      throw new NotImplementedException("SelectSquare() must be overridden in a derived class.");
    }
  }
}
