using System.IO;

namespace SosGame {
  static class GameSave {
    internal const string SavedGameFileName = "SavedGame.csv";
    internal const string SavedGameSettingsFileName = "SavedGameSettings.csv";

    internal static void OverwriteSavedGame() {
      File.WriteAllText(GameSave.SavedGameFileName, "");
      File.WriteAllText(GameSave.SavedGameSettingsFileName, "");
    }

    internal static void RecordMove(Move move, int boardSize, char gameMode) {
      string gameSettings = $"{boardSize},{gameMode}";
      string moveLine = $"{move.Player},{move.Letter},{move.Row},{move.Column}{Environment.NewLine}";
      FileInfo savedGameFileInfo = new FileInfo(SavedGameFileName);
      
      if (savedGameFileInfo.Length == 0) {
        File.AppendAllText(SavedGameSettingsFileName, gameSettings);
        File.AppendAllText(SavedGameFileName, moveLine);
      } else {
        File.AppendAllText(SavedGameFileName, moveLine);
      }
    }
  }
}
