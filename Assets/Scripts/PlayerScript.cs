using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Photon.Pun;
using System.IO;

public class PlayerScript : MonoBehaviour    
{
    public GameObject defaultUnit;

    public int playerTurnRoll;
    public bool playerTurnOver;
    public bool isThisPlayersTurn;
    public TurnManager tm;
    public Unit[] units;
    public List<Unit> myUnits = new List<Unit>();
    public Unit currentUnit;
    private PhotonView PV;

    public int playerTurnStep = 1;

    public Text MovementStat;
    public Text SprintStat;
    public Text WeaponSkillStat;
    public Text BallisticSkillStat;
    public Text StrengthStat;
    public Text ToughnessStat;
    public Text WoundsStat;
    public Text InitiativeStat;
    public Text AttacksStat;
    public Text LeadershipStat;

    public Dropdown rangedDrop;

    private bool playerMoveOver;
    private bool playerShootOver;
    private bool playerFightOver;


    private List<Transform> unitSpawns;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        tm = FindObjectOfType<TurnManager>();
        Camera playerCam = GetComponent<Camera>();
        AudioListener listener = GetComponent<AudioListener>();
        Canvas canvas = GetComponentInChildren<Canvas>();

        unitSpawns = new List<Transform>(); //Initialize spawn location list

        playerMoveOver = false;
        playerShootOver = false;
        playerFightOver = false;

        //Remove other player camera/audio listener so not on wrong camera
        if (!PV.IsMine)
        {
            Destroy(playerCam);
            Destroy(listener);
            canvas.gameObject.SetActive(false);
        }


        if (PV.IsMine)
            SpawnUnits();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!isLocalPlayer)
        {
            return;
        }*/
    }


    public void SpawnUnits()
    {
        //Find all unit spawns for player number and spawn units
        //This is placeholder while I work on combat
        //TODO: Allow users to place their own units from array
        int spawnNumber = int.Parse(PhotonNetwork.LocalPlayer.NickName);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player " + spawnNumber + " Unit Spawns"))
        {
            unitSpawns.Add(go.GetComponent<Transform>());
        }

        foreach (Transform t in unitSpawns)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonUnitPrefab"), t.position, Quaternion.identity, 0);
        }

        ListMyUnits();
    }

    //Simply get a list of which units this player owns
    public void ListMyUnits()
    {

        units = FindObjectsOfType<Unit>();

        foreach (Unit u in units)
        {
            if (PV.IsMine)
            {
                myUnits.Add(u);
            }
        }
    }

    public void CheckTurn()
    {
        //Set by the turn manager
        if (isThisPlayersTurn)
        {
            //allow doing stuff, maybe move to be called at the end of unit move/shoot function etc.
            //Moving, checking if all units that can move have or have passed
            if (playerTurnStep == 1)
            {
                playerMoveOver = true;
                int unitsChecked = 0;
                foreach (Unit u in myUnits)
                {
                    if (u.canMove == true)
                        playerMoveOver = false;
                    unitsChecked++;
                }
                if (playerMoveOver && unitsChecked == myUnits.Count)
                {
                    Debug.Log("PlayerTurnStep over, units checked: " + unitsChecked + " of: " + myUnits.Count);
                    playerMoveOver = false;
                    playerTurnStep++;
                }
            }
            //Shooting, checking if all units that can shoot have or have passed
            else if (playerTurnStep == 2)
            {
                playerShootOver = true;
                foreach (Unit u in myUnits)
                {
                    if (u.canFire == true)
                        playerShootOver = false;
                }
                if (playerShootOver)
                {
                    playerShootOver = false;
                    playerTurnStep++;
                }
            }
            //Combat, checking if all units that are in combat have fought
            else if (playerTurnStep == 3)
            {
                playerFightOver = true;
                foreach (Unit u in myUnits)
                {
                    if (u.canFight == true)
                        playerFightOver = false;
                }
                if (playerFightOver)
                {
                    playerFightOver = false;
                    TurnOver();
                }
            }
        }
    }

    void TurnOver()
    {
        //lock models, stop movement, check for units not moved?
        isThisPlayersTurn = false;
        playerTurnStep = 1;
        foreach(Unit u in myUnits)
        {
            u.canMove = true;
            if(u.hasRangedWeapon)
                u.canFire = true;
            u.canFight = true;
        }
        tm.ChangeTurn();
    }

    //Get units details when clicked and update the UI accordingly
    public void UpdateUIUnitStats(Unit u)
    {
        MovementStat.text = u.modelMovement.ToString();
        SprintStat.text = (u.modelMovement*2).ToString();
        WeaponSkillStat.text = u.modelWS.ToString();
        BallisticSkillStat.text = u.modelBS.ToString();
        StrengthStat.text = u.modelStrength.ToString();
        ToughnessStat.text = u.modelToughness.ToString();
        WoundsStat.text = u.modelWounds.ToString();
        InitiativeStat.text = u.modelInitiative.ToString();
        AttacksStat.text = u.modelAttacks.ToString();
        LeadershipStat.text = u.modelLeadership.ToString();
        currentUnit = u;
    }

    //Update the ranged weapon drop down
    public void UpdateRangedList(Unit u)
    {
        List<string> options = new List<string>();
        rangedDrop.ClearOptions();
        foreach (EquipmentBaseInfo e in u.unitEquipment)
        {
            if (e is Ranged)
            {
                options.Add(e.equipmentName);
            }
        }
        rangedDrop.AddOptions(options);
    }

    //Called from UI, sets the appropriate stats for movement choice
    public void LookingToMove()
    {
        //Error text for no unit
        currentUnit.maxDist = currentUnit.modelMovement;
        currentUnit.moving = true;
    }

    public void LookingToSprint()
    {
        //Error text for no unit
        currentUnit.maxDist = (currentUnit.modelMovement * 2);
        currentUnit.moving = true;
    }

    public void LookingToStayStill()
    {
        //Error text for no unit
        currentUnit.maxDist = 0;
        currentUnit.moving = true;
        currentUnit.stayedStill = true;
        Debug.Log("stayed still");
    }

    //Process from UI with accepted movement
    public void AcceptMove()
    {
        if (!isThisPlayersTurn)
            Debug.Log("It's not your go"); //Theoretically shouldn't have the buttons till it's your move but placeholder
        else if (currentUnit.moving)
        {
            currentUnit.navAgent.isStopped = false;
            currentUnit.moving = false;
            currentUnit.canMove = false;
            if (currentUnit.maxDist > currentUnit.modelMovement) //if sprinting, can't fire
            {
                currentUnit.canFire = false;
            }
            currentUnit.maxDist = currentUnit.modelMovement;
            currentUnit.lRend.enabled = false;
            currentUnit.ShowUninteractable();
            CheckTurn();
        }
        //else "You haven't moved this unit yet"
    }

    //SHOOTING: Check targets?

    //Process from UI with accepted shot
    public void AcceptShot(Unit uTarget, Ranged rangedWep)
    {
        //If no target, return and pass for this unit
        if (uTarget == null)
        {
            currentUnit.canFire = false;
            return;
        }
        else
        {
            //half range
            //cover
            //special rules check
            //weapon
        }
    }

    /*[Command]
    void CmdStartUpOnNetwork()
    {
        //Spawn a unit and give me authority
        GameObject go = Instantiate(defaultUnit);
        go.transform.parent = this.transform;
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }*/
}
