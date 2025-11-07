using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S_MainMenuCamera : MonoBehaviour
{
    [SerializeField] Vector3 EndPosition = new Vector3(0.064000003f, 1.91999996f, -9.61600018f);
    [SerializeField] float PanDuration = 4f;
    [SerializeField] float FadeDuration = 3.5f;

    public Image FadeImage;
    public Canvas FadeCanvas;

    Vector3 StartPosition;
    private bool HasMoved;

    void Start()
    {
        StartPosition = transform.position;
        StartCoroutine(CameraStartAnim());
        StartCoroutine(CameraFadeOnEnter());
    }

    IEnumerator CameraStartAnim()
    {
        float elapsed = 0f;

        while (elapsed < PanDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / PanDuration);
            t = Mathf.SmoothStep(0f, 1f, t);
            transform.position = Vector3.Lerp(StartPosition, EndPosition, t);
            yield return null;
        }
        transform.position = EndPosition;
    }

    IEnumerator CameraFadeOnEnter()
    {
        float elapsed = 0f;

        while (elapsed < FadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / FadeDuration);
            float eased = Mathf.Pow(t, 6f);

            Color Alpha = FadeImage.color;
            Alpha.a = 1f - eased;
            FadeImage.color = Alpha;

            if (Alpha.a <= 0f)
            {
                FadeCanvas.gameObject.SetActive(false);
            }
            yield return null;
        }
    }
}