using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 1;

    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private MeshRenderer line;

    
    private Vector3 startPosition;
    private Vector3 endPosition;

    private bool isComplete;



    void Start()
    {
        

        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z );

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
        

        int value1 = 0;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HP>().TakeDamage(damage);
        }
    }
}
