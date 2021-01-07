using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Networking.Types;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Unit : MonoBehaviour
{

    public string modelFaction; //dwarf, undead
    public string modelName; //character name
    public string modelType; //noble, slayer etc.
    public bool modelIsHero; //hero unit or henchman

    public int modelMovement;
    public int modelWS;
    public int modelBS;
    public int modelStrength;
    public int modelToughness;
    public int modelWounds;
    public int modelInitiative;
    public int modelAttacks;
    public int modelLeadership;
    public int modelExperience;

    public bool canMove;
    public bool canFire;
    public bool canFight;
    public bool canCast;
    public bool stayedStill;
    public bool isFighting;

    public bool selected;
    public bool moving;
    public bool shooting;

    public List<EquipmentBaseInfo> unitEquipment = new List<EquipmentBaseInfo>();
    public bool hasRangedWeapon;

    private PlayerScript player;
    private PlayerScript[] playerList;
    private PhotonView pv;

    private Light greenRing;
    public LineRenderer lRend;
    private List<Light> otherGreenRings = new List<Light>();
    private List<Unit> otherUnits = new List<Unit>();
    public NavMeshAgent navAgent;
    private NavMeshPath navPath;
    
    public float maxDist;
    public float soFar;

    public Vector3[] clearLine;
    private Color origColor;
    private Color fade;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
            return;

        navPath = new NavMeshPath();
        origColor = GetComponent<MeshRenderer>().material.color;

        //player = GetComponentInParent<PlayerScript>();
        //As Units are spawned indepentably through Proton, and setting children in transform causes issues, use player list to find our player and set them

        playerList = FindObjectsOfType<PlayerScript>();
        foreach (PlayerScript p in playerList)
        {
            if (pv.IsMine)
                player = p;
        }

        //get this units green highlight ring, find all rings and store them in a list
        greenRing = GetComponentInChildren<Light>();
        lRend = GetComponentInChildren<LineRenderer>();
        navAgent = GetComponent<NavMeshAgent>();

        GameObject[] rings = GameObject.FindGameObjectsWithTag("GreenRing");
        for (int i = 0; i < rings.Length; i++)
        {
            //if(is) Need to check and only add compenents of local player, else we'll be unselecting for other players as well
         //   if (pv.IsMine)
         //   {
                otherGreenRings.Add(rings[i].GetComponent<Light>());
                otherUnits.Add(rings[i].GetComponentInParent<Unit>());
         //   }
        }

        //Debug purposes, randomize stats
        TestSetDetails();

        //Unit checks if it has been given a ranged weapon, if not, it will be skipped in the ranged combat step
        foreach (EquipmentBaseInfo e in unitEquipment)
        {
            if (e is Ranged)
            {
                hasRangedWeapon = true;
                canFire = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Stop players controlling other users units
        if (!pv.IsMine)
            return;

        //To account for load issues, if we don't have a player assigned, run start again to assign
        if (player == null)
            Start();

        if (player.isThisPlayersTurn)
        {
            //Turn steps: 1 - Move, 2 - Shoot/magic, 3 - Melee combat
            if (canMove && player.playerTurnStep == 1)
            {
                if (navAgent.isStopped == false)
                    navAgent.isStopped = true;

                //If currently in combat
                if (isFighting)
                {
                    canMove = false;
                    moving = false;
                }
                //Set from UI when player selects unit to move
                if (moving)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        //Get mouse press for target location to move to, if over part of the UI, ignore. Otherwise, clicking 'Accept' on UI changes raycast target to worldspace behind the UI panel
                        if (EventSystem.current.IsPointerOverGameObject())
                        {
                            // Do nothing
                        }
                        else
                        {
                            RaycastHit hit;
                            //Cant use main camera, need player cam
                            Ray ray = player.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                            if (Physics.Raycast(ray, out hit, 100))
                            {

                                Debug.Log("Raycast");
                                soFar = 0.0f;
                                // Get target location on navmesh, calculate a patch from the unit to the hit, then go through it step by step. Add each step to soFar to track distance, and when you reach max dist. for that unit, plot final point and draw line

                                NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, navPath);
                                for (int i = 0; i < navPath.corners.Length - 1; i++) // Leave room to add 1
                                {
                                    float segmentDistance = (navPath.corners[i + 1] - navPath.corners[i]).magnitude; //Get next seg length
                                    if (soFar + segmentDistance <= maxDist) //If less than max, add to steps we can walk, go again
                                    {
                                        soFar += segmentDistance;
                                    }
                                    else // Path length exceeds maxDist
                                    {
                                        Vector3 finalPoint = navPath.corners[i] + ((navPath.corners[i + 1] - navPath.corners[i]).normalized * (maxDist - soFar));
                                        NavMesh.CalculatePath(transform.position, finalPoint, NavMesh.AllAreas, navPath);
                                        navAgent.SetPath(navPath);
                                        Debug.DrawLine(transform.position, finalPoint, Color.red, 2, false); //TODO: Come back and draw line for each part and array, clear.
                                        Vector3[] points = new Vector3[2];
                                        points[0] = transform.position;
                                        points[1] = finalPoint;
                                        lRend.enabled = true;
                                        lRend.SetPositions(points);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if(hasRangedWeapon && canFire && player.playerTurnStep == 2)
            {
                //If the player's ranged drop down is empty, add equipment to list 
                if (player.rangedDrop.options.Count == 0)
                {
                    List<string> m_DropOptions = new List<string> {};

                    foreach (Ranged rw in unitEquipment)
                    {
                        //Add to drop down
                        m_DropOptions.Add(rw.equipmentName);
                    }

                    player.rangedDrop.AddOptions(m_DropOptions);

                    //grey if unavailable
                }


            }
            else if (!canFire && player.playerTurnStep == 2) 
            {
                ShowUninteractable();
            }
        }
    }

    public void ShowUninteractable() // For showing unit already given order/unavailable
    {
        fade = GetComponent<MeshRenderer>().material.color;
        fade.a = 150f;
        GetComponent<MeshRenderer>().material.color = fade;
    }

    private void OnMouseDown()
    {
        //highlight, turn off other highlights, set this unit selected
        //TODO: Maybe a manager or better way to handle this?
        foreach (Light l in otherGreenRings)
                l.enabled = false;
            
        foreach (Unit u in otherUnits)
                u.selected = false;

        selected = true;
        greenRing.enabled = true;
        //set stats in UI
        player.UpdateUIUnitStats(this);
        Debug.Log("Should update UI");
        //check for move
        

    }

    void TestSetDetails()
    {
        modelFaction = "Dwarf";
        modelName = "Harbek";
        modelType = "Slayer";
        modelIsHero = true;
        modelMovement = Random.Range(3,8);
        modelWS = Random.Range(1, 4);
        modelBS = Random.Range(1, 5);
        modelStrength = Random.Range(1, 6);
        modelToughness = Random.Range(1, 6);
        modelWounds = Random.Range(1, 6);
        modelInitiative = Random.Range(1, 6);
        modelAttacks = Random.Range(1, 6);
        modelLeadership = Random.Range(1, 9);
        modelExperience = 13;

        foreach (EquipmentBaseInfo e in unitEquipment)
        {
            if (e is Ranged)
            {
                hasRangedWeapon = true;
                canFire = true;
            }
        }
    }

    void ProgressPlayerTurnStep()
    {
        GetComponent<MeshRenderer>().material.color = origColor;

    }
}
