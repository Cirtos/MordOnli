using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour

{
    public static TurnManager tm;

    public PlayerScript[] players;
    public int playerTurn;
    public int gameTurn;

    public Transform[] TwoPlayerSpawns;
    public Transform[] ThreePlayerSpawns;
    public Transform[] FourPlayerSpawns;

    //Singleton
    private void Awake()
    {
        if (tm == null)
            tm = this;
        else
        {
            if (tm != this)
            {
                Destroy(tm.gameObject);
                tm = this;
            }
        }
        //Not necessary, only 1 per scene, should delete on return?
        //DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerScript>();
        foreach(PlayerScript p in players)
        {
            //p.playerTurnRoll = Random.Range(1, 6);

        }
        //Replace with roll logic to determine turn order, currently just assigns
        playerTurn = 1;
        gameTurn = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //To counter issues with player 1 being out of range during Start call
        if (players.Length == 0)
            players = FindObjectsOfType<PlayerScript>();
        else if (gameTurn == 1 && playerTurn == 1)
            players[0].isThisPlayersTurn = true;
    }

    public void ChangeTurn()
    {
        if(playerTurn < players.Length-1)
        {
            playerTurn++;
            players[playerTurn].isThisPlayersTurn = true;
        }
        else
        {
            playerTurn = 0;
            gameTurn++;
            players[0].isThisPlayersTurn = true;
        }
    }

    //TODO: Use to determine player order
    public void UpdateTurnOrder()
    {
        foreach (PlayerScript p in players)
        {
            if (p.playerTurnRoll == 0)
                break;
        }
    }

    //Use playerlist from photon room list to assign spawn points/board edge
    public Transform ReturnSpawnPoint(int i) 
    {
        if (PhotonRoom.room.playersInRoom == 4)
            return FourPlayerSpawns[i];
        else if (PhotonRoom.room.playersInRoom == 3)
            return ThreePlayerSpawns[i];
        else
            return TwoPlayerSpawns[i];
    }
}
