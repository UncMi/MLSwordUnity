using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject PlayerPrefab;
    public GameObject TestPlayerPrefab;


    GameObject NewPlayer;
    GameObject NewTestPlayer;

    public GameObject Player;
    public GameObject TestPlayer;
    

    Vector3 PlayerSpawn = new Vector3(-3, 0, 0);
    Vector3 TestPlayerSpawn = new Vector3(3, 0, 0);

    public void ResetMatch()
    {
        Player.transform.position = PlayerSpawn;
        TestPlayer.transform.position = TestPlayerSpawn;
    }

  
    public void RestartMatch()
    {
        Destroy(NewPlayer);
        Destroy(NewTestPlayer);
        
        NewPlayer = Instantiate(PlayerPrefab, PlayerSpawn, Quaternion.identity);
        NewTestPlayer = Instantiate(TestPlayerPrefab, TestPlayerSpawn, Quaternion.identity);
    }
}
