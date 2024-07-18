using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace CharacterStats
{
    [Serializable]
    public class InfectionCleaner
    {
        private const string InfectionKey = "\"85\":{\"ID\":85,\"Buffid\":7086}";
        public bool CharacterHasInfection;
        
        [HideInInspector]
        public string data;
        
        public InfectionCleaner(string data)
        {
            this.data = data;
            CharacterHasInfection = data.Contains(InfectionKey);
            
        }

        [Button]
        public void ClearInfection()
        {
            if (!data.Contains(InfectionKey)) return;
            int startIndex = data.IndexOf(InfectionKey);
            int length = InfectionKey.Length;
            
            if (startIndex != -1)
            {
                Debug.Log(this.data);

                string dataBetween = data.Remove(startIndex, length + 1);
                data = dataBetween;
                CharacterHasInfection = false;
                this.data = dataBetween;
                
                Debug.Log(this.data);
            }
            
        }
    }
}