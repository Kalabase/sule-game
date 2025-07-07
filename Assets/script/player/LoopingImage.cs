using UnityEngine;
using UnityEngine.UI;

public class LoopingImage : MonoBehaviour
{
    public SpriteCollection spriteCollection;
    public float changeInterval = 0.5f;

    private Image image;
    private int index = 0;
    private float timer = 0f;

    void Start()
    {
        image = GetComponent<Image>();

        if (spriteCollection != null && spriteCollection.sprites.Length > 0)
        {
            image.sprite = spriteCollection.sprites[0];
        }
    }

    void Update()
    {
        if (spriteCollection == null || spriteCollection.sprites.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= changeInterval)
        {
            timer = 0f;
            index = (index + 1) % spriteCollection.sprites.Length;
            image.sprite = spriteCollection.sprites[index];
        }
    }
}
