using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SosGame {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private GameState _gameState;
    private GameLogic _gameLogic;
    private CancellationTokenSource _cancellationTokenSource;
    private DispatcherTimer _replayTimer;

    public MainWindow()
    {
      _gameState = new GameState(GameLogic.SimpleGame, GameLogic.DefaultBoardSize);
      _gameLogic = new SimpleGameLogic(_gameState);
      _cancellationTokenSource = new CancellationTokenSource();
      _replayTimer = new DispatcherTimer();
      InitializeComponent();
    }

    ~MainWindow() {
      _cancellationTokenSource.Dispose();
    }

    private void SetGameMode() {
      int boardSize = Convert.ToInt32(BoardSize.Text);
      string message = "";

      if (SimpleGame.IsChecked == true) {
        _gameState = new GameState(GameLogic.SimpleGame, boardSize);
        _gameLogic = new SimpleGameLogic(_gameState);
        message = "You're now playing a simple game!";
      } else if (GeneralGame.IsChecked == true) {
        _gameState = new GameState(GameLogic.GeneralGame, boardSize);
        _gameLogic = new GeneralGameLogic(_gameState);
        message = "You're now playing a general game!";
      }

      MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ClearGameBoardGrid() {
      GameBoard.Children.Clear();
      GameBoard.RowDefinitions.Clear();
      GameBoard.ColumnDefinitions.Clear();
    }

    private void ResetGameBoardNameScope() {
      NameScope gameBoardNameScope = new NameScope();
      NameScope.SetNameScope(GameBoard, gameBoardNameScope);
    }

    private void InitializeGameBoardGrid(int boardSize) {
      int gridSquareSize = 100 / boardSize;

      for (int i = 0; i < boardSize; i++) {
        RowDefinition rowDefinition = new RowDefinition();
        rowDefinition.Height = new GridLength(gridSquareSize, GridUnitType.Star);
        GameBoard.RowDefinitions.Add(rowDefinition);

        ColumnDefinition columnDefinition = new ColumnDefinition();
        columnDefinition.Width = new GridLength(gridSquareSize, GridUnitType.Star);
        GameBoard.ColumnDefinitions.Add(columnDefinition);
      }
    }

    private void CreateGameBoardSquares(int boardSize) {
      var gameBoardNameScope = NameScope.GetNameScope(GameBoard);

      for (int r = 0; r < boardSize; r++) {
        for (int c = 0; c < boardSize; c++) {
          Button gameBoardSquare = new Button();
          gameBoardSquare.Name = $"Square{r}{c}";
          gameBoardSquare.SetValue(Grid.RowProperty, r);
          gameBoardSquare.SetValue(Grid.ColumnProperty, c);
          gameBoardSquare.FontSize = 24;
          gameBoardSquare.FontWeight = FontWeights.Bold;
          gameBoardSquare.Background = Brushes.WhiteSmoke;
          gameBoardSquare.Click += BoardSquare_Click;
          GameBoard.Children.Add(gameBoardSquare);
          gameBoardNameScope.RegisterName(gameBoardSquare.Name, gameBoardSquare);
        }
      }
    }

    private void CreateNewGameBoard() {
      int boardSize = Convert.ToInt32(BoardSize.Text);

      ClearGameBoardGrid();
      ResetGameBoardNameScope();
      InitializeGameBoardGrid(boardSize);
      CreateGameBoardSquares(boardSize);
      
      _gameState.CreateNewGameBoardContents(boardSize);
    }

    private async void SetBluePlayerType() {
      if (BluePlayerHuman.IsChecked == true) {
        _gameState.BluePlayer = new Player(GameLogic.BluePlayer);
        BluePlayerS.IsEnabled = true;
        BluePlayerO.IsEnabled = true;
      } else if (BluePlayerComputer.IsChecked == true) {
        _gameState.BluePlayer = new ComputerPlayer(GameLogic.BluePlayer);
        BluePlayerS.IsEnabled = false;
        BluePlayerO.IsEnabled = false;

        if (!_gameState.ReplayingGame && _gameState.CurrentPlayer.Color == GameLogic.BluePlayer) {
          await MakeComputerMove();
        }
      }
    }

    private async void SetRedPlayerType() {
      if (RedPlayerHuman.IsChecked == true) {
        _gameState.RedPlayer = new Player(GameLogic.RedPlayer);
        RedPlayerS.IsEnabled = true;
        RedPlayerO.IsEnabled = true;
      } else if (RedPlayerComputer.IsChecked == true) {
        _gameState.RedPlayer = new ComputerPlayer(GameLogic.RedPlayer);
        RedPlayerS.IsEnabled = false;
        RedPlayerO.IsEnabled = false;

        if (!_gameState.ReplayingGame && _gameState.CurrentPlayer.Color == GameLogic.RedPlayer) {
          await MakeComputerMove();
        }
      }
    }

    private void ResetPlayers() {
      SetBluePlayerType();
      SetRedPlayerType();

      BluePlayerS.IsChecked = true;
      RedPlayerS.IsChecked = true;

      SetCurrentPlayer(GameLogic.BluePlayer);
    }

    private void ResetRecordGame() {
      RecordingGame.IsEnabled = true;
      RecordingGame.IsChecked = false;
      _gameState.RecordingGame = false;
    }

    private async void StartNewGame() {
      SetGameMode();
      CreateNewGameBoard();
      ResetPlayers();
      ResetRecordGame();

      if (_gameState.CurrentPlayer.Type == GameLogic.ComputerPlayer) {
        await MakeComputerMove();
      }
    }

    private void MakeMove(Button boardSquare) {
      int row = (int)boardSquare.GetValue(Grid.RowProperty);
      int column = (int)boardSquare.GetValue(Grid.ColumnProperty);

      if (_gameLogic.IsMoveValid(row, column)) {
        Player currentPlayer = _gameState.CurrentPlayer;

        char letter = currentPlayer.Letter;
        boardSquare.Content = letter;
        _gameState.UpdateGameBoardContents(letter, row, column);

        bool moveFormsSequence;
        List<Tuple<int, int>> sequenceSquares;

        if (letter == 'S') {
          (moveFormsSequence, sequenceSquares) = _gameLogic.SMoveFormsSequence(row, column);
        } else {
          (moveFormsSequence, sequenceSquares) = _gameLogic.OMoveFormsSequence(row, column);
        }

        if (moveFormsSequence) {
          ColorSequenceSquares(currentPlayer.Color, sequenceSquares);
          currentPlayer.Points += sequenceSquares.Count / 2;
        }

        Move move = new Move(currentPlayer.Color, letter, row, column);
        EndMove(move, moveFormsSequence);
      }
    }

    private async Task MakeComputerMove() {
      _gameState.CurrentPlayer.Letter = _gameState.CurrentPlayer.SelectLetter();

      Tuple<int, int> square = _gameState.CurrentPlayer.SelectSquare(_gameState.BoardSize, _gameLogic.IsMoveValid);
      string squareName = $"Square{square.Item1}{square.Item2}";
      Button squareButton = (Button)GameBoard.FindName(squareName);

      _cancellationTokenSource.Cancel();
      _cancellationTokenSource = new CancellationTokenSource();
      CancellationToken cancellationToken = _cancellationTokenSource.Token;

      try {
        await Task.Delay(1500, cancellationToken);

        if (!cancellationToken.IsCancellationRequested) {
          squareButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
      } catch (TaskCanceledException) {
        Debug.WriteLine("Computer move cancelled.");
      }
    }

    private void ColorSequenceSquares(char currentPlayer, List<Tuple<int, int>> sequenceSquares) {
      if (currentPlayer == GameLogic.BluePlayer) {
        foreach (Tuple<int, int> sequenceSquare in sequenceSquares) {
          string squareName = $"Square{sequenceSquare.Item1}{sequenceSquare.Item2}";
          Button square = (Button)GameBoard.FindName(squareName);

          if (_gameState.GameMode == GameLogic.SimpleGame) {
            square.Background = Brushes.Blue;
          } else if (_gameState.GameMode == GameLogic.GeneralGame) {
            if (square.Background == Brushes.Red) {
              square.Background = Brushes.Purple;
            } else if (square.Background != Brushes.Purple) {
              square.Background = Brushes.Blue;
            }
          }
        }
      } else if (currentPlayer == GameLogic.RedPlayer) {
        foreach (Tuple<int, int> sequenceSquare in sequenceSquares) {
          string squareName = $"Square{sequenceSquare.Item1}{sequenceSquare.Item2}";
          Button square = (Button)GameBoard.FindName(squareName);

          if (_gameState.GameMode == GameLogic.SimpleGame) {
            square.Background = Brushes.Red;
          } else if (_gameState.GameMode == GameLogic.GeneralGame) {
            if (square.Background == Brushes.Blue) {
              square.Background = Brushes.Purple;
            } else if (square.Background != Brushes.Purple) {
              square.Background = Brushes.Red;
            }
          }
        }
      }
    }

    private void SetCurrentPlayer(char newCurrentPlayer) {
      if (newCurrentPlayer == GameLogic.BluePlayer) {
        CurrentPlayer.Text = "Blue";
        CurrentPlayer.Foreground = Brushes.Blue;
        _gameState.CurrentPlayer = _gameState.BluePlayer;
      } else if (newCurrentPlayer == GameLogic.RedPlayer) {
        CurrentPlayer.Text = "Red";
        CurrentPlayer.Foreground = Brushes.Red;
        _gameState.CurrentPlayer = _gameState.RedPlayer;
      }
    }

    private static void DisplayGameOverMessage(char winner) {
      string gameOverMessage = "";

      if (winner == GameLogic.BluePlayer) {
        gameOverMessage = "Blue Player Wins!";
      } else if (winner == GameLogic.RedPlayer) {
        gameOverMessage = "Red Player Wins!";
      } else {
        gameOverMessage = "Draw Game";
      }

      MessageBox.Show(gameOverMessage, "", MessageBoxButton.OK, MessageBoxImage.None);
    }

    private async void EndMove(Move move, bool moveFormsSequence) {
      if (_gameState.RecordingGame) {
        GameSave.RecordMove(move, _gameState.BoardSize, _gameState.GameMode);
      }

      bool gameOver = _gameLogic.GameOver();

      if (gameOver) {
        char winner = _gameLogic.DetermineWinner();
        DisplayGameOverMessage(winner);
      } else if ((_gameState.GameMode == GameLogic.GeneralGame) && moveFormsSequence) {
        if (!_gameState.ReplayingGame && (_gameState.CurrentPlayer.Type == GameLogic.ComputerPlayer)) {
          await MakeComputerMove();
        }
      } else {
        if (_gameState.CurrentPlayer.Color == GameLogic.BluePlayer) {
          SetCurrentPlayer(GameLogic.RedPlayer);
        } else if (_gameState.CurrentPlayer.Color == GameLogic.RedPlayer) {
          SetCurrentPlayer(GameLogic.BluePlayer);
        }

        if (!_gameState.ReplayingGame && (_gameState.CurrentPlayer.Type == GameLogic.ComputerPlayer)) {
          await MakeComputerMove();
        }
      }
    }

    private void SetUpReplay() {
      _replayTimer = new DispatcherTimer();

      string gameSettingsLine = File.ReadLines(GameSave.SavedGameSettingsFileName).First();
      List<string> gameSettings = gameSettingsLine.Split(',').ToList();
      string boardSize = gameSettings[0];
      char gameMode = gameSettings[1][0];

      BoardSize.Text = boardSize;

      if (gameMode == GameLogic.SimpleGame) {
        SimpleGame.IsChecked = true;
      } else if (gameMode == GameLogic.GeneralGame) {
        GeneralGame.IsChecked = true;
      }

      SetGameMode();
      CreateNewGameBoard();

      _gameState.ReplayingGame = true;

      BluePlayerComputer.IsChecked = true;
      RedPlayerComputer.IsChecked = true;

      ResetPlayers();
      ResetRecordGame();
    }

    private void ReplayMove(string moveLine) {
      List<string> move = moveLine.Split(',').ToList();

      char letter = move[1][0];
      int row = Convert.ToInt32(move[2]);
      int column = Convert.ToInt32(move[3]);

      _gameState.CurrentPlayer.Letter = letter;

      string squareName = $"Square{row}{column}";
      Button squareButton = (Button)GameBoard.FindName(squareName);
      squareButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }

    private void ReplayGame() {
      SetUpReplay();

      List<string> moves = File.ReadAllLines(GameSave.SavedGameFileName).ToList();
      int currentMoveIndex = 0;

      _replayTimer.Interval = TimeSpan.FromMilliseconds(1500);
      _replayTimer.Tick += (sender, e) => {
        if (currentMoveIndex < moves.Count) {
          string move = moves[currentMoveIndex];
          ReplayMove(move);
          currentMoveIndex++;
        } else {
          _replayTimer.Stop();
          _gameState.ReplayingGame = false;
        }
      };
      _replayTimer.Start();
    }

    private void RecordGame_Checked(object sender, RoutedEventArgs e) {
      if (GameGrid.IsLoaded && !_gameState.GameInProgress) {
        string confirmMessage = "Are you sure you want to record your game? The previous saved game will be overwritten";
        MessageBoxResult confirmMessageBox = MessageBox.Show(confirmMessage, "", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (confirmMessageBox == MessageBoxResult.Yes) {
          _gameState.UpdateRecordingGameState(true);
        } else {
          RecordingGame.IsChecked = false;
        }
      }
    }

    private void RecordGame_Unchecked(object sender, RoutedEventArgs e) {
      if (GameGrid.IsLoaded && !_gameState.GameInProgress) {
        _gameState.UpdateRecordingGameState(false);
      }
    }

    private void GameMode_Checked(object sender, RoutedEventArgs e) {
      if (GameGrid.IsLoaded && !_gameState.GameInProgress) {
        SetGameMode();
      }
    }

    private async void BoardSize_TextChanged(object sender, TextChangedEventArgs e) {
      if (GameGrid.IsLoaded) {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = _cancellationTokenSource.Token;

        try {
          await Task.Delay(500, cancellationToken);

          if (!cancellationToken.IsCancellationRequested) {
            string boardSizeTextBoxVal = BoardSize.Text;
            bool isBoardSizeValid = GameLogic.IsBoardSizeValid(boardSizeTextBoxVal);

            if (!isBoardSizeValid && boardSizeTextBoxVal != "") {
              string errorMessage = $"Please enter a valid integer between {GameLogic.MinBoardSize} and {GameLogic.MaxBoardSize}, inclusive";
              MessageBox.Show(errorMessage, "", MessageBoxButton.OK, MessageBoxImage.Error);

              BoardSize.Text = $"{_gameState.BoardSize}";
            } else if (isBoardSizeValid && !_gameState.GameInProgress) {
              CreateNewGameBoard();
            }
          }
        } catch (TaskCanceledException) {
          Debug.WriteLine("Cannot create new game board during game");
        }
      }
    }

    private void BluePlayerType_Checked(object sender, RoutedEventArgs e) { 
      if (GameGrid.IsLoaded && !_gameState.GameInProgress) {
        SetBluePlayerType();
      }
    }

    private void RedPlayerType_Checked(object sender, RoutedEventArgs e) {
      if (GameGrid.IsLoaded && !_gameState.GameInProgress) {
        SetRedPlayerType();
      }
    }

    private void BluePlayerLetter_Checked(object sender, RoutedEventArgs e) {
      if (GameGrid.IsLoaded) {
        if (BluePlayerS.IsChecked == true) {
          _gameState.BluePlayer.Letter = 'S';
        } else if (BluePlayerO.IsChecked == true) {
          _gameState.BluePlayer.Letter = 'O';
        }
      }
    }

    private void RedPlayerLetter_Checked(object sender, RoutedEventArgs e) {
      if (GameGrid.IsLoaded) {
        if (RedPlayerS.IsChecked == true) {
          _gameState.RedPlayer.Letter = 'S';
        } else if (RedPlayerO.IsChecked == true) {
          _gameState.RedPlayer.Letter = 'O';
        }
      }
    }

    private void BoardSquare_Click(object sender, RoutedEventArgs e) {
      if (GameGrid.IsLoaded) {
        Button boardSquare = (Button)sender;
        MakeMove(boardSquare);

        if (_gameState.GameInProgress && RecordingGame.IsEnabled) {
          RecordingGame.IsEnabled = false;
        }
      }
    }

    private void Replay_Click(object sender, RoutedEventArgs e) {
      if (GameGrid.IsLoaded && _gameState.GameInProgress) {
        string confirmMessage = "Are you sure you want to replay your saved game? The current game will be lost";
        MessageBoxResult confirmMessageBox = MessageBox.Show(confirmMessage, "", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (confirmMessageBox == MessageBoxResult.Yes) {
          ReplayGame();
        }
      } else {
        ReplayGame();
      }
    }

    private void NewGame_Click(object sender, RoutedEventArgs e) {
      if (GameGrid.IsLoaded && _gameState.GameInProgress) {
        string confirmMessage = "Are you sure you want to start a new game? The current game will be lost";
        MessageBoxResult confirmMessageBox = MessageBox.Show(confirmMessage, "", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (confirmMessageBox == MessageBoxResult.Yes) {
          StartNewGame();
        }
      } else {
        StartNewGame();
      }
    }
  }
}
