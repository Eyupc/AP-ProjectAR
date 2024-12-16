using UnityEngine;
using TMPro;
using System.Collections;

public class WelcomeStop1Overlay : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI welcomeText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

    public void ShowOverlay()
    {
        StartCoroutine(AnimateOverlay());
    }

    private IEnumerator AnimateOverlay()
    {
        // Fade in
        float duration = 1f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            canvasGroup.alpha = elapsed / duration;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Hold
        yield return new WaitForSeconds(5f);

        // Fade out
        elapsed = 0;
        while (elapsed < duration)
        {
            canvasGroup.alpha = 1 - (elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}