using System.Collections.Generic;

namespace Dribble.Savable {
   public static class SavableProfile {
      public static int ActiveProfile { get; private set; }
      private static List<ISavable> savables = new List<ISavable>();

      static SavableProfile() {
         SetActiveProfile(0);
      }

      public static void SetActiveProfile(int profileIndex) {
         ActiveProfile = profileIndex;
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