using System.Collections;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 1;

    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private MeshRenderer line;
    private Vector3 endPosition;

    private bool isComplete;


    private Vector3 startPosition;


    private void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);

        isComplete = false;
        StartCoroutine(Chop());
    }

    // Update is called once per frame
    private void Update()
    {
        if (isComplete && GameManager.Instance.CheckState<PlayingState>())
        {
            isComplete = false;
            StartCoroutine(Chop());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) other.gameObject.GetComponent<HP>().TakeDamage(damage);
    }

    private IEnumerator Chop()
    {
        var value1 = 0;

        if (GameManager.Instance.CheckState<PlayingState>())
        {
            for (float i = 0; i < startPosition.y; i += Time.deltaTime * speed)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, i);
                yield return null;

                if (transform.position == endPosition && value1 == 0)
                {
                    SoundManager.Instance.PlaySoundEffect(SoundEffects.KnifeTrapChop);
                    value1++;
                }
            }

            for (float i = 0; i < startPosition.y; i += Time.deltaTime * speed)
            {
                transform.position = Vector3.Lerp(endPosition, startPosition, i);
                yield return null;
            }
        }


        isComplete = true;
    }
}