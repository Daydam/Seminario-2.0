using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public int playerIndex;
    public Transform[] camPositions;
    [Range(0,1)]
    public float movementSpeed;

	void Update ()
	{
        transform.position = Vector3.Lerp(transform.position, camPositions[CharacterSelectionManager.Instance.SelectedModifier[playerIndex]].position, movementSpeed);
        transform.forward = Vector3.Lerp(transform.forward, camPositions[CharacterSelectionManager.Instance.SelectedModifier[playerIndex]].forward, movementSpeed);
    }
}
