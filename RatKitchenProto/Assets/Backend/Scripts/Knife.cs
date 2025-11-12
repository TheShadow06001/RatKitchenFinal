using System.Collections;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 1;

   
    private Vector3 startPos;
    private Vector3 endPos;
    [SerializeField] private Transform endPosTransform;


    private bool isComplete;



    void Start()
    {


        startPos = transform.position;
        endPos = endPosTransform.position;

        isComplete = false;
        StartCoroutine(Chop());

    }

    // Update is called once per frame
    void Update()
    {

        if (isComplete && GameManager.Instance.CheckState<PlayingState>())
        {
            isComplete = false;
            StartCoroutine(Chop());
        }
    }

    IEnumerator Chop()
    {
        if (GameManager.Instance.CheckState<PlayingState>())
        {
            SoundManager.Instance.PlaySoundEffect(SoundEffects.KnifeTrapWhoosh);

            for (float i = 0; i < 1; i += Time.deltaTime * speed)
            {
                while (GameManager.Instance.CheckState<PauseState>())
                    yield return null;

                transform.position = Vector3.Lerp(startPos, endPos, i);

                yield return null;
            }

            SoundManager.Instance.PlaySoundEffect(SoundEffects.KnifeTrapWhoosh);

            for (float t = 0; t < 1; t += Time.deltaTime * speed)
            {
                while (GameManager.Instance.CheckState<PauseState>())
                    yield return null;

                transform.position = Vector3.Lerp(endPos, startPos, t);

                yield return null;
            }
        }

        isComplete = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HeartDisplay.instance.TakeDamage();
        }
    }
}
