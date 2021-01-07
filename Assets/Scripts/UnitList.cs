using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour
{

    public List<ModelBasic> UnitTemplates = new List<ModelBasic>();

    // Start is called before the first frame update
    void Start()
    {
        UnitTemplates.Add(new ModelBasic(ModelBasic.ModelFaction.Dwarf, "Harbek", 0, "Slayer", true, 3, 4, 3, 3, 4, 1, 2, 1, 9, 13));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
