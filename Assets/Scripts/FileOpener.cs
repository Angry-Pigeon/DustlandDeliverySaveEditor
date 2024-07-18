using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileOpener : MonoBehaviour
{
    public Button openFileButton;
    public TextAsset loadedTextAsset;

    [field: SerializeField]
    public SaveEditor SaveEditor { get; private set; }

    [field: SerializeField]
    public TMP_InputField CharacterNameInput { get; private set; }

    [field: SerializeField]
    public TMP_Text Status { get; private set; }

    [field: SerializeField]
    public Button ClearInfectionButton { get; private set; }

    private string path;

    

    
    void Start()
    {
        if (openFileButton != null)
        {
            openFileButton.onClick.AddListener(OpenFileBrowser);
        }

        if (CharacterNameInput != null)
        {
            CharacterNameInput.onSubmit.AddListener(OnCharacterNameSubmit);
            CharacterNameInput.onDeselect.AddListener(OnCharacterNameSubmit);
        }
        
        if (SaveEditor == null)
        {
            SaveEditor = FindObjectOfType<SaveEditor>();
        }

        if (ClearInfectionButton != null)
        {
            ClearInfectionButton.onClick.AddListener(ClearInfectionAndCreateNewSaveFile);
        }

        if (PlayerPrefs.HasKey("CharacterName"))
        {
            CharacterNameInput.text = PlayerPrefs.GetString("CharacterName");
        }
    }
    private void ClearInfectionAndCreateNewSaveFile()
    {
        if (SaveEditor.InfectionCleaner == null)
        {
            Debug.LogError("InfectionCleaner is null. Load the character data first.");
            return;
        }

        SaveEditor.InfectionCleaner.ClearInfection();
        SaveEditor.UpdateInfectionStatusInSaveFileSelectedLocation(path);
    }


    private void OnCharacterNameSubmit(string arg0)
    {
        PlayerPrefs.SetString("CharacterName", arg0);
    }

    [Button]
    public void TestPath()
    {
        string localLowPath = Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile),
            "AppData", "LocalLow", "NeutronStar", "WastelandExpress"
        );

        Debug.Log(localLowPath);
    }

    void OpenFileBrowser()
    {
#if UNITY_STANDALONE_WIN
        string defaultPath = Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile),
            "AppData", "LocalLow", "NeutronStar", "WastelandExpress"
        );

        // Ensure the directory exists
        if (!Directory.Exists(defaultPath))
        {
            Debug.Log("Directory does not exist: " + defaultPath);
        }

        var extensions = new[] {
            new ExtensionFilter("JSON Files", "json")
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", defaultPath, extensions, false);
        if (paths.Length > 0)
        {
            Debug.Log("Selected file: " + paths[0]);
            LoadTextAsset(paths[0]);
            
            //Remove the file name from the path
            path = Path.GetDirectoryName(paths[0]);
        }
#endif
    }

    void LoadTextAsset(string filePath)
    {
        string fileContent = File.ReadAllText(filePath);
        loadedTextAsset = new TextAsset(fileContent);
        Debug.Log("Loaded TextAsset content: " + fileContent);
        
        SaveEditor.saveFileAsset = loadedTextAsset;
        
        if (SaveEditor.CheckIfFileReadable(fileContent, CharacterNameInput.text))
        {
            bool hasInfection = SaveEditor.InfectionCleaner.CharacterHasInfection;
            Status.text = "Character Found! \n Infection Status: " + (hasInfection ? "Infected" : "Clean");
        }else
        {
            Status.text = $"Character With Name \"{CharacterNameInput.text}\" Not Found!";
        }
    }
}
