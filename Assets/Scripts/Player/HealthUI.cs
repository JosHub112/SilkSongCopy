using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    public List<Image> lifeImages = new List<Image>();

    [Header("Sprites")]
    public Sprite fullSprite;
    public Sprite hitSprite;
    public Sprite emptySprite;

    [Header("Animation")]
    public float hitDuration = 0.15f;

    private int lastLives = -1;
    private Dictionary<int, Coroutine> anims = new Dictionary<int, Coroutine>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (lifeImages == null || lifeImages.Count == 0)
        {
            var healthGO = GameObject.Find("Health");
            if (healthGO != null)
            {
                foreach (Transform child in healthGO.transform)
                {
                    var img = child.GetComponent<Image>();
                    if (img != null) lifeImages.Add(img);
                }

                lifeImages.Sort((a, b) =>
                    a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
            }
        }

        if (fullSprite == null && lifeImages.Count > 0)
            fullSprite = lifeImages[0].sprite;
    }

    public void SetLives(int currentLives)
    {
        int total = lifeImages.Count;
        currentLives = Mathf.Clamp(currentLives, 0, total);

        // First time setup
        if (lastLives == -1)
        {
            for (int i = 0; i < total; i++)
                lifeImages[i].sprite = i < currentLives ? fullSprite : emptySprite;

            lastLives = currentLives;
            return;
        }

        // Lost lives → animate
        if (currentLives < lastLives)
        {
            for (int i = currentLives; i < lastLives; i++)
                PlayHitAnimation(i);
        }

        // Gained lives → instant refill
        if (currentLives > lastLives)
        {
            for (int i = lastLives; i < currentLives; i++)
            {
                StopAnim(i);
                lifeImages[i].sprite = fullSprite;
            }
        }

        lastLives = currentLives;
    }

    void PlayHitAnimation(int index)
    {
        StopAnim(index);
        anims[index] = StartCoroutine(HitRoutine(index));
    }

    IEnumerator HitRoutine(int index)
    {
        lifeImages[index].sprite = hitSprite;
        yield return new WaitForSeconds(hitDuration);
        lifeImages[index].sprite = emptySprite;
    }

    void StopAnim(int index)
    {
        if (anims.TryGetValue(index, out Coroutine c))
        {
            StopCoroutine(c);
            anims.Remove(index);
        }
    }

    public int MaxLives() => lifeImages.Count;
}
