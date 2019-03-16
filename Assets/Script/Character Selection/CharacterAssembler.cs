using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAssembler
{
	public static void Assemble(GameObject body, GameObject defensive, GameObject complimentary1, GameObject complimentary2, GameObject weapon)
    {
        Transform defNode = null;
        Transform comp1Node = null;
        Transform comp2Node = null;
        Transform weaponNode = null;

        foreach (Transform child in body.transform)
        {
            foreach (Transform c in child.transform)
            {
                if (c.name == "Node_Def") defNode = c;
                if (c.name == "Node_Comp1") comp1Node = c;
                if (c.name == "Node_Comp2") comp2Node = c;
                if (c.name == "Node_Weapon") weaponNode = c;
            }
        }

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

    public static GameObject ChangeBody(GameObject newBody, GameObject oldBody, GameObject defensive, GameObject complimentary1, GameObject complimentary2, GameObject weapon)
    {
        defensive.transform.parent = null;
        complimentary1.transform.parent = null;
        complimentary2.transform.parent = null;
        weapon.transform.parent = null;
        GameObject.Destroy(oldBody);
        Assemble(newBody, defensive, complimentary1, complimentary2, weapon);
	    return newBody;
    }

    public static GameObject ChangePart(GameObject oldPart, GameObject newPart)
    {
        newPart.transform.position = oldPart.transform.position;
        newPart.transform.rotation = oldPart.transform.rotation;
        newPart.transform.parent = oldPart.transform.parent;
        GameObject.Destroy(oldPart);
	    return newPart;
    }
}