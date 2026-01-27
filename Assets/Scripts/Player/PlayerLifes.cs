using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 6;
    public int currentLives = 6;

    void Awake()
    {
        Debug.Log("[PlayerLives] Awake on: " + gameObject.name);

        if (HealthManager.Instance == null)
        {
            Debug.Log("[PlayerLives] No HealthManager found, creating one");
            var go = new GameObject("HealthManager");
            go.AddComponent<HealthManager>();
        }

        if (currentLives < 0)
        {
            Debug.Log("[PlayerLives] Setting currentLives to maxLives");
            currentLives = maxLives;
        }

        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        Debug.Log("[PlayerLives] Starting lives: " + currentLives);

        if (HealthManager.Instance != null)
        {
            Debug.Log("[PlayerLives] Syncing UI with HealthManager");
            HealthManager.Instance.SetLives(currentLives);
        }
        else
        {
            Debug.Log("[PlayerLives] HealthManager Instance still null");
        }
    }

    public void TakeDamage(int amount = 1)
    {
        Debug.Log("[PlayerLives] TakeDamage called with amount: " + amount);

        if (amount <= 0)
        {
            Debug.Log("[PlayerLives] Damage amount invalid (<=0)");
            return;
        }

        currentLives -= amount;
        Debug.Log("[PlayerLives] Lives after damage: " + currentLives);

        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        if (HealthManager.Instance != null)
        {
            Debug.Log("[PlayerLives] Updating UI to: " + currentLives);
            HealthManager.Instance.SetLives(currentLives);
        }
        else
        {
            Debug.Log("[PlayerLives] UI not updated: HealthManager missing");
        }

        if (currentLives <= 0) Die();
    }

    void Die()
    {
        Debug.Log("[PlayerLives] PLAYER DIED – reloading scene");

        // Optional: prevent negative lives or double calls
        currentLives = 0;

        // Reload current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

}
