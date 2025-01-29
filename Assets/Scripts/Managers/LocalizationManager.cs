using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    public string defaultLanguage = "en";
    public string currentLanguage { get; private set; }
    private Dictionary<string, Dictionary<string, string>> localizedText;

    public event Action OnLanguageChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLocalization();
            currentLanguage = defaultLanguage;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadLocalization()
    {
        localizedText = new Dictionary<string, Dictionary<string, string>>();
        string filePath = Path.Combine(Application.streamingAssetsPath, "localization.csv");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length <= 1)
            {
                Debug.LogError("Localization file is empty or missing headers: " + filePath);
                return;
            }
            string[] languages = lines[0].Split(',');
            
            var csvSplit = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))", RegexOptions.Compiled);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = csvSplit.Split(lines[i]);
                if (values.Length != languages.Length)
                {
                    Debug.LogError($"Error in localization file format at line {i + 1}. Number of values doesn't match the number of languages.");
                    continue;
                }
                string key = values[0];
                Dictionary<string, string> translations = new Dictionary<string, string>();

                for (int j = 1; j < values.Length; j++)
                {
                    translations.Add(languages[j], values[j].Trim(' ', '\"'));
                }

                localizedText.Add(key, translations);
            }
        }
        else
        {
            Debug.LogError("Localization file not found: " + filePath);
        }
    }

    public string GetLocalizedText(string key)
    {
        if (localizedText.TryGetValue(key, out var translations))
        {
            if (translations.TryGetValue(currentLanguage, out var text))
                return text;
            if (translations.TryGetValue(defaultLanguage, out text))
                return text;
        }
        return $"MISSING: {key}";
    }

    public void SetLanguage(string language)
    {
        if (currentLanguage != language)
        {
            currentLanguage = language;
            OnLanguageChanged?.Invoke();
        }
    }
}
