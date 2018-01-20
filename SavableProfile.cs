using System.Collections.Generic;

namespace Dribble {
   public static class SavableProfile {
      public delegate void ProfileChangeCallback();

      public static int ActiveProfile { get; private set; }
      private static List<ProfileChangeCallback> savables = new List<ProfileChangeCallback>();

      static SavableProfile() {
         SetActiveProfile(0);
      }

      public static void SetActiveProfile(int profileIndex) {
         ActiveProfile = profileIndex;
         SavableDisk.Reload();
         for (var i = 0; i < savables.Count; i++) {
            savables[i]();
         }
      }

      public static void SubscribeToProfileChange(ProfileChangeCallback callback) {
         savables.Add(callback);
      }
   }
}