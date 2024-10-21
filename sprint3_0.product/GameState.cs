namespace SoSGame {
  internal class GameState
  {
    public static int BoardSize { get; set; }
    public static char GameMode { get; set; }
    public static char CurrentPlayer { get; set; }
    public static char BluePlayerLetter { get; set; }
    public static char RedPlayerLetter { get; set; }
    public static int BluePlayerPoints { get; set; }
    public static int RedPlayerPoints { get; set; }
    public static bool GameInProgress { get; set; }
    public static char[][] GameBoardContents { get; set; }

    static GameState() {
      BoardSize = 3;
      GameMode = 'S';
      CurrentPlayer = 'B';
      BluePlayerLetter = 'S';
      RedPlayerLetter = 'S';
      BluePlayerPoints = 0;
      RedPlayerPoints = 0;
      GameInProgress = false;
      GameBoardContents = new char[BoardSize][];

      for (int i = 0; i < BoardSize; i++) {
        GameBoardContents[i] = new char[BoardSize];
      }
    }

    public static void ResetGameState() {
      CurrentPlayer = 'B';
      BluePlayerLetter = 'S';
      RedPlayerLetter = 'S';
      BluePlayerPoints = 0;
      RedPlayerPoints = 0;
      GameInProgress = false;
      GameBoardContents = new char[BoardSize][];

      for (int i = 0; i < BoardSize; i++) {
        GameBoardContents[i] = new char[BoardSize];
      }
    }

    public static void CreateNewGameBoardContents(int newBoardSize) {
      BoardSize = newBoardSize;

      GameBoardContents = new char[BoardSize][];

      for (int i = 0; i < BoardSize; i++) {
        GameBoardContents[i] = new char[BoardSize];
      }
    }

    public static void UpdateGameInProgressState() {
      GameInProgress = !GameInProgress;
    }

    public static void UpdateGameBoardContents(char letter, int row, int column) {
      GameBoardContents[row][column] = letter;

      if (!GameInProgress) {
        UpdateGameInProgressState();
      }
    }
  }
}
