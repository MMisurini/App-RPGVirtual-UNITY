using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSetting : MonoBehaviour
{
    public static MultiplayerSetting multiplayerSettings;

    public bool delayStart;
    public int maxPlayer;

    public int menuScene;
    public int multiplayerScene;

    private void Awake() {
        if (MultiplayerSetting.multiplayerSettings == null) {
            MultiplayerSetting.multiplayerSettings = this;
        } else {
            if (MultiplayerSetting.multiplayerSettings != this)
                Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
