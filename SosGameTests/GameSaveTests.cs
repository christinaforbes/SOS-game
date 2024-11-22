using NUnit.Framework;
using SosGame;

namespace SosGameTests {
  [TestFixture]
  public class GameSaveTests {
    [SetUp]
    public void Setup() {}

    [Test]
    public void OverwriteSavedGame_SavedGameFileLength_IsEqual() {
      long expected = 0;

      GameSave.OverwriteSavedGame();
      FileInfo savedGameFileInfo = new FileInfo(GameSave.SavedGameFileName);
      long actual = savedGameFileInfo.Length;

      Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OverwriteSavedGame_SavedGameSettingsFileLength_IsEqual() {
      long expected = 0;

      GameSave.OverwriteSavedGame();
      FileInfo savedGameSettingsFileInfo = new FileInfo(GameSave.SavedGameSettingsFileName);
      long actual = savedGameSettingsFileInfo.Length;

      Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase('B', 'S', 0, 2)]
    [TestCase('R', 'O', 1, 1)]
    [TestCase('B', 'O', 2, 4)]
    [TestCase('R', 'S', 3, 2)]
    public void RecordMove_SavedGameFileLastLine_IsEqual(char player, char letter, int row, int column) {
      string expected = $"{player},{letter},{row},{column}";

      Move move = new Move(player, letter, row, column);
      GameSave.RecordMove(move, 5, 'S');
      string actual = File.ReadLines(GameSave.SavedGameFileName).Last();

      Assert.That(actual, Is.EqualTo(expected));
    }
  }
}
