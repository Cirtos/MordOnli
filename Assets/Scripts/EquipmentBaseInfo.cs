using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;


//This script is definiting equipment classes and what information is necessary so that they can be added to inventories
public abstract class EquipmentBaseInfo : ScriptableObject
{
    public string equipmentName;
    public int equipmentID;
}

public class Ranged: EquipmentBaseInfo
{
    public int range;
    public int strength;
    public int reloadTime;
    public bool isBlackpowder;
    public bool isLoaded;
    public bool canMoveAndFire;
    public int saveModifier;

    public Ranged(int id, string itemName, int range, int strength, int reloadTime, bool isBlackpowder, bool isLoaded, bool canMoveAndFire, int saveMod)
    {
        this.equipmentID = id;
        this.equipmentName = itemName;
        this.range = range;
        this.strength = strength;
        this.reloadTime = reloadTime;
        this.isBlackpowder = isBlackpowder;
        this.isLoaded = isLoaded;
        this.canMoveAndFire = canMoveAndFire;
        this.saveModifier = saveMod;
    }
}

public class Melee: EquipmentBaseInfo
{
    public int strength;
    public bool canParry;
    public bool hitsFirst;
    public bool hitsLast;
    public bool twoHands;
    public int saveModifier;

    public Melee(int id, string itemName, int strength, bool canParry, bool hitsFirst, bool hitsLast, bool twoHands, int saveMod)
    {
        this.equipmentID = id;
        this.equipmentName = itemName;
        this.strength = strength;
        this.canParry = canParry;
        this.hitsFirst = hitsFirst;
        this.hitsLast = hitsLast;
        this.twoHands = twoHands;
        this.saveModifier = saveMod;
    }
}
 
public class Armour: EquipmentBaseInfo
{
    public int save;

    public Armour(int id, string itemName, int save)
    {
        this.equipmentID = id;
        this.equipmentName = itemName;
        this.save = save;
    }
}

public class Other: EquipmentBaseInfo
{
    //Things like climb rope, lucky charms, madcaps etc.
}
