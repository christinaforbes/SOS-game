namespace SoSGame {
  class ComputerPlayer : Player {
    private static readonly Random _random = new Random();

    public ComputerPlayer(char playerLetter) : base(playerLetter) {
      PlayerType = 'C';
    }

    internal override char SelectLetter() {
      List<char> letters = new List<char> {'S', 'O'};
      int index = _random.Next(letters.Count);
      return letters[index];
    }

    internal override Tuple<int, int> SelectSquare(GameLogic gameLogic) {
      int boardSize = gameLogic.BoardSize;
      int row;
      int column;

      do {
        row = _random.Next(boardSize);
        column = _random.Next(boardSize);
      } while (!gameLogic.IsMoveValid(row, column));

      return Tuple.Create(row, column);
    }
  }
}
