using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    [Range(0f, 1f)]
    public float approximationRatio;

    [Range(0f, 1f)]
    public float maximumDistancePercentage;

    public float minimumDistance;

    public Vector3 cameraOffset;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 camPos = new Vector3();

        foreach (Player p in GameManager.Instance.Players)
        {
            camPos += new Vector3(p.transform.position.x, 0, p.transform.position.z);
        }
        camPos /= GameManager.Instance.Players.Count;

        transform.position = Vector3.Lerp(transform.position, camPos + cameraOffset.normalized, approximationRatio);
        transform.LookAt(camPos);
        
        transform.position += transform.forward * (Vector3.Distance(camPos, transform.position) - CalculateCameraDistance(camPos));
    }

    float CalculateCameraDistance(Vector3 center)
    {
        //Realization Angles: to check when an entity is going too far
        float radAngle = cam.fieldOfView * Mathf.Deg2Rad;
        float radHFOV = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) * cam.aspect);
        float hFOV = Mathf.Rad2Deg * radHFOV;
        float checkVAngle = maximumDistancePercentage * cam.fieldOfView / 2;
        float checkHAngle = maximumDistancePercentage * hFOV / 2;

        //Calculate the distance the camera should be at to be have checkVAngle degrees from the center of the scene to the farthest character
        float farthestDistanceX = 0f;
        float farthestDistanceY = 0f;

        foreach (Player p in GameManager.Instance.Players)
        {
            float distanceX = Mathf.Abs(p.transform.position.x - center.x);
            float distanceY = Mathf.Abs(p.transform.position.z - center.z);
            if (distanceX > farthestDistanceX) farthestDistanceX = distanceX;
            if (distanceY > farthestDistanceY) farthestDistanceY = distanceY;
        }

        float heightFromX = farthestDistanceX / Mathf.Tan(checkHAngle * Mathf.Deg2Rad);
        float heightFromY = farthestDistanceY / Mathf.Tan(checkVAngle * Mathf.Deg2Rad);

        return Mathf.Max(heightFromX, heightFromY, minimumDistance);
    }
}
