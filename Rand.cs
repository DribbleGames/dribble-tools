using UnityEngine;

namespace Dribble.Util {
   public class Rand {
      private static Rand currentRand;
      public static Rand Static = new Rand(null);

      private Random.State internalState;

      public Rand(int? seed) {
         SaveCurrent();
         currentRand = this;
         if (seed != null) {
            Random.InitState(seed.Value);
         }
      }

      private static void SaveCurrent() {
         if (currentRand != null) {
            currentRand.internalState = Random.state;
         }
      }

      private void Activate() {
         if (currentRand != this) {
            SaveCurrent();
            currentRand = this;
            Random.state = internalState;
         }
      }

      public int Range(int min, int max) {
         Activate();
         return Random.Range(min, max);
      }

      public float Range(float min, float max) {
         Activate();
         return Random.Range(min, max);
      }

      public float Get() {
         Activate();
         return Random.value;
      }
   }

}