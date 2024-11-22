﻿using SosGame;

namespace SosGameTests {
  [TestFixture]
  public class GeneralGameLogicTests {
    private GameState _gameState;
    private GeneralGameLogic _gameLogic;

    [SetUp]
    public void Setup() {
      _gameState = new GameState(GameLogic.GeneralGame, 4);
      _gameLogic = new GeneralGameLogic(_gameState);
    }

    [Test]
    public void General_GameOver_BoardFilled_IsTrue() {
      _gameState.GameBoardContents = new char[4][] {
        new char[] {'S', 'O', 'S', 'O'},
        new char[] {'S', 'O', 'S', 'O'},
        new char[] {'S', 'O', 'S', 'O'},
        new char[] {'S', 'O', 'S', 'O'}
      };

      bool actual = _gameLogic.GameOver();

      Assert.That(actual, Is.True);
    }

    [Test]
    public void General_GameOver_BoardNotFilled_IsFalse() {
      _gameState.GameBoardContents = new char[4][] {
        new char[] {'S', 'O', '\0', 'O'},
        new char[] {'S', 'O', '\0', 'O'},
        new char[] {'S', 'O', '\0', 'O'},
        new char[] {'S', 'O', '\0', 'O'}
      };

      bool actual = _gameLogic.GameOver();

      Assert.That(actual, Is.False);
    }

    [Test]
    public void General_GameOver_BoardEmpty_IsFalse() {
      _gameState.GameBoardContents = new char[4][] {
        new char[] {'\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0'},
        new char[] {'\0', '\0', '\0', '\0'}
      };

      bool actual = _gameLogic.GameOver();

      Assert.That(actual, Is.False);
    }

    [TestCase(1, 0)]
    [TestCase(3, 1)]
    [TestCase(10, 5)]
    public void General_DetermineWinner_BluePlayer_IsEqual(int bluePlayerPoints, int redPlayerPoints) {
      _gameState.BluePlayer.Points = bluePlayerPoints;
      _gameState.RedPlayer.Points = redPlayerPoints;

      char actual = _gameLogic.DetermineWinner();

      Assert.That(actual, Is.EqualTo('B'));
    }

    [TestCase(0, 1)]
    [TestCase(1, 3)]
    [TestCase(5, 10)]
    public void General_DetermineWinner_RedPlayer_IsEqual(int bluePlayerPoints, int redPlayerPoints) {
      _gameState.BluePlayer.Points = bluePlayerPoints;
      _gameState.RedPlayer.Points = redPlayerPoints;

      char actual = _gameLogic.DetermineWinner();

      Assert.That(actual, Is.EqualTo('R'));
    }

    [TestCase(0, 0)]
    [TestCase(5, 5)]
    public void General_DetermineWinner_Draw_IsEqual(int bluePlayerPoints, int redPlayerPoints) {
      _gameState.BluePlayer.Points = bluePlayerPoints;
      _gameState.RedPlayer.Points = redPlayerPoints;

      char actual = _gameLogic.DetermineWinner();

      Assert.That(actual, Is.EqualTo('D'));
    }
  }
}
