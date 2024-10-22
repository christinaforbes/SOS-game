using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoSGame
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private GameLogic _gameLogic = new SimpleGameLogic();
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public MainWindow()
    {
      InitializeComponent();
    }

    private void SetGameMode() {
      int boardSize = Convert.ToInt32(BoardSize.Text);
      string gameMode = "";

      if (SimpleGame.IsChecked == true) {
        _gameLogic = new SimpleGameLogic(boardSize);
        gameMode = "simple";
      } else if (GeneralGame.IsChecked == true) {
        _gameLogic = new GeneralGameLogic(boardSize);
        gameMode = "general";
      }

      string message = $"You're now playing a {gameMode} game!";
      MessageBoxResult gameModeMessageBox = MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void CreateNewGameBoard() {
      GameBoard.Children.Clear();
      GameBoard.RowDefinitions.Clear();
      GameBoard.ColumnDefinitions.Clear();

      NameScope gameBoardNameScope = new NameScope();
      NameScope.SetNameScope(GameBoard, gameBoardNameScope);

      int boardSize = Convert.ToInt32(BoardSize.Text);
      int gridSquareSize = 100 / boardSize;

      for (int i = 0; i < boardSize; i++) {
        RowDefinition rowDefinition = new RowDefinition();
        rowDefinition.Height = new GridLength(gridSquareSize, GridUnitType.Star);
        GameBoard.RowDefinitions.Add(rowDefinition);

        ColumnDefinition columnDefinition = new ColumnDefinition();
        columnDefinition.Width = new GridLength(gridSquareSize, GridUnitType.Star);
        GameBoard.ColumnDefinitions.Add(columnDefinition);
      }

      for (int i = 0; i < boardSize; i++) {
        for (int j = 0; j < boardSize; j++) {
          Button gameBoardSquare = new Button();
          gameBoardSquare.Name = $"Square{i}{j}";
          gameBoardSquare.SetValue(Grid.RowProperty, i);
          gameBoardSquare.SetValue(Grid.ColumnProperty, j);
          gameBoardSquare.FontSize = 24;
          gameBoardSquare.FontWeight = FontWeights.Bold;
          gameBoardSquare.Background = Brushes.WhiteSmoke;
          gameBoardSquare.Click += BoardSquare_Click;
          GameBoard.Children.Add(gameBoardSquare);
          gameBoardNameScope.RegisterName(gameBoardSquare.Name, gameBoardSquare);
        }
      }

      _gameLogic.CreateNewGameBoardContents(boardSize);
    }

    private void StartNewGame() {
      SetGameMode();
      CreateNewGameBoard();
      
      CurrentPlayer.Text = "Blue";
      CurrentPlayer.Foreground = Brushes.Blue;
      BluePlayerS.IsChecked = true;
      RedPlayerS.IsChecked = true;
    }

    private void GameMode_Checked(object sender, RoutedEventArgs e) {
      if (GameGrid.IsLoaded && !_gameLogic.GameInProgress) {
        SetGameMode();
      }
    }

    private async void BoardSize_TextChanged(object sender, TextChangedEventArgs e) {
      _cancellationTokenSource.Cancel();
      _cancellationTokenSource = new CancellationTokenSource();
      CancellationToken cancellationToken = _cancellationTokenSource.Token;

      try {
        await Task.Delay(500, cancellationToken);

        if (!cancellationToken.IsCancellationRequested) {
          string boardSizeTextBoxVal = BoardSize.Text;
          bool isBoardSizeValid = _gameLogic.IsBoardSizeValid(boardSizeTextBoxVal);

          if (!isBoardSizeValid && boardSizeTextBoxVal != "") {
            string errorMessage = "Please enter a valid integer between 3 and 10, inclusive";
            MessageBoxResult errorMessageBox = MessageBox.Show(errorMessage, "Invalid Board Size", MessageBoxButton.OK, MessageBoxImage.Error);

            BoardSize.Text = $"{_gameLogic.BoardSize}";
          } else if (isBoardSizeValid && !_gameLogic.GameInProgress) {
            CreateNewGameBoard();
          }
        }
      } catch (TaskCanceledException) {
        return;
      }
    }

    private void BluePlayerLetter_Checked(object sender, RoutedEventArgs e) {
      if (BluePlayerS.IsChecked == true) {
        _gameLogic.BluePlayerLetter = 'S';
      } else if (BluePlayerO.IsChecked == true) {
        _gameLogic.BluePlayerLetter = 'O';
      }
    }

    private void RedPlayerLetter_Checked(object sender, RoutedEventArgs e) {
      if (RedPlayerS.IsChecked == true) {
        _gameLogic.RedPlayerLetter ='S';
      } else if (RedPlayerO.IsChecked == true) {
        _gameLogic.RedPlayerLetter = 'O';
      }
    }

    private void ColorSequenceSquares(char currentPlayer, List<Tuple<int, int>> sequenceSquares) {
      if (currentPlayer == 'B') {
        foreach (Tuple<int, int> sequenceSquare in sequenceSquares) {
          string squareName = $"Square{sequenceSquare.Item1}{sequenceSquare.Item2}";
          Button square = (Button)GameBoard.FindName(squareName);

          if (_gameLogic.GameMode == 'S') {
            square.Background = Brushes.Blue;
          } else if (_gameLogic.GameMode == 'G') {
            if (square.Background == Brushes.Purple) {
              continue;
            } else if (square.Background == Brushes.Red) {
              square.Background = Brushes.Purple;
            } else {
              square.Background = Brushes.Blue;
            }
          }
        }
      } else if (currentPlayer == 'R') {
        foreach (Tuple<int, int> sequenceSquare in sequenceSquares) {
          string squareName = $"Square{sequenceSquare.Item1}{sequenceSquare.Item2}";
          Button square = (Button)GameBoard.FindName(squareName);

          if (_gameLogic.GameMode == 'S') {
            square.Background = Brushes.Red;
          } else if (_gameLogic.GameMode == 'G') {
            if (square.Background == Brushes.Purple) {
              continue;
            } else if (square.Background == Brushes.Blue) {
              square.Background = Brushes.Purple;
            } else {
              square.Background = Brushes.Red;
            }
          }
        }
      }
    }

    private void DisplayGameOverMessage(char winner) {
      string gameOverMessage = "";

      if (winner == 'B') {
        gameOverMessage = "Blue Player Wins!";
      } else if (winner == 'R') {
        gameOverMessage = "Red Player Wins!";
      } else {
        gameOverMessage = "Draw Game";
      }

      MessageBoxResult gameOverMessageBox = MessageBox.Show(gameOverMessage, "", MessageBoxButton.OK, MessageBoxImage.None);
    }

    private void EndMove(char currentPlayer, bool moveFormsSequence) {
      if (currentPlayer == 'B') {
        if (_gameLogic.GameMode == 'S') {
          bool gameOver = _gameLogic.GameOver();

          if (gameOver) {
            char winner = _gameLogic.DetermineWinner();
            DisplayGameOverMessage(winner);
          } else {
            CurrentPlayer.Text = "Red";
            CurrentPlayer.Foreground = Brushes.Red;
            _gameLogic.CurrentPlayer = 'R';
          }
        } else if (_gameLogic.GameMode == 'G') {
          bool gameOver = _gameLogic.GameOver();
      
          if (gameOver) {
            char winner = _gameLogic.DetermineWinner();
            DisplayGameOverMessage(winner);
          } else if (moveFormsSequence && !gameOver) {
            return;
          } else {
            CurrentPlayer.Text = "Red";
            CurrentPlayer.Foreground = Brushes.Red;
            _gameLogic.CurrentPlayer = 'R';
          }
        }
      } else if (currentPlayer == 'R') {
        if (_gameLogic.GameMode == 'S') {
          bool gameOver = _gameLogic.GameOver();

          if (gameOver) {
            char winner = _gameLogic.DetermineWinner();
            DisplayGameOverMessage(winner);
          } else {
            CurrentPlayer.Text = "Blue";
            CurrentPlayer.Foreground = Brushes.Blue;
            _gameLogic.CurrentPlayer = 'B';
          }
        } else if (_gameLogic.GameMode == 'G') {
          bool gameOver = _gameLogic.GameOver();

          if (gameOver) {
            char winner = _gameLogic.DetermineWinner();
            DisplayGameOverMessage(winner);
          } else if (moveFormsSequence && !gameOver) {
            return;
          } else {
            CurrentPlayer.Text = "Blue";
            CurrentPlayer.Foreground = Brushes.Blue;
            _gameLogic.CurrentPlayer = 'B';
          }
        }
      }
    }

    private void BoardSquare_Click(object sender, RoutedEventArgs e)
    {
      Button boardSquare = (Button)sender;

      int row = (int)boardSquare.GetValue(Grid.RowProperty);
      int column = (int)boardSquare.GetValue(Grid.ColumnProperty);

      if (_gameLogic.IsMoveValid(row, column)) {
        if (_gameLogic.CurrentPlayer == 'B') {
          if (_gameLogic.BluePlayerLetter == 'S') {
            boardSquare.Content = "S";
            _gameLogic.UpdateGameBoardContents('S', row, column);

            (bool moveFormsSequence, List<Tuple<int, int>> sequenceSquares) = _gameLogic.SMoveFormsSequence(row, column);

            if (moveFormsSequence) {
              ColorSequenceSquares('B', sequenceSquares);
              _gameLogic.BluePlayerPoints = sequenceSquares.Count / 2;
            }

            EndMove('B', moveFormsSequence);
          } else if (_gameLogic.BluePlayerLetter == 'O') {
            boardSquare.Content = "O";
            _gameLogic.UpdateGameBoardContents('O', row, column);

            (bool moveFormsSequence, List<Tuple<int, int>> sequenceSquares) = _gameLogic.OMoveFormsSequence(row, column);

            if (moveFormsSequence) {
              ColorSequenceSquares('B', sequenceSquares);
              _gameLogic.BluePlayerPoints = sequenceSquares.Count / 2;
            }

            EndMove('B', moveFormsSequence);
          }
        } else if (_gameLogic.CurrentPlayer == 'R') {
          if (_gameLogic.RedPlayerLetter == 'S') {
            boardSquare.Content = "S";
            _gameLogic.UpdateGameBoardContents('S', row, column);

            (bool moveFormsSequence, List<Tuple<int, int>> sequenceSquares) = _gameLogic.SMoveFormsSequence(row, column);

            if (moveFormsSequence) {
              ColorSequenceSquares('R', sequenceSquares);
              _gameLogic.RedPlayerPoints = sequenceSquares.Count / 2;
            }

            EndMove('R', moveFormsSequence);
          } else if (_gameLogic.RedPlayerLetter == 'O') {
            boardSquare.Content = "O";
            _gameLogic.UpdateGameBoardContents('O', row, column);

            (bool moveFormsSequence, List<Tuple<int, int>> sequenceSquares) = _gameLogic.OMoveFormsSequence(row, column);

            if (moveFormsSequence) {
              ColorSequenceSquares('R', sequenceSquares);
              _gameLogic.RedPlayerPoints = sequenceSquares.Count / 2;
            }

            EndMove('R', moveFormsSequence);
          }
        }
      }
    }

    private void NewGame_Click(object sender, RoutedEventArgs e) {
      if (_gameLogic.GameInProgress) {
        string confirmMessage = "Are you sure you want to start a new game?";
        MessageBoxResult confirmMessageBox = MessageBox.Show(confirmMessage, "", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (confirmMessageBox == MessageBoxResult.Yes) {
          StartNewGame();
        }
      }
      else {
        StartNewGame();
      }
    }
  }
}
