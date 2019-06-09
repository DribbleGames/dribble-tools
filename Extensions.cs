using Dribble.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Dribble {
   public static class EnumerableExtensions {
      public static int IndexOf<T>(this IEnumerable<T> list, T target) {
         var i = 0;
         foreach (var item in list) {
            if (Equals(item, target)) {
               return i;
            }
            i++;
         }
         return -1;
      }

      public static void Apply<T>(this IEnumerable<T> list, Action<T, int> lambda) {
         var i = 0;
         foreach (var item in list) {
            lambda(item, i++);
         }
      }

      public static void Apply<T>(this IEnumerable<T> list, Action<T> lambda) {
         foreach (var item in list) {
            lambda(item);
         }
      }

      public static HashSet<T> ToHashSet<T>(this IEnumerable<T> list) {
         return new HashSet<T>(list);
      }

      public static IEnumerable<T> ToEnumerable<T>(this T item) {
         return Enumerable.Repeat(item, 1);
      }

   }

   public static class ListExtension {
      public static T RemoveRandom<T>(this IList<T> list) {
         var index = Random.Range(0, list.Count);
         var item = list[index];
         list.RemoveAt(index);
         return item;
      }

      public static IList<T> RemoveWhere<T>(this IList<T> list, Func<T, bool> predicate) {
         for (var i = list.Count - 1; i >= 0; i--) {
            if (predicate(list[i])) {
               list.RemoveAt(i);
            }
         }
         return list;
      }

      public static T ChooseRandom<T>(this IList<T> list) {
         var index = Random.Range(0, list.Count);
         return list[index];
      }

      public static void Apply<T>(this IList<T> list, Action<T> lambda) {
         for (var i = 0; i < list.Count; i++) {
            lambda(list[i]);
         }
      }

      public static void Apply<T>(this IList<T> list, Action<T, int> lambda) {
         for (var i = 0; i < list.Count; i++) {
            lambda(list[i], i);
         }
      }

      public static IEnumerable<T> Except<T>(this IEnumerable<T> list, T other) {
         var otherAsEnumerable = Enumerable.Repeat(other, 1);
         return list.Except(otherAsEnumerable);
      }

      public static IEnumerable<T> Concat<T>(this IEnumerable<T> list, T other) {
         var otherAsEnumerable = Enumerable.Repeat(other, 1);
         return list.Concat(otherAsEnumerable);
      }

      public static int FirstIndex<T>(this IEnumerable<T> list, Func<T, bool> predicate) {
         var index = 0;
         foreach (var item in list) {
            if (predicate(item)) {
               return index;
            }
            index++;
         }
         return -1;
      }

      public static IEnumerable<Pair<T>> SelfJoin<T>(this List<T> list) {
         for (var i = 0; i < list.Count; i++) {
            for (var j = i + 1; j < list.Count; j++) {
               yield return new Pair<T>(list[i], list[j]);
            }
         }
      }

      public static IEnumerable<Pair<T>> ToPairs<T>(this List<T> list) {
         for (var i = 0; i < list.Count; i += 2) {
            yield return new Pair<T>(list[i], list[i + 1]);
         }
      }

      public static TSource MinBy<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector) {
         if (!source.Any()) {
            return default(TSource);
         }
         var minValue = float.MaxValue;
         var minElement = source.First();
         foreach (var candidate in source) {
            var candidateValue = selector(candidate);
            if (candidateValue < minValue) {
               minValue = candidateValue;
               minElement = candidate;
            }
         }
         return minElement;
      }

      public static IList<int> IndicesWhere<T>(this IList<T> list, Func<T, bool> predicate) {
         var output = new List<int>();
         for (var i = 0; i < list.Count; i++) {
            if (predicate(list[i])) {
               output.Add(i);
            }
         }
         return output;
      }
   }

   public static class ParticlesExtension {
      public static void EnableEmission(this ParticleSystem system, bool enabled = true) {
         var em = system.emission;
         em.enabled = enabled;
      }
      public static void DisableEmission(this ParticleSystem system) {
         EnableEmission(system, false);
      }
      public static void SetStartColor(this ParticleSystem system, Color color) {
         var mainModule = system.main;
         mainModule.startColor = new ParticleSystem.MinMaxGradient(color);
      }
   }

   public static class RendererExtensions {
      public static Color GetTint(this Component component) {
         return component.GetComponent<Renderer>().material.GetColor("_TintColor");
      }

      public static void SetTint(this Component component, Color color) {
         component.GetComponent<Renderer>().material.SetColor("_TintColor", color);
      }

      public static void SetEmissionColor(this Component component, Color color) {
         component.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
      }

      public static void LerpColor(this Component component, Color target, float amount) {
         var current = component.GetComponent<Renderer>().material.color;
         component.GetComponent<Renderer>().material.color = Color.Lerp(current, target, amount);
      }
   }

   public static class VectorExtension {
      public static Vector3 ToVector3(this Vector2 input) {
         return new Vector3(input.x, input.y, 0);
      }

      public static Vector2 ToVector2(this Vector3 input) {
         return new Vector2(input.x, input.y);
      }

      public static Vector3 DeZero(this Vector3 input) {
         var x = Math.Abs(input.x) < .001f ? .001f : input.x;
         var y = Math.Abs(input.y) < .001f ? .001f : input.y;
         var z = Math.Abs(input.z) < .001f ? .001f : input.z;
         return new Vector3(x, y, z);
      }

      public static Vector3 WithX(this Vector3 input, float x) {
         input.x = x;
         return input;
      }

      public static Vector3 WithY(this Vector3 input, float y) {
         input.y = y;
         return input;
      }

      public static Vector3 WithZ(this Vector3 input, float z) {
         input.z = z;
         return input;
      }

      public static Vector2 WithX(this Vector2 input, float x) {
         input.x = x;
         return input;
      }

      public static Vector2 WithY(this Vector2 input, float y) {
         input.y = y;
         return input;
      }
   }

   public static class BoolExtensions {
      public static int Sum(params bool[] list) {
         return list.Aggregate(0, ((current, b) => current + (b ? 1 : 0)));
      }
   }

   public struct TransformState {
      public TransformState(Vector3 position, Quaternion rotation) {
         Position = position;
         Rotation = rotation;
         IsLocal = false;
      }

      public TransformState(Transform transform, bool local) {
         if (local) {
            Position = transform.localPosition;
            Rotation = transform.localRotation;
         }
         else {
            Position = transform.position;
            Rotation = transform.rotation;
         }
         IsLocal = local;
      }

      public void Restore(Transform transform) {
         if (IsLocal) {
            transform.localPosition = Position;
            transform.localRotation = Rotation;
         }
         else {
            transform.position = Position;
            transform.rotation = Rotation;
         }
      }

      public void LerpTransform(Transform transform, float speed) {
         var initialPosition = IsLocal ? transform.localPosition : transform.position;
         var initialRotation = IsLocal ? transform.localRotation : transform.rotation;
         var transitionPosition = Vector3.Lerp(initialPosition, Position, speed * Moment.DeltaTime);
         var transitionRotation = Quaternion.Lerp(initialRotation, Rotation, speed * Moment.DeltaTime);
         if (IsLocal) {
            transform.localPosition = transitionPosition;
            transform.localRotation = transitionRotation;
         }
         else {
            transform.position = transitionPosition;
            transform.rotation = transitionRotation;
         }
      }

      public readonly Vector3 Position;
      public readonly Quaternion Rotation;
      public bool IsLocal;
   }

   public static class TransformExtensions {
      public static void LookToward(this Transform transform, Transform target, float speed) {
         LookToward(transform, target.position, speed);
      }

      public static void LookToward(this Transform transform, Vector3 target, float speed) {
         var currentRotation = transform.rotation;
         transform.LookAt(target);
         transform.rotation = Quaternion.Lerp(currentRotation, transform.rotation, speed * Moment.DeltaTime);
      }

      public static bool MoveToward(this Transform transform, Vector3 desiredPosition, float speed, float distanceTolerance = float.NaN) {
         var difference = (desiredPosition - transform.position);
         var distance = difference.magnitude;

         if (float.IsNaN(distanceTolerance)) {
            distanceTolerance = speed * Moment.DeltaTime;
         }

         if (distance < distanceTolerance) {
            return true;
         }

         transform.position += difference.normalized * speed * Moment.DeltaTime;
         return false;
      }

      public static bool SkidToward(this Transform transform, Vector3 target, ref Vector3 velocity, float speed, float skidFactor, float tolerance) {
         var delta = (target - transform.position).normalized * speed;
         velocity = Vector3.Lerp(velocity, delta, skidFactor * Moment.DeltaTime);
         if (velocity.magnitude > speed) {
            velocity = velocity.normalized * speed;
         }
         transform.position += velocity * Moment.DeltaTime;
         return Vector3.Distance(transform.position, target) < tolerance;
      }

      public static bool LerpToward(this Transform transform, Vector3 desiredPosition, float speed, float distanceTolerance = float.NaN) {
         var distance = Vector3.Distance(transform.position, desiredPosition);
         if (!float.IsNaN(distanceTolerance) && distance < distanceTolerance) {
            return true;
         }

         transform.position = Vector3.Lerp(transform.position, desiredPosition, speed * Moment.DeltaTime);
         return false;
      }

      public static bool LerpTowardLocal(this Transform transform, Vector3 desiredPosition, float speed, float distanceTolerance = float.NaN) {
         var distance = Vector3.Distance(transform.localPosition, desiredPosition);
         if (!float.IsNaN(distanceTolerance) && distance < distanceTolerance) {
            return true;
         }

         transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPosition, speed * Moment.DeltaTime);
         return false;
      }

      public const float DefaultArc = .3f;
      public static bool ArcToward(this Transform transform, Vector3 targetPosition, float speed) {
         var positionDelta = targetPosition - transform.position;
         var distanceToTravel = speed * Moment.DeltaTime;
         if (positionDelta.magnitude < distanceToTravel) {
            return true;
         }

         var perpendicularDelta = (targetPosition.x < transform.position.x)
            ? new Vector3(positionDelta.y, -positionDelta.x)
            : new Vector3(-positionDelta.y, positionDelta.x);


         var adjustedDelta = Vector3.Lerp(positionDelta, perpendicularDelta, DefaultArc);
         transform.position += adjustedDelta.normalized * distanceToTravel;
         return false;
      }

      public static TransformState GetState(this Transform transform, bool local = false) {
         return new TransformState(transform, local);
      }
   }

   public static class MonoBehaviourExtensions {
      public static float DistanceTo(this MonoBehaviour obj, Vector3 point) {
         return Vector3.Distance(obj.transform.position, point);
      }

      public static float DistanceTo(this MonoBehaviour obj, Vector2 point) {
         return Vector2.Distance(obj.transform.position, point);
      }

      public static float X(this MonoBehaviour obj) {
         return obj.transform.position.x;
      }

      public static float Y(this MonoBehaviour obj) {
         return obj.transform.position.y;
      }

      public static float Top(this Transform transform) {
         return transform.position.y + transform.localScale.y;
      }

      public static void ClearAndDestroy<T>(this ICollection<T> list) where T : MonoBehaviour {
         foreach (var item in list) {
            Object.Destroy(item.gameObject);
         }
         list.Clear();
      }

      // Note: Uses reflection. Don't call every frame.
      public static T CopyComponentOnto<T>(this T original, GameObject destObject) where T : Component {
         var destination = destObject.AddComponent<T>();
         original.CopyComponentValuesOnto(destination);
         return destination;
      }

      // Note: Uses reflection. Don't call every frame.
      public static void CopyComponentValuesOnto<T>(this T original, Component destination) where T : Component {
         var type = original.GetType();
         var fields = type.GetFields();
         foreach (var field in fields) {
            field.SetValue(destination, field.GetValue(original));
         }
         var props = type.GetProperties();
         foreach (var prop in props) {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name" || prop.Name == "parent")
               continue;
            prop.SetValue(destination, prop.GetValue(original, null), null);
         }
      }
   }

   public static class FloatExtensionss {
      public static bool IsPositive(this float a) {
         return !float.IsNaN(a) && a > 0;
      }

      public static bool IsSameSignAs(this float a, float b) {
         return (a > 0 && b > 0) || (a < 0 && b < 0);
      }

      public static float DividedBy(this float numerator, float denominator) {
         if (Math.Abs(denominator) < .001f) {
            denominator = .001f;
         }
         return numerator / denominator;
      }

      public static float ReplaceNaN(this float value, float replacement) {
         return float.IsNaN(value) ? replacement : value;
      }

      public static float DistanceTo(this float value, float other) {
         return Mathf.Abs(value - other);
      }

      public static bool SecondsHaveElapsedSince(this float duration, float initialTime) {
         return Time.time - initialTime > duration;
      }

      public static float RemapInRange(this float value, float newFrom, float newTo, float originalFrom = 0, float originalTo = 1) {
         var progress = (value - originalFrom) / (originalTo - originalFrom);
         return newFrom + (newTo - newFrom) * progress;
      }

      public static float Oscillate(this float period, float from = 0, float to = 1, float offset = 0) {
         var zeroToOne = (Time.time + offset) % period / period;
         var zeroToTwo = zeroToOne * 2;
         var zeroToOneToZero = zeroToOne < .5f ? zeroToTwo : 2 - zeroToTwo;
         return from + zeroToOneToZero * (to - from);
      }

      public static float Clamp(this float value, float min = 0, float max = 1) {
         return Mathf.Clamp(value, min, max);
      }

      public static float MoveToward(this float position, float desiredPosition, float speed, float distanceTolerance = float.NaN) {
         var difference = (desiredPosition - position);
         var distance = Mathf.Abs(difference);

         if (float.IsNaN(distanceTolerance)) {
            distanceTolerance = speed * Moment.DeltaTime;
         }

         if (distance < distanceTolerance) {
            return position;
         }

         return position + difference * speed * Moment.DeltaTime;
      }
   }

   public static class IntExtensions {
      public static void Repeat(this int count, Action<int> action) {
         for (var i = 0; i < count; i++) {
            action(i);
         }
      }
      public static int Count(this int count, Func<int, bool> action) {
         var n = 0;
         for (var i = 0; i < count; i++) {
            if (action(i)) {
               n++;
            }
         }
         return n;
      }
      public static IEnumerable<T> Select<T>(this int count, Func<int, T> func) {
         for (var i = 0; i < count; i++) {
            yield return func(i);
         }
      }
   }

   public struct Pair<T> {
      public T A;
      public T B;

      public Pair(T a, T b) {
         A = a;
         B = b;
      }

      public T this[int index] {
         get { return index == 0 ? A : B; }
      }
   }


   public static class EnumExtensions {
      public static IEnumerable<T> Values<T>() {
         var levels = Enum.GetValues(typeof(T));
         return levels.Cast<T>();
      }

      public static T Parse<T>(string value) {
         return (T)Enum.Parse(typeof(T), value);
      }
   }


   public static class ColorExtensions {
      public static float Luminance(this Color color) {
         return .3f * color.r + .59f * color.g + .11f * color.b;
      }

      public static float DistanceTo(this Color color, Color otherColor) {
         return Mathf.Abs(color.r - otherColor.r) +
               Mathf.Abs(color.g - otherColor.g) +
               Mathf.Abs(color.b - otherColor.b);
      }

      public static Color Lighten(this Color color, float amount) {
         return Color.Lerp(color, Color.white, amount);
      }

      public static Color Darken(this Color color, float amount) {
         return Color.Lerp(color, Color.black, amount);
      }

      public static Color Average(this List<Color> colors) {
         var output = colors.Aggregate(new float[3], (floats, color) => {
            floats[0] += color.r / colors.Count;
            floats[1] += color.g / colors.Count;
            floats[2] += color.b / colors.Count;
            return floats;
         });
         return new Color(output[0], output[1], output[2]);
      }

      public static Color Closest(this Color color, List<Color> otherColors) {
         float dummy;
         return Closest(color, otherColors, out dummy);
      }

      public static Color Closest(this Color color, List<Color> otherColors, out float closestDistance) {
         closestDistance = Mathf.Infinity;
         var closest = color;
         for (var i = 0; i < otherColors.Count; i++) {
            var otherColor = otherColors[i];
            var distance = color.DistanceTo(otherColor);
            if (distance < closestDistance) {
               closestDistance = distance;
               closest = otherColor;
            }
         }
         return closest;
      }

      public static Color GetCloneWithAlpha(this Color color, float newAlpha) {
         return new Color(color.r, color.g, color.b, newAlpha);
      }

      public static Color GetCloneClamped(this Color color, float min = Mathf.NegativeInfinity, float max = Mathf.Infinity) {
         color.r = Mathf.Clamp(color.r, min, max);
         color.g = Mathf.Clamp(color.g, min, max);
         color.b = Mathf.Clamp(color.b, min, max);
         return color;
      }

      public static Color GetCloneWithSaturation(this Color color, float newValue) {
         float h, s, v;
         Color.RGBToHSV(color, out h, out s, out v);
         var updatedColor = Color.HSVToRGB(h, newValue, v);
         updatedColor.a = color.a;
         return updatedColor;
      }

      public static Color GetCloneWithValue(this Color color, float newValue) {
         float h, s, v;
         Color.RGBToHSV(color, out h, out s, out v);
         var updatedColor = Color.HSVToRGB(h, s, newValue);
         updatedColor.a = color.a;
         return updatedColor;
      }

      public static Color GetCloneWithRotatedHue(this Color color, float rotation) {
         float h, s, v;
         Color.RGBToHSV(color, out h, out s, out v);
         h += rotation;
         h %= 1f;
         var updatedColor = Color.HSVToRGB(h, s, v);
         updatedColor.a = color.a;
         return updatedColor;
      }

      public static Vector3 ToVector3RGB(this Color color) {
         return new Vector3(color.r, color.g, color.b);
      }

      public static Color ToColor(this Vector3 vector) {
         return new Color(vector.x, vector.y, vector.z);
      }
   }

   public static class GradientExtensions {
      public enum GradientStyle {
         FadeInOut = 0,
      }

      public static readonly GradientAlphaKey[] FadeInThenOut =
         { new GradientAlphaKey(0, 0), new GradientAlphaKey(1f, .5f), new GradientAlphaKey(0, 1) };

      public static Gradient EvenlySpacedColors(IList<Color> colors, GradientAlphaKey[] alphaKeys) {

         var colorKeys = new List<GradientColorKey>();
         for (var i = 0; i < colors.Count; i++) {
            var time = 1f * i / (colors.Count - 1);
            colorKeys.Add(new GradientColorKey(colors[i], time));
         }

         return new Gradient() {
            colorKeys = colorKeys.ToArray(),
            alphaKeys = alphaKeys
         };
      }
   }

   public static class ComponentExtensions {
      public static T[] GetComponentsInChildrenExcludeSelf<T>(this Component component) {
         var childrenAndSelf = component.GetComponentsInChildren<T>();
         if (childrenAndSelf.Length == 0) {
            return childrenAndSelf;
         }
         var hasT = component.GetComponent<T>();
         if (hasT == null) {
            return childrenAndSelf;
         }

         var list = childrenAndSelf.ToList();
         list.RemoveAt(0);
         return list.ToArray();
      }
   }
}