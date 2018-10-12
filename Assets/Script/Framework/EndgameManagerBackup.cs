using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

/*public class EndgameManager : MonoBehaviour
{
    void LoadPlayers()
    {
        var playerInfo = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players");
        var organizedPlayers = playerInfo.playerControllers.OrderByDescending(a => playerInfo.playerScores[System.Array.IndexOf(playerInfo.playerControllers, a)]).ToArray();
        _players = new GameObject[organizedPlayers.Length];

        for (int i = 0; i < organizedPlayers.Length; i++)
        {
            var URLs = Serializacion.LoadJsonFromDisk<CharacterURLs>("Player " + (organizedPlayers[i] + 1));
            _controls = new Controller[4];
            _controls[i] = new Controller(i);

            //Dejo los objetos ccomo children del body por cuestiones de carga de los scripts. Assembler no debería generar problemas, ya que su parent objetivo sería el mismo.
            var player = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs.bodyURL), spawnPos[i].position, Quaternion.identity);
            var weapon = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs.weaponURL), player.transform.position, Quaternion.identity, player.transform);
            var comp1 = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 1/" + URLs.complementaryURL[0]), player.transform.position, Quaternion.identity, player.transform);
            var comp2 = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 2/" + URLs.complementaryURL[1]), player.transform.position, Quaternion.identity, player.transform);
            var def = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs.defensiveURL), player.transform.position, Quaternion.identity, player.transform);
            var tx = Instantiate(playerText, player.transform.position, Quaternion.identity, player.transform);
            tx.transform.localPosition = Vector3.zero;
            tx.transform.Rotate(transform.up, 180);

            CharacterAssembler.Assemble(player.gameObject, def, comp1, comp2, weapon);

            player.gameObject.layer = LayerMask.NameToLayer("Player" + (playerInfo.playerControllers[i] + 1));
            player.gameObject.tag = "Player " + (playerInfo.playerControllers[i] + 1);
            foreach (Transform t in player.transform)
            {
                t.gameObject.layer = LayerMask.NameToLayer("Player" + (playerInfo.playerControllers[i] + 1));
                t.gameObject.tag = "Player " + (playerInfo.playerControllers[i] + 1);
            }

            player.transform.forward = spawnPos[i].forward;
            _players[i] = player;

            //tx.GetComponentInChildren<Text>().text = player.gameObject.tag + "\n" + organizedPlayers[i];
        }

}
*/
