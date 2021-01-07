using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSetting : MonoBehaviour
{
    // Start is called before the first frame update
    public static MultiplayerSetting multiplayerSetting;

    public bool delayStart;
    public int maxPlayers;
    public int menuScene;
    public int multiplayerScene;

    public void Awake()
    {
        if (multiplayerSetting == null)
            multiplayerSetting = this;
        else
        {
            if (multiplayerSetting != this)
            {
                Destroy(multiplayerSetting.gameObject);
                multiplayerSetting = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
