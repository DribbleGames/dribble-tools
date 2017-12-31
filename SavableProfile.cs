using System.Collections.Generic;

namespace Dribble.Savable {
   public static class SavableProfile {
      private static int activeProfile = 0;
      public static string ActiveProfileName => "P" + activeProfile + ".";
      private static List<ISavable> savables = new List<ISavable>();

      static SavableProfile() {
         SetActiveProfile(0);
      }

      public static void SetActiveProfile(int profileIndex) {
         activeProfile = profileIndex;
         for (var i = 0; i < savables.Count; i++) {
            savables[i].OnProfileSwitch();
         }
         SavableDisk.Reload();
      }

      internal static void Register(ISavable savable) {
         savables.Add(savable);
      }

      public interface ISavable {
         void OnProfileSwitch();
      }
   }
}