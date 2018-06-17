using System;

namespace Dribble {
   public class State<T> {
      private event Action<T> OnValueChange;
      public T value { get; private set; }

      public State(T value = default(T)) {
         this.value = value;
      }

      public void SetValue(T value) {
         if (value.Equals(this.value)) {
            return;
         }

         this.value = value;
         if (OnValueChange != null) OnValueChange(value);
      }

      public void SubscribeAndFire(Action<T> onChange) {
         Subscribe(onChange);
         onChange(value);
      }

      public void Subscribe(Action<T> onChange) {
         OnValueChange += onChange;
      }

      public void Unsubscribe(Action<T> onChange) {
         OnValueChange -= onChange;
      }
   }
}