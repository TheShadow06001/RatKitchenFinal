using UnityEngine;

public class Choppingboard : MonoBehaviour
{
    [Header("Variables")][SerializeField] private int maxScale = 8;

    [Header("References")]
    [SerializeField]
    private GameObject knifePrefab;

    [SerializeField] private Transform parentTransform;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        var value = Random.Range(2, maxScale);
        Debug.Log(value);

        for (var i = 0; i < value * 4; i += 4)
            Instantiate(knifePrefab, new Vector3(transform.position.x + i, transform.position.y, transform.position.z),
                Quaternion.identity, parentTransform);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}