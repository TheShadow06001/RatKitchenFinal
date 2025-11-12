using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        var entry = GameObject.Find("EntryPoint").transform;
        transform.position = entry.position;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}