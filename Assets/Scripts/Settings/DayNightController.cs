using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    private Color targetColor = new Color(130f / 255f, 130f / 255f, 130f / 255f); // Цвет 828282
    private Color originalColor = Color.white; // Белый цвет
    private bool isColorChanged = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isColorChanged)
            {
                // Возвращаем цвет на белый
                ChangeColor(prefab1, originalColor);
                ChangeColor(prefab2, originalColor);
            }
            else
            {
                // Меняем цвет на 828282
                ChangeColor(prefab1, targetColor);
                ChangeColor(prefab2, targetColor);
            }

            // Переключаем состояние
            isColorChanged = !isColorChanged;
        }
    }

    void ChangeColor(GameObject prefab, Color color)
    {
        // Находим все объекты, которые используют данный префаб
        SpriteRenderer[] allSprites = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer spriteRenderer in allSprites)
        {
            if (spriteRenderer.sprite == prefab.GetComponent<SpriteRenderer>().sprite)
            {
                spriteRenderer.color = color;
            }
        }
    }
}
