using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ModelBasic : MonoBehaviour
{
    public ModelFaction mFaction; //dwarf, undead
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

    public int modelID;

    public bool hasMoved;
    public bool hasFired;
    public bool hasFought;
    public bool hasCast;

    public int state; //0 = fine, 1 = knocked down, 2 = stunned, 3 = dead or just delete?

    public enum ModelFaction
    {
        Dwarf,
        Skaven,
        WitchHunter,
        Undead,
        Reikland,
        Marienburg,
        Middenheim,
        Possessed,
        Sisters
    }

    public ModelBasic(ModelFaction faction, string name, int id, string mclass, bool isHero, int movement, int ws, int bs, int str, int tgh, int w, int i, int a, int l, int xp)
    {
        mFaction = faction;
        modelName = name;
        modelID = id;
        modelType = mclass;
        modelIsHero = isHero;
        modelMovement = movement;
        modelWS = ws;
        modelBS = bs;
        modelStrength = str;
        modelToughness = tgh;
        modelWounds = w;
        modelInitiative = i;
        modelAttacks = a;
        modelLeadership = l;
        modelExperience = xp;
}
}
