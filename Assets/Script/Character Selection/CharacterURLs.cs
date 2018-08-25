using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player File", menuName = "ScriptableObjects/Save Files/Player File")]
public class CharacterURLs : ScriptableObject
{

    public string bodyURL;
    public string weaponURL;
    public string defensiveURL;
    public string[] complementaryURL;
    //Add defensive skills and the rest of the stuff!
}
