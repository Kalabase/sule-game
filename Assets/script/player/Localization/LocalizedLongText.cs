using UnityEngine;
using TMPro;

public class LocalizedLongText : MonoBehaviour
{
    public string storyFilePath = "story/intro.txt";
    public TextMeshProUGUI storyText;

    void Start()
    {
        storyText.text = LocalizationManager.Instance.LoadLocalizedLongText(storyFilePath);
    }
}
