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

    public void StartFadeAndRespawn(GameObject player, Vector3 spawnPoint, float duration)
    {
        StartCoroutine(FadeAndRespawn(player, spawnPoint, duration));
    }

    private IEnumerator FadeAndRespawn(GameObject player, Vector3 spawnPoint, float duration)
    {
        yield return FadeOutRoutine(duration);

        player.transform.position = spawnPoint;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        DifficultyManager.Instance.LevelComplete();
        yield return FadeInRoutine(duration);

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