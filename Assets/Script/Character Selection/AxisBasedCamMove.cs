using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisBasedCamMove : MonoBehaviour
{
    public int playerIndex;
    public Transform[] camPositions;
    [Range(0, 1)]
    public float movementSpeed;
    CameraRotate readyRotation;
    Vector3 originalForward;

    void Start()
    {
        readyRotation = GetComponent<CameraRotate>();
        originalForward = transform.forward;
    }

    void Update()
    {
        if (CharacterSelectionManager.Instance.Ready[playerIndex])
        {
            readyRotation.enabled = true;
            transform.forward = Vector3.Lerp(transform.forward, transform.position - GetForward(), movementSpeed);
        }
        else
        {
            readyRotation.enabled = false;
            transform.forward = Vector3.Lerp(transform.forward, transform.position - camPositions[CharacterSelectionManager.Instance.SelectedModifier[playerIndex]].position, movementSpeed);
        }
    }

    Vector3 GetForward()
    {
        var distanceVector = transform.position - camPositions[camPositions.Length - 1].position.normalized;
        var myDistVect = transform.position - transform.forward;
        return new Vector3(myDistVect.x, distanceVector.y, myDistVect.z);
    }
}