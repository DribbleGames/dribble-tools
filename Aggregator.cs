using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dribble.Util {
   public class Aggregator {
      public class Token {
         private readonly Aggregator aggregator;
         private float lastValue = 1;
         private float actualValue = 1;
         private bool hasBeenSet;

         public Token(Aggregator aggregator) {
            this.aggregator = aggregator;
         }

         public float CurrentValue() {
            return lastValue;
         }

         public void Set(float value) {
            if (Mathf.Abs(value - actualValue) < .000001f) {
               lastValue = value;
               return;
            }
            if (hasBeenSet) {
               aggregator.RemoveMultiplier(actualValue);
            }
            aggregator.AddMultiplier(value);
            lastValue = value;
            actualValue = value;
            hasBeenSet = true;
         }

         public void Reset() {
            Set(1);
         }
      }

      public readonly float Default;
      private readonly List<float> multipliers = new List<float>();

      private bool hasCalculated;
      private float calculatedValue;

      public delegate void OnChangeHandler(float value);
      private event OnChangeHandler OnChangeEvent;

      public Aggregator(float defaultValue, OnChangeHandler handler = null) {
         Default = defaultValue;
         calculatedValue = Default;
         if (handler != null) {
            OnChangeEvent += handler;
         }
      }

      private void ExecuteCallback() {
         if (OnChangeEvent != null) {
            Update();
            OnChangeEvent(calculatedValue);
         }
      }

      public void AddMultiplier(float multiplier) {
         multipliers.Add(multiplier);
         hasCalculated = false;
         ExecuteCallback();
      }

      public void RemoveMultiplier(float multiplier) {
         multipliers.Remove(multiplier);
         hasCalculated = false;
         ExecuteCallback();
      }

      private void Update() {
         calculatedValue = Default;
         for (var i = 0; i < multipliers.Count; i++) {
            calculatedValue *= multipliers[i];
         }
         hasCalculated = true;
      }

      public float Get() {
         if (!hasCalculated) {
            Update();
         }
         return calculatedValue;
      }

      public Token GenerateToken() {
         return new Token(this);
      }
   }
}