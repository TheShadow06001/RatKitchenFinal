using UnityEngine;

public class CameraFollowPoints : MonoBehaviour
{
    [SerializeField] private Transform generationPoint;
    [SerializeField] private Transform destructionPoint;
    [SerializeField] private float generationOffset;
    [SerializeField] private float destroyOffset;

    private void Update()
    {
        generationPoint.position = new Vector3(transform.position.x /*+ generationOffset*/, generationPoint.position.y,
            generationPoint.position.z + generationOffset);
        destructionPoint.position = new Vector3(transform.position.x /*+ destroyOffset*/, destructionPoint.position.y,
            destructionPoint.position.z + destroyOffset);
    }
}