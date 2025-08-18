using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    private Dictionary<string, string> localizedText;
    public string currentLanguage = "tr_TR";
    public bool isReady = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLocalizedText(currentLanguage);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLocalizedText(string languageCode)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Languages", languageCode, "short.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            localizedText = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataAsJson);
            currentLanguage = languageCode;
            isReady = true;
        }
        else
        {
            Debug.LogError("Dil dosyası bulunamadı: " + filePath);
        }
    }

    public string GetLocalizedValue(string key)
    {
        if (localizedText != null && localizedText.ContainsKey(key))
            return localizedText[key];
        else
            return "[[" + key + "]]"; // Eksik çeviri göstergesi
    }

    public string LoadLocalizedLongText(string relativePath) // örnek: "story/intro.txt"
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Languages", currentLanguage, relativePath);

        if (File.Exists(filePath))
            return File.ReadAllText(filePath, System.Text.Encoding.UTF8);
        else
            return "[[Missing: " + relativePath + "]]";
    }

    public List<string> GetAvailableLanguages()
    {
        List<string> langs = new List<string>();
        string basePath = Path.Combine(Application.streamingAssetsPath, "Languages");

        if (Directory.Exists(basePath))
        {
            foreach (var dir in Directory.GetDirectories(basePath))
            {
                langs.Add(Path.GetFileName(dir));
            }
        }

        return langs;
    }
}
