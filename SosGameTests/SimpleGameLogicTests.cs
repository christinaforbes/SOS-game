﻿using SosGame;

namespace SosGameTests {
  [TestFixture]
  public class SimpleGameLogicTests {
    private GameState _gameState;
    private SimpleGameLogic _gameLogic;

    [SetUp]
    public void Setup() {
      _gameState = new GameState(GameLogic.SimpleGame, 4);
      _gameLogic = new SimpleGameLogic(_gameState);
    }

    [TestCase(1, 0)]
    [TestCase(0, 1)]
    [TestCase(2, 0)]
    [TestCase(0, 3)]
    public void Simple_GameOver_SequenceFormed_IsTrue(int bluePlayerPoints, int redPlayerPoints) {
      _gameState.BluePlayer.Points = bluePlayerPoints;
      _gameState.RedPlayer.Points = redPlayerPoints;

      bool actual = _gameLogic.GameOver();

      Assert.That(actual, Is.True);
    }

    [Test]
    public void Simple_GameOver_BoardFilled_IsTrue() {
      _gameState.GameBoardContents = new char[4][] {
        new char[] {'S', 'S', 'O', 'O'},
        new char[] {'S', 'S', 'O', 'O'},
        new char[] {'S', 'S', 'O', 'O'},
        new char[] {'S', 'S', 'O', 'O'}
      };

      bool actual = _gameLogic.GameOver();

      Assert.That(actual, Is.True);
    }

    public void Simple_GameOver_SequenceNotFormedBoardNotFilled_IsFalse() {
      _gameState.GameBoardContents = new char[4][] {
        new char[] {'S', '\0', '\0', '\0'},
        new char[] {'\0', 'S', '\0', '\0'},
        new char[] {'\0', '\0', 'O', '\0'},
        new char[] {'\0', '\0', '\0', 'O'}
      };

      bool actual = _gameLogic.GameOver();

      Assert.That(actual, Is.False);
    }

    public void Simple_GameOver_BoardEmpty_IsFalse() {
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
    [TestCase(2, 0)]
    [TestCase(5, 0)]
    public void Simple_DetermineWinner_BluePlayer_IsEqual(int bluePlayerPoints, int redPlayerPoints) {
      _gameState.BluePlayer.Points = bluePlayerPoints;
      _gameState.RedPlayer.Points = redPlayerPoints;

      char actual = _gameLogic.DetermineWinner();

      Assert.That(actual, Is.EqualTo('B'));
    }

    [TestCase(0, 1)]
    [TestCase(0, 2)]
    [TestCase(0, 5)]
    public void Simple_DetermineWinner_RedPlayer_IsEqual(int bluePlayerPoints, int redPlayerPoints) {
      _gameState.BluePlayer.Points = bluePlayerPoints;
      _gameState.RedPlayer.Points = redPlayerPoints;

      char actual = _gameLogic.DetermineWinner();

      Assert.That(actual, Is.EqualTo('R'));
    }

    [TestCase(0, 0)]
    public void Simple_DetermineWinner_Draw_IsEqual(int bluePlayerPoints, int redPlayerPoints) {
      _gameState.BluePlayer.Points = bluePlayerPoints;
      _gameState.RedPlayer.Points = redPlayerPoints;

      char actual = _gameLogic.DetermineWinner();

      Assert.That(actual, Is.EqualTo('D'));
    }
  }
}
