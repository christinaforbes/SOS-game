namespace SosGame {
  class Move {
    public char Player { get; set; }
    public char Letter { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }

    public Move(char player, char letter, int row, int column) {
      Player = player;
      Letter = letter;
      Row = row;
      Column = column;
    }
  }
}
