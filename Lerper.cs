using UnityEngine;

namespace Dribble.Util {
   public class Lerper {
      private float desiredValue;
      public float Value { get; private set; }
      private readonly float rate;

      public Lerper(float initialValue, float transitionRate) {
         rate = transitionRate;
         Value = initialValue;
         desiredValue = Value;
      }

      public void Set(float value) {
         desiredValue = value;
      }

      public void Update() {
         Value = Mathf.Lerp(Value, desiredValue, rate * Moment.Moment.DeltaTime);
      }
   }
}