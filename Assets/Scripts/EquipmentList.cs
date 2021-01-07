using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentList : MonoBehaviour
{
    public List<EquipmentBaseInfo> equipment;
    // Start is called before the first frame update

    void Awake()
    {
        BuildItemDatabase();
    }

    //This builds this item database at start by creating a list of items using EquipmentBaseInfo and assigning item IDs so that they can be added to unit inventories
    void BuildItemDatabase()
    {
        equipment = new List<EquipmentBaseInfo>();
        {

            //RANGED WEAPONS
            new Ranged(0, "Short Bow", 16, 3, 1, false, true, true, 0);
            new Ranged(1, "Bow", 24, 3, 1, false, true, true, 0);
            new Ranged(2, "Long Bow", 30, 3, 1, false, true, true, 0);
            new Ranged(3, "Elf Bow", 36, 3, 1, false, true, true, -1);
            new Ranged(4, "Crossbow", 30, 4, 1, false, true, false, 0);
            new Ranged(5, "Sling", 18, 3, 1, false, true, true, 0); //can fire twice at half range
            //Throwing Star
            //Repeater Crossbow
            //Crossbow Pistol

            new Ranged(9, "Pistol", 6, 4, 2, true, true, true, -2);
            //Duelling Pistol
            //Blunderbuss
            new Ranged(12, "Handgun", 24, 4, 2, true, true, false, -2);

            //ARMOUR (House rule stats)
            new Armour(13, "Heavy Armour", 4);
            new Armour(14, "Light Armour", 5);
            new Armour(15, "Shield", 6);
            new Armour(16, "Buckler", 0);
            new Armour(17, "Helmet", 0);

            //MELEE
            new Melee(17, "Dagger", 0, false, false, false, false, +1);
            new Melee(18, "Hammer, Staff, Mace or Club", 0, false, false, false, false, 0);
            new Melee(19, "Axe", 0, false, false, false, false, -2);
            new Melee(20, "Sword", 0, true, false, false, false, 0);
            new Melee(21, "Flail", 2, false, false, false, true, 0);
            new Melee(22, "Morning Star", 1, false, false, false, false, 0); //can't dual wield
            new Melee(23, "Halberd", 0, false, false, false, true, 0);
            new Melee(24, "Spear", 0, false, true, false, false, 0);
            new Melee(25, "Two-Handed Sword, Hammer, Axe etc.", 0, false, false, true, true, 0);
        }
    }

    public EquipmentBaseInfo GetEquip(int id)
    {
        return equipment.Find(equip => equip.equipmentID == id);
    }

}
