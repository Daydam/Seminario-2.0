using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAssembler
{
	public static void Assemble(GameObject body, GameObject defensive, GameObject complimentary1, GameObject complimentary2, GameObject weapon)
    {
        var defNode = body.transform.Find("Node_Def");
        var comp1Node = body.transform.Find("Node_Comp1");
        var comp2Node = body.transform.Find("Node_Comp2");
        var weaponNode = body.transform.Find("Node_Weapon");

        defensive.transform.position = defNode.position;
        defensive.transform.rotation = defNode.rotation;
        complimentary1.transform.position = comp1Node.position;
        complimentary1.transform.rotation = comp1Node.rotation;
        complimentary2.transform.position = comp2Node.position;
        complimentary2.transform.rotation = comp2Node.rotation;
        weapon.transform.position = weaponNode.position;
        weapon.transform.rotation = weaponNode.rotation;

        defensive.transform.parent = body.transform;
        complimentary1.transform.parent = body.transform;
        complimentary2.transform.parent = body.transform;
        weapon.transform.parent = body.transform;
    }

    public static void ChangeBody(GameObject newBody, GameObject oldBody, GameObject defensive, GameObject complimentary1, GameObject complimentary2, GameObject weapon)
    {
        defensive.transform.parent = null;
        complimentary1.transform.parent = null;
        complimentary2.transform.parent = null;
        weapon.transform.parent = null;
        GameObject.Destroy(oldBody);
        Assemble(newBody, defensive, complimentary1, complimentary2, weapon);
    }

    public static void ChangePart(GameObject oldPart, GameObject newPart)
    {
        newPart.transform.position = oldPart.transform.position;
        newPart.transform.rotation = oldPart.transform.rotation;
        newPart.transform.parent = oldPart.transform.parent;
        GameObject.Destroy(oldPart);
    }
}
