using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S_MainMenuCamera : MonoBehaviour
{
    [SerializeField] private Vector3 EndPosition = new(0.064000003f, 1.91999996f, -9.61600018f);
    [SerializeField] private float PanDuration = 4f;
    [SerializeField] private float FadeDuration = 3.5f;

    public Image FadeImage;
    public Canvas FadeCanvas;
    private bool HasMoved;

    private Vector3 StartPosition;

    private void Start()
    {
        StartPosition = transform.position;
        StartCoroutine(CameraStartAnim());
        StartCoroutine(CameraFadeOnEnter());
    }

    private IEnumerator CameraStartAnim()
    {
        var elapsed = 0f;

        while (elapsed < PanDuration)
        {
            elapsed += Time.deltaTime;
            var t = Mathf.Clamp01(elapsed / PanDuration);
            t = Mathf.SmoothStep(0f, 1f, t);
            transform.position = Vector3.Lerp(StartPosition, EndPosition, t);
            Camera.main.fieldOfView = Mathf.SmoothStep(55f, 45f, t);
            yield return null;
        }

        transform.position = EndPosition;
    }

    private IEnumerator CameraFadeOnEnter()
    {
        var elapsed = 0f;

        while (elapsed < FadeDuration)
        {
            elapsed += Time.deltaTime;
            var t = Mathf.Clamp01(elapsed / FadeDuration);
            var eased = Mathf.Pow(t, 6f);

            var Alpha = FadeImage.color;
            Alpha.a = 1f - eased;
            FadeImage.color = Alpha;

            if (Alpha.a <= 0f) FadeCanvas.gameObject.SetActive(false);
            yield return null;
        }
    }
}