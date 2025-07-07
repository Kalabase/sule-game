using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    public string key;

    void Start()
    {
        var text = GetComponent<TextMeshProUGUI>();
        if (LocalizationManager.Instance != null)
        {
            text.text = LocalizationManager.Instance.GetLocalizedValue(key);
        }
    }
}
