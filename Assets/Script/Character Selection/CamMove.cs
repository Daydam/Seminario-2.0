using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public int playerIndex;
    public Transform[] camPositions;
    [Range(0,1)]
    public float movementSpeed;
    CameraRotate parentRotation;
    Vector3 originalParentForward;

    void Start()
    {
        parentRotation = transform.parent.GetComponent<CameraRotate>();
        originalParentForward = parentRotation.transform.forward;
    }

    void Update ()
	{
        if(CharacterSelectionManager.Instance.Ready[playerIndex])
        {
            parentRotation.enabled = true;
            transform.position = Vector3.Lerp(transform.position, camPositions[camPositions.Length-1].position, movementSpeed);
            transform.forward = Vector3.Lerp(transform.forward, camPositions[camPositions.Length - 1].forward, movementSpeed);
        }
        else
        {
            parentRotation.enabled = false;
            parentRotation.transform.forward = Vector3.Lerp(parentRotation.transform.forward, originalParentForward, movementSpeed);
            transform.position = Vector3.Lerp(transform.position, camPositions[CharacterSelectionManager.Instance.SelectedModifier[playerIndex]].position, movementSpeed);
            transform.forward = Vector3.Lerp(transform.forward, camPositions[CharacterSelectionManager.Instance.SelectedModifier[playerIndex]].forward, movementSpeed);
        }
    }
}
