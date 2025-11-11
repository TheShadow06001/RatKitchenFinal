using System.Collections;
using UnityEngine;

public class DestinationTrigger : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    private SceneFader fader;
    private Vector3 spawnPoint;

    private void Awake()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Destination").transform.position;
        fader = FindFirstObjectByType<SceneFader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && fader != null)
        {
            //other.transform.position = spawnPoint;
            //StartCoroutine(FadeTeleport());
            //DifficultyManager.Instance.LevelComplete();

            fader.StartFadeAndRespawn(other.gameObject, spawnPoint, fadeDuration);
        }
    }

    //private IEnumerator FadeTeleport()
    //{
    //    if (fader == null || spawnPoint == null) yield break;

    //    // fade to black
    //    yield return StartCoroutine(fader.FadeOutRoutine(fadeDuration));

    //    // Move player back to SpawnPoint


    //    // this one is optional which resets the velocity if/when using Rigidbody
    //    //var rb = player.GetComponent<Rigidbody>();
    //    //if (rb != null)
    //    //{
    //    //    rb.linearVelocity = Vector3.zero;
    //    //    rb.angularVelocity = Vector3.zero;
    //    //}

    //    // Fade back in
    //    yield return StartCoroutine(fader.FadeInRoutine(fadeDuration));
    //}
}