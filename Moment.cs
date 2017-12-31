using UnityEngine;
using Dribble.Util;

namespace Dribble.Moment {
   public static class Moment {
      public static readonly Aggregator TimeScale = new Aggregator(1f, Refresh);

      private static void Refresh(float value) {
         Time.timeScale = Mathf.Min(100, value);
      }

      public static float DeltaTime {
         get { return Mathf.Min(Time.deltaTime, .1f * Time.timeScale); }
      }

      public static float Since(float time) {
         return Time.time - time;
      }

      public static float Until(float time) {
         return time - Time.time;
      }
   }
}