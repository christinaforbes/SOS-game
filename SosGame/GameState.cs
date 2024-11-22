namespace SosGame {
  class GameState {
    public Player BluePlayer { get; set; }
    public Player RedPlayer { get; set; }
    public Player CurrentPlayer { get; set; }
    public int BoardSize { get; set; }
    public char GameMode { get; set; }
    public char[][] GameBoardContents { get; set; }
    public bool GameInProgress { get; set; }
    public bool RecordingGame { get; set; }
    public bool ReplayingGame { get; set; }

    public GameState(char gameMode, int boardSize) {
      BluePlayer = new Player(GameLogic.BluePlayer);
      RedPlayer = new Player(GameLogic.RedPlayer);
      CurrentPlayer = BluePlayer;
      BoardSize = boardSize;
      GameMode = gameMode;
      GameInProgress = false;
      RecordingGame = false;
      ReplayingGame = false;
      GameBoardContents = new char[BoardSize][];
      
      for (int i = 0; i < BoardSize; i++) {
        GameBoardContents[i] = new char[BoardSize];
      }
    }

    internal void CreateNewGameBoardContents(int newBoardSize) {
      BoardSize = newBoardSize;
      GameBoardContents = new char[BoardSize][];

      for (int i = 0; i < BoardSize; i++) {
        GameBoardContents[i] = new char[BoardSize];
      }
    }

    internal void UpdateGameBoardContents(char letter, int row, int column) {
      GameBoardContents[row][column] = letter;

      if (!GameInProgress) {
        UpdateGameInProgressState();
      }
    }

    internal void UpdateGameInProgressState() {
      GameInProgress = !GameInProgress;
    }

    internal void UpdateRecordingGameState(bool newRecordingGameState) {
      if (RecordingGame != newRecordingGameState) {
        RecordingGame = newRecordingGameState;
      }

      if (RecordingGame) {
        GameSave.OverwriteSavedGame();
      }
    }
  }
}
