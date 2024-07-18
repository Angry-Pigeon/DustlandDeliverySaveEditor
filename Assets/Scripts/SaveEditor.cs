using UnityEngine;
using CharacterStats;
using Sirenix.OdinInspector;
using System.IO;
using Sirenix.Utilities;
using UnityEngine.Serialization;

public class SaveEditor : MonoBehaviour
{
    public TextAsset saveFileAsset;
    public string CharacterName;

    public CharacterStatusInfo CharacterStatusInfo;
    public InfectionCleaner InfectionCleaner;

    public bool CheckIfFileReadable(string text, string characterName)
    {
        if(text.IsNullOrWhitespace()) return false;
        if(characterName.IsNullOrWhitespace()) return false;
        this.CharacterName = characterName;
        
        string data = ExtractCharacterRelatedData(text, CharacterName);
        if (data == "No Data Found")
        {
            Debug.Log("Character data not found in the save file.");
            return false;
        }
        else
        {
            CharacterStatusInfo = CharacterStatusInfo.ExtractData(data);
            InfectionCleaner = new InfectionCleaner(data);
            return true;
        }
    }
    
    public void UpdateInfectionStatusInSaveFileSelectedLocation(string path1)
    {
        if (InfectionCleaner == null)
        {
            Debug.LogError("InfectionCleaner is null. Load the character data first.");
            return;
        }

        string realNameKey = "\"_realname\":";
        string keyToLookFor = realNameKey + "\"" + CharacterName + "\"";

        string saveData = saveFileAsset.text;
        
        int startIndex = saveData.IndexOf(keyToLookFor);
        
        // Replace the old data with the new data
        saveData = saveData.Remove(startIndex, ExtractCharacterRelatedData(saveFileAsset, CharacterName).Length);
        saveData = saveData.Insert(startIndex, InfectionCleaner.data);

        // Save the updated data back to the project directory
        string path = Path.Combine(path1, "UpdatedSaveFile.json");
        File.WriteAllText(path, saveData);

        Debug.Log("Save file updated at: " + path);
    }
    

    [Button]
    public void TEST_ClearInfection()
    {
        string data = ExtractCharacterRelatedData(saveFileAsset, CharacterName);
        InfectionCleaner = new InfectionCleaner(data);
    }

    [Button]
    public void TEST_FindSelectedUserByName()
    {
        string realNameKey = "\"_realname\":";
        string keyToLookFor = realNameKey + "\"" + CharacterName + "\"";

        Debug.Log(keyToLookFor); // This line is added to demonstrate the output

        string saveData = saveFileAsset.text;

        int startIndex = saveData.IndexOf(keyToLookFor);
        if (startIndex != -1)
        {
            int endIndex = saveData.IndexOf(realNameKey, startIndex + keyToLookFor.Length);
            if (endIndex == -1)
            {
                endIndex = saveData.Length; // If no more "_realname": found, set endIndex to the end of the text
            }

            string dataBetween = saveData.Substring(startIndex, endIndex - startIndex);
            Debug.Log("Data between realnames: " + dataBetween);

            // Extract data using the method
            CharacterStatusInfo characterInfo = CharacterStatusInfo.ExtractData(dataBetween);
            this.CharacterStatusInfo = characterInfo;
            Debug.Log("Character Real Name: " + characterInfo.RealName);
        }
        else
        {
            Debug.Log("Not found");
        }
    }

    [Button]
    public void UpdateCharacterDataInSaveFile()
    {
        if (CharacterStatusInfo == null)
        {
            Debug.LogError("CharacterStatusInfo is null. Load the character data first.");
            return;
        }

        string realNameKey = "\"_realname\":";
        string keyToLookFor = realNameKey + "\"" + CharacterName + "\"";

        string saveData = saveFileAsset.text;
        
        int startIndex = saveData.IndexOf(keyToLookFor);
        int endIndex = startIndex + CharacterStatusInfo.lengthOfDataEnd;
        
        // Replace the old data with the new data
        saveData = saveData.Remove(startIndex, endIndex - startIndex);
        saveData = saveData.Insert(startIndex, CharacterStatusInfo.ToString());

        // Save the updated data back to the project directory
        string path = Path.Combine(Application.dataPath, "UpdatedSaveFile.json");
        File.WriteAllText(path, saveData);

        Debug.Log("Save file updated at: " + path);
    }
    
    [Button]
    public void UpdateInfectionStatusInSaveFile()
    {
        if (InfectionCleaner == null)
        {
            Debug.LogError("InfectionCleaner is null. Load the character data first.");
            return;
        }

        string realNameKey = "\"_realname\":";
        string keyToLookFor = realNameKey + "\"" + CharacterName + "\"";

        string saveData = saveFileAsset.text;
        
        int startIndex = saveData.IndexOf(keyToLookFor);
        
        // Replace the old data with the new data
        saveData = saveData.Remove(startIndex, ExtractCharacterRelatedData(saveFileAsset, CharacterName).Length);
        saveData = saveData.Insert(startIndex, InfectionCleaner.data);

        // Save the updated data back to the project directory
        string path = Path.Combine(Application.dataPath, "UpdatedSaveFile.json");
        File.WriteAllText(path, saveData);

        Debug.Log("Save file updated at: " + path);
    }

    public static string ExtractCharacterRelatedData(TextAsset saveFile, string CharacterName)
    {
        string realNameKey = "\"_realname\":";
        string keyToLookFor = realNameKey + "\"" + CharacterName + "\"";

        Debug.Log(keyToLookFor); // This line is added to demonstrate the output

        string saveData = saveFile.text;

        int startIndex = saveData.IndexOf(keyToLookFor);
        if (startIndex != -1)
        {
            int endIndex = saveData.IndexOf(realNameKey, startIndex + keyToLookFor.Length);
            if (endIndex == -1)
            {
                endIndex = saveData.Length; // If no more "_realname": found, set endIndex to the end of the text
            }

            string dataBetween = saveData.Substring(startIndex, endIndex - startIndex);
            Debug.Log("Data between realnames: " + dataBetween);
            return dataBetween;
        }
        else
        {
            Debug.Log("Not found");
            return "No Data Found";
        }
    }
    
    public static string ExtractCharacterRelatedData(string saveFile, string CharacterName)
    {
        string realNameKey = "\"_realname\":";
        string keyToLookFor = realNameKey + "\"" + CharacterName + "\"";

        Debug.Log(keyToLookFor); 

        string saveData = saveFile;

        int startIndex = saveData.IndexOf(keyToLookFor);
        if (startIndex != -1)
        {
            int endIndex = saveData.IndexOf(realNameKey, startIndex + keyToLookFor.Length);
            if (endIndex == -1)
            {
                endIndex = saveData.Length; 
            }

            string dataBetween = saveData.Substring(startIndex, endIndex - startIndex);
            Debug.Log("Data between realnames: " + dataBetween);
            return dataBetween;
        }
        else
        {
            Debug.Log("Not found");
            return "No Data Found";
        }
    }
}