using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CharacterStats
{
    [Serializable]
    public class CharacterStatusInfo
    {
        public string RealName;
        public int Rarity;
        public List<string> Taste;
        public int Hungry;
        public HungryTime HungryTime;
        public int Thirst;
        public ThirstTime ThirstTime;
        public int Appetite;
        public int Fatigue;
        public int PeopleState;
        public int PeopleDrunk;
        public HurtTime HurtTime;
        public CalmTime CalmTime;
        public int Identity;
        public Experience Experience;

        [HideInInspector]
        public int indexOfDatEnd;
        [HideInInspector]
        public int lengthOfDataEnd;
        
        public static CharacterStatusInfo ExtractData(string data)
        {
            CharacterStatusInfo characterInfo = new CharacterStatusInfo
            {
                RealName = ExtractStringValue(data, "\"_realname\":\"", "\""),
                Rarity = ExtractIntValue(data, "\"Rarity\":", ","),
                Taste = new List<string> { ExtractStringValue(data, "\"Taste\":\"", "\"") },
                Hungry = ExtractIntValue(data, "\"Hungry\":", ","),
                HungryTime = ExtractTime<HungryTime>(data, "\"HungryTime\":"),
                Thirst = ExtractIntValue(data, "\"Thirsty\":", ","),
                ThirstTime = ExtractTime<ThirstTime>(data, "\"ThirstyTime\":"),
                Appetite = ExtractIntValue(data, "\"Appetite\":", ","),
                Fatigue = ExtractIntValue(data, "\"Fatigue\":", ","),
                PeopleState = ExtractIntValue(data, "\"PeopleState\":", ","),
                PeopleDrunk = ExtractIntValue(data, "\"PeopleDrunk\":", ","),
                HurtTime = ExtractTime<HurtTime>(data, "\"HurtTime\":"),
                CalmTime = ExtractTime<CalmTime>(data, "\"CalmTime\":"),
                Identity = ExtractIntValue(data, "\"Identity\":", ","),
                Experience = ExtractExperience(data)
            };
            
            characterInfo.indexOfDatEnd = data.IndexOf("\"ArrivedCity\"");
            string dataBetween = data.Substring(0, characterInfo.indexOfDatEnd);
            characterInfo.lengthOfDataEnd = dataBetween.Length;
            Debug.Log("Character Status Data: " + dataBetween);
            
            
            

            return characterInfo;
        }

        private static string ExtractStringValue(string data, string key, string endChar)
        {
            int startIndex = data.IndexOf(key) + key.Length;
            int endIndex = data.IndexOf(endChar, startIndex);
            return data.Substring(startIndex, endIndex - startIndex);
        }

        private static int ExtractIntValue(string data, string key, string endChar)
        {
            int startIndex = data.IndexOf(key) + key.Length;
            int endIndex = data.IndexOf(endChar, startIndex);
            if (endIndex == -1)
            {
                endIndex = data.Length;
            }
            string valueStr = data.Substring(startIndex, endIndex - startIndex).Trim();
            return int.Parse(valueStr);
        }

        private static T ExtractTime<T>(string data, string key) where T : new()
        {
            T time = new T();
            string timeData = ExtractStringValue(data, key + "{", "}");
            var fields = time.GetType().GetFields();

            foreach (var field in fields)
            {
                var fieldType = field.FieldType;
                if (fieldType == typeof(int))
                {
                    field.SetValue(time, ExtractIntValue(timeData, "\"" + field.Name + "\":", ","));
                }
            }

            return time;
        }

        private static Experience ExtractExperience(string data)
        {
            Experience experience = new Experience();
            string expData = ExtractStringValue(data, "\"Experience\":{", "\"ArrivedCity\"");
            experience.CurrentExperience = ExtractIntFromData(ref expData, "\"CurrentExp\":");
            experience.SpecialPoint = ExtractIntFromData(ref expData, "\"SpecialPoint\":");
            experience.Level = ExtractIntFromData(ref expData, "\"level\":");
            return experience;
        }

        private static int ExtractIntFromData(ref string data, string key)
        {
            int startIndex = data.IndexOf(key) + key.Length;
            int endIndex = data.IndexOf(",", startIndex);
            if (endIndex == -1)
            {
                endIndex = data.Length;
            }

            string valueStr = data.Substring(startIndex, endIndex - startIndex).Trim();
            data = data.Substring(endIndex + 1);

            return int.Parse(valueStr);
        }

        public override string ToString()
        {
            string data = "";
            data += ($"\"_realname\":\"{RealName}\",");
            data += ($"\"Rarity\":{Rarity},");
            data += ($"\"Taste\":\"{Taste[0]}\",");
            data += ($"\"Hungry\":{Hungry},");
            data += ($"\"HungryTime\":{{\"Days\":{HungryTime.Days},\"Hours\":{HungryTime.Hours},\"Minutes\":{HungryTime.Minutes},\"TotalMinutes\":{HungryTime.TotalMinutes}}},");
            data += ($"\"Thirsty\":{Thirst},");
            data += ($"\"ThirstyTime\":{{\"Days\":{ThirstTime.Days},\"Hours\":{ThirstTime.Hours},\"Minutes\":{ThirstTime.Minutes},\"TotalMinutes\":{ThirstTime.TotalMinutes}}},");
            data += ($"\"Appetite\":{Appetite},");
            data += ($"\"Fatigue\":{Fatigue},");
            data += ($"\"PeopleState\":{PeopleState},");
            data += ($"\"PeopleDrunk\":{PeopleDrunk},");
            data += ($"\"HurtTime\":{{\"Days\":{HurtTime.Days},\"Hours\":{HurtTime.Hours},\"Minutes\":{HurtTime.Minutes},\"TotalMinutes\":{HurtTime.TotalMinutes}}},");
            data += ($"\"CalmTime\":{{\"Days\":{CalmTime.Days},\"Hours\":{CalmTime.Hours},\"Minutes\":{CalmTime.Minutes},\"TotalMinutes\":{CalmTime.TotalMinutes}}},");
            data += ($"\"Identity\":{Identity},");
            data += ($"\"Experience\":{{\"CurrentExp\":{Experience.CurrentExperience},\"SpecialPoint\":{Experience.SpecialPoint},\"level\":{Experience.Level},");
            // Ensure we correctly format and close the JSON string
            return data;
        }
    }

    [Serializable]
    public class HungryTime
    {
        public int Days;
        public int Hours;
        public int Minutes;
        public int TotalMinutes;
    }

    [Serializable]
    public class ThirstTime
    {
        public int Days;
        public int Hours;
        public int Minutes;
        public int TotalMinutes;
    }

    [Serializable]
    public class HurtTime
    {
        public int Days;
        public int Hours;
        public int Minutes;
        public int TotalMinutes;
    }

    [Serializable]
    public class CalmTime
    {
        public int Days;
        public int Hours;
        public int Minutes;
        public int TotalMinutes;
    }

    [Serializable]
    public class Experience
    {
        public int CurrentExperience;
        public int SpecialPoint;
        public int Level;
    }
}
