using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    [Header("Life Animators")]
    public List<Animator> lifeAnimators = new List<Animator>();

    private int lastLives = -1;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Auto-fill from Health parent if empty
        if (lifeAnimators.Count == 0)
        {
            var healthGO = GameObject.Find("Health");
            if (healthGO != null)
            {
                foreach (Transform child in healthGO.transform)
                {
                    var anim = child.GetComponent<Animator>();
                    if (anim != null)
                        lifeAnimators.Add(anim);
                }

                lifeAnimators.Sort((a, b) =>
                    a.transform.GetSiblingIndex()
                    .CompareTo(b.transform.GetSiblingIndex()));
            }
        }
    }

    public void SetLives(int currentLives)
    {
        int total = lifeAnimators.Count;
        currentLives = Mathf.Clamp(currentLives, 0, total);

        // First setup
        if (lastLives == -1)
        {
            lastLives = currentLives;
            return;
        }

        // Lost lives
        if (currentLives < lastLives)
        {
            for (int i = currentLives; i < lastLives; i++)
            {
                PlayBreakAnimation(i);
            }
        }

        // Gained lives (reset to glimmer)
        if (currentLives > lastLives)
        {
            for (int i = lastLives; i < currentLives; i++)
            {
                ResetLife(i);
            }
        }

        lastLives = currentLives;
    }

    void PlayBreakAnimation(int index)
    {
        var anim = lifeAnimators[index];

        anim.SetTrigger("Break");
        anim.SetTrigger("Hit");
        
    }

    void ResetLife(int index)
    {
        var anim = lifeAnimators[index];

        anim.ResetTrigger("Break");
        anim.ResetTrigger("Hit");
        anim.Play("MaskGlimmer", 0, 0f);
    }

    public int MaxLives() => lifeAnimators.Count;
}
