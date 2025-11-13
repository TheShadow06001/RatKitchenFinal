using System.Collections;
using UnityEngine;

public class Blender : MonoBehaviour
{
    [Header("Variables")] [SerializeField] private int damage = 1;

    [SerializeField] private float range;
    [SerializeField] private float dieRange;
    [SerializeField] private float fallPercentage;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float checkCooldown;
    [SerializeField] private Transform deathPoint;
    private bool canFall = true;
    private bool canTakeDamage = true;
    private readonly float endRotation = -90;
    private bool hasFallen;
    private GameObject player;

    private int test;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        SoundManager.Instance.PlaySoundEffect(SoundEffects.BlenderBlending);
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.CheckState<PlayingState>())
        {
            if (CheckRange(false) <= range) FallOver();

            if (CheckRange(true) <= dieRange && canTakeDamage)
            {
                player.GetComponent<HP>().TakeDamage(damage);
                canTakeDamage = false;
                StartCoroutine(TimerDie());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red - new Color(0, 0, 0, 0.5f);
        Gizmos.DrawSphere(deathPoint.position, dieRange);
        Debug.DrawRay(transform.position, transform.right * range, Color.green);
    }


    private void FallOver()
    {
        if (!hasFallen)
        {
            float value = Random.Range(0, 100);
            if (value >= 100 - fallPercentage && canFall)
            {
                StartCoroutine(Fall());

                hasFallen = true;
                test++;
                Debug.Log("Falling took " + test + " tries");
            }
            else if (canFall)
            {
                test++;
                Debug.Log("FallOver() has run " + test + " times");
                canFall = false;
                StartCoroutine(TimerFall());
            }
        }
    }

    private float CheckRange(bool toDie)
    {
        if (!toDie)
        {
            var distance = Vector3.Distance(player.transform.position, transform.position);
            return distance;
        }
        else
        {
            var distance = Vector3.Distance(player.transform.position, deathPoint.position);
            return distance;
        }
    }

    private IEnumerator TimerFall()
    {
        yield return new WaitForSeconds(checkCooldown);
        canFall = true;
    }

    private IEnumerator TimerDie()
    {
        yield return new WaitForSeconds(checkCooldown);
        canTakeDamage = true;
    }

    private IEnumerator Fall()
    {
        for (float i = 0; i > endRotation; i -= Time.deltaTime * fallSpeed)
        {
            while (GameManager.Instance.CheckState<PauseState>())
                yield return null;

            transform.rotation = Quaternion.Euler(new Vector3(0, 90, i));

            yield return null;
        }
    }
}