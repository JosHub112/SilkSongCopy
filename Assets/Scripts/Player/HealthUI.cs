using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Hearts UI")]
    public Image[] hearts;            // assign heart images in inspector
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    // Call once at start to set total hearts
    public void SetMaxHealth(int maxHealth)
    {
        // if hearts array doesn't match, try to clamp or leave as-is
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < maxHealth)
            {
                hearts[i].gameObject.SetActive(true);
                hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }

    // Update shown hearts to match current health
    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeartSprite;
            else
                hearts[i].sprite = emptyHeartSprite;
        }
    }
}
