using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public ItemManager itemManager;
    public UIManager uiManager;
    public Wallet wallet;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        StartCoroutine(InitializeManagers());
    }

    void Start()
    {
        AudioConfiguration config = AudioSettings.GetConfiguration();
        if (config.sampleRate != 48000) {
            config.sampleRate = 48000;
            AudioSettings.Reset(config);
            Debug.LogWarning("Audio sample rate was not 48000 Hz. It has been reset.");
        }
    }


    private System.Collections.IEnumerator InitializeManagers()
    {
        // ItemManager'ı başlat
        itemManager.Initialize();
        Debug.Log("ItemManager initialized.");
        yield return null; // Bir frame bekle

        // Wallet'ı başlat
        wallet.Initialize();
        Debug.Log("Wallet initialized.");
        yield return null; // Bir frame bekle

        // UIManager'ı başlat
        uiManager.Initialize();
        Debug.Log("UIManager initialized.");

        var apple = ItemManager.GetItem("apple");
    }
}










