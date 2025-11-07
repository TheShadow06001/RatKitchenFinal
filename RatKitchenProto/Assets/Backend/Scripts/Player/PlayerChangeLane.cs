using UnityEngine;

public class PlayerChangeLane : MonoBehaviour
{
    private int lane = 1;

    public float leftLaneX = -0.5f;
    public float middleLaneX = 0.5f;
    //public float rightLaneX = 0.5f;
    private float targetHorizontalX;
    [SerializeField] private float laneChangeSpeed = 10f;

    public bool isChangingLanes;
    [SerializeField] private float laneChangeThreshold = 0.1f;
    void Start()
    {
        targetHorizontalX = middleLaneX;
        isChangingLanes = false;
    }

    void Update()
    {
        PlayerChangeLine();

        Vector3 currentPos = transform.position;

        if (Mathf.Abs(currentPos.x - targetHorizontalX) > laneChangeThreshold)
        {
            isChangingLanes = true;
        }
        else { isChangingLanes = false; }

        float newX = Mathf.Lerp(currentPos.x, targetHorizontalX, Time.deltaTime * laneChangeSpeed);
        transform.position = new Vector3(newX, currentPos.y, currentPos.z);
    }
    void PlayerChangeLine()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (lane < 1)
            {
                lane++;
            }

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (lane > 0)
            {
                lane--;
            }
        }
        if (lane == 0)
        {
            targetHorizontalX = leftLaneX;
        }
        else if (lane == 1)
        {
            targetHorizontalX = middleLaneX;
        }
        /*else if (lane == 2)
        {
            targetHorizontalX = rightLaneX;
        }*/
    }
}
