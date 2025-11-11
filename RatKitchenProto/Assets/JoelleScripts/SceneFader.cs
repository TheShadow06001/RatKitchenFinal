using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Start()
    {
        // Fade in when the scene starts 
        StartCoroutine(FadeInRoutine(1f));
    }

    public void RatHoleFade(float fadeDuration)
    {
        StartCoroutine(FadeTeleport(fadeDuration));
    }

    public IEnumerator FadeTeleport(float fadeDuration)
    {
        yield return StartCoroutine(FadeOutRoutine(fadeDuration));
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeInRoutine(fadeDuration));
    }

    // Fade from transparent and then black
    public IEnumerator FadeOutRoutine(float duration)
    {
        float t = 0;
        var c = image.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = t / duration;
            image.color = c;
            yield return null;
        }
    }

    // Fade from black and then transparent
    public IEnumerator FadeInRoutine(float duration)
    {
        float t = 0;
        var c = image.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = 1f - t / duration;
            image.color = c;
            yield return null;
        }
    }
}