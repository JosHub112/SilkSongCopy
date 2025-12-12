using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimationController : MonoBehaviour
{
    [System.Serializable]
    public struct KeyTriggerMapping
    {
        public string key;
        public string animationTrigger;
    }

    public KeyTriggerMapping[] keyMappings;
    public Text keyDisplayText; // Reference to the UI Text component to display the pressed key
    public float displayDuration = 1f; // Duration to display the pressed key

    private Animator animator;
    private Coroutine displayCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        PopulateTriggerMappings();
    }

    void Update()
    {
        foreach (var mapping in keyMappings)
        {
            if (Input.GetKeyDown(mapping.key))
            {
                // Trigger the corresponding animation
                animator.SetTrigger(mapping.animationTrigger);
                DisplayPressedKey(mapping.key);
            }
        }
    }

    private void PopulateTriggerMappings()
    {
        AnimatorControllerParameter[] parameters = animator.parameters;
        keyMappings = new KeyTriggerMapping[parameters.Length];
        int keyIndex = 1;

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].type == AnimatorControllerParameterType.Trigger)
            {
                if (keyIndex > 9)
                {
                    keyIndex = 1;
                }

                keyMappings[i] = new KeyTriggerMapping
                {
                    key = keyIndex.ToString(),
                    animationTrigger = parameters[i].name
                };

                keyIndex++;
            }
        }
    }

    private void DisplayPressedKey(string key)
    {
        if (keyDisplayText != null)
        {
            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }
            keyDisplayText.text = "YOU JUST PRESSED " + key ;
            displayCoroutine = StartCoroutine(HideKeyAfterDelay());
        }
    }

    private IEnumerator HideKeyAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        keyDisplayText.text = string.Empty;
    }
}
