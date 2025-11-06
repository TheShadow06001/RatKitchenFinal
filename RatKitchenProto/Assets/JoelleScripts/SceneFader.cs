using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Start()
    {
        // Fade in when the scene starts 
        StartCoroutine(FadeInRoutine(1f));
    }

    // Fade from transparent and then black
    public IEnumerator FadeOutRoutine(float duration)
    {
        float t = 0;
        Color c = image.color;
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
        Color c = image.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / duration);
            image.color = c;
            yield return null;
        }
    }
}
