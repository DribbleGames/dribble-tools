using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

namespace Dribble {

   public class SavableDisk : MonoBehaviour {
      private static Dictionary<string, string> dict = new Dictionary<string, string>();
      private const string Delimiter = "::";
      private const string PairsDelimiter = "``";
      private const float TimeBetweenSaves = 5f;

      private static float lastUpdateTime;
      private static float lastSaveTime;

      public void Update() {
         if (lastUpdateTime > lastSaveTime && Time.time - lastSaveTime > TimeBetweenSaves) {
            lastSaveTime = Time.time;
            Write();
         }
      }

      internal static string Get(string key) {
         string value;
         return dict.TryGetValue(key, out value) ? value : "";
      }

      internal static void Set(string key, string value) {
         lastUpdateTime = Time.time;
         dict[key] = value;
      }

      internal static void Reload() {
         dict.Clear();

         var localString = PlayerPrefs.GetString("P" + SavableProfile.ActiveProfile);
         Read(localString);
      }

      private static void Read(string contents) {
         var keyValuePairs = contents.Split(new[] { PairsDelimiter }, StringSplitOptions.None);
         foreach (var kvpString in keyValuePairs) {
            var keyAndValue = kvpString.Split(new[] { Delimiter }, StringSplitOptions.None);
            if (keyAndValue.Length > 1) {
               var key = keyAndValue[0];
               var value = keyAndValue[1];
               dict[key] = value;
            }
         }
      }

      public static void Write() {
         var builder = new StringBuilder();
         foreach (var kvp in dict) {
            builder.Append(kvp.Key);
            builder.Append(Delimiter);
            builder.Append(kvp.Value);
            builder.Append(PairsDelimiter);
         }
         var finalValue = builder.ToString();
         PlayerPrefs.SetString("P" + SavableProfile.ActiveProfile, finalValue);
      }

      public static void ClearEverything() {
         PlayerPrefs.SetString("P" + SavableProfile.ActiveProfile, "");
         SavableProfile.SetActiveProfile(0);
      }
   }
}