﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
    public MainWindow()
    {
      InitializeComponent();
    }

    private void BluePlayerLetter_Checked(object sender, RoutedEventArgs e) {
      if (BluePlayerS.IsChecked == true) {
        GameState.UpdateBluePlayerLetter('S');
      } else if (BluePlayerO.IsChecked == true) {
        GameState.UpdateBluePlayerLetter('O');
      }

      return;
    }

    private void RedPlayerLetter_Checked(object sender, RoutedEventArgs e) {
      if (RedPlayerS.IsChecked == true) {
        GameState.UpdateRedPlayerLetter('S');
      } else if (RedPlayerO.IsChecked == true) {
        GameState.UpdateRedPlayerLetter('O');
      }
    }

    private void BoardSquareButton_Click(object sender, RoutedEventArgs e)
    {
      Button boardSquare = (Button)sender;

      int row = (int)boardSquare.GetValue(Grid.RowProperty);
      int column = (int)boardSquare.GetValue(Grid.ColumnProperty);

      if (GameLogic.IsMoveValid(row, column)) {
        if (GameState.CurrentPlayer == 'B') {
          if (GameState.BluePlayerLetter == 'S') {
            boardSquare.Content = "S";
            GameState.UpdateGameBoardContents('S', row, column);
          } else if (GameState.BluePlayerLetter == 'O') {
            boardSquare.Content = "O";
            GameState.UpdateGameBoardContents('O', row, column);
          }

          CurrentPlayer.Text = "Red";
          CurrentPlayer.Foreground = Brushes.Red;
          GameState.UpdateCurrentPlayer('R');
        } else if (GameState.CurrentPlayer == 'R') {
          if (GameState.RedPlayerLetter == 'S') {
            boardSquare.Content = "S";
            GameState.UpdateGameBoardContents('S', row, column);
          } else if (GameState.RedPlayerLetter == 'O') {
            boardSquare.Content = "O";
            GameState.UpdateGameBoardContents('O', row, column);
          }

          CurrentPlayer.Text = "Blue";
          CurrentPlayer.Foreground = Brushes.Blue;
          GameState.UpdateCurrentPlayer('B');
        }
      }
    }

    private void BoardSize_TextChanged(object sender, TextChangedEventArgs e) {
      string boardSizeTextBoxVal = BoardSize.Text;

      if (!GameLogic.IsBoardSizeValid(boardSizeTextBoxVal) && boardSizeTextBoxVal != "") {
        string errorMessage = "Please enter a valid integer between 3 and 10, inclusive";
        MessageBoxResult errorMessageBox = MessageBox.Show(errorMessage, "Invalid Board Size", MessageBoxButton.OK, MessageBoxImage.Error);
      }

      return;
    }

    private void CreateNewGameBoard() {
      // create game board
      GameBoard.Children.Clear();
      GameBoard.RowDefinitions.Clear();
      GameBoard.ColumnDefinitions.Clear();

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
          gameBoardSquare.Click += BoardSquareButton_Click;
          GameBoard.Children.Add(gameBoardSquare);
        }
      }

      GameState.CreateNewGameBoardContents(boardSize);
    }

    private void SetGameMode() {
      if (SimpleGame.IsChecked == true) {
        GameState.UpdateGameMode('S');
      }
      else if (GeneralGame.IsChecked == true) {
        GameState.UpdateGameMode('G');
      }

      string gameMode = (GameState.GameMode == 'S') ? "simple" : "general";
      string message = $"You're now playing a {gameMode} game!";
      MessageBoxResult gameModeMessageBox = MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void StartNewGame() {
      CreateNewGameBoard();
      SetGameMode();

      CurrentPlayer.Text = "Blue";
      CurrentPlayer.Foreground = Brushes.Blue;
      BluePlayerS.IsChecked = true;
      RedPlayerS.IsChecked = true;

      GameState.ResetGameState();
    }

    private void NewGameButton_Click(object sender, RoutedEventArgs e) {
      if (GameState.GameInProgress) {
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