using System;

namespace Dribble.Savable {
   public class Savable<T> {
      protected readonly string Key;
      private T currentValue;
      private bool hasBeenFetched;
      protected Func<string, T> Parser;

      protected Savable(string key) {
         Key = key;
         SavableProfile.SubscribeToProfileChange(OnProfileSwitch);
      }

      public virtual T Value {
         get {
            if (hasBeenFetched) {
               return currentValue;
            }
            var storedString = SavableDisk.Get(Key);
            currentValue = Parser(storedString);
            hasBeenFetched = true;
            return currentValue;
         }
         set {
            currentValue = value;
            SavableDisk.Set(Key, Serialize(currentValue));
         }
      }

      public void OnProfileSwitch() {
         hasBeenFetched = false;
      }

      protected virtual string Serialize(T value) {
         return value.ToString();
      }

      public override string ToString() {
         return Value.ToString();
      }
   }

   public class SavableFloat : Savable<float> {
      public SavableFloat(string key, float defaultValue = 0) : base(key) {
         Parser = storedString => {
            float parsedValue;
            return float.TryParse(storedString, out parsedValue) ? parsedValue : defaultValue;
         };
      }

      public override string ToString() {
         return Value.ToString("0.0");
      }
   }

   public class SavableInt : Savable<int> {
      public SavableInt(string key, int defaultValue = 0) : base(key) {
         Parser = storedString => {
            int parsedValue;
            return int.TryParse(storedString, out parsedValue) ? parsedValue : defaultValue;
         };
      }

      public void Increment() {
         Value += 1;
      }
   }

   public class SavableBool : Savable<bool> {
      public SavableBool(string key, bool defaultValue = false) : base(key) {
         bool parsedValue;
         Parser = storedString => bool.TryParse(storedString, out parsedValue) ? parsedValue : defaultValue;
      }

      public void Toggle() {
         Value = !Value;
      }
   }

   public class SavableEnum<T> : Savable<T> {
      public SavableEnum(string key, T defaultValue) : base(key) {
         Parser = storedString => {
            try {
               return (T)Enum.Parse(typeof(T), storedString);
            }
            catch (Exception) {
               return defaultValue;
            }
         };
      }
   }

   public class SavableString : Savable<string> {
      public SavableString(string key, string defaultValue = "") : base(key) {
         Parser = storedString => storedString ?? defaultValue;
      }
   }
}
