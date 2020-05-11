using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBrushes : MonoBehaviour
{
    public void Clear() {
        Transform player = GameObject.Find("Draw").transform;

        if (player.childCount > 0) {
            for (int i = 0; i < player.childCount; i++) {
                Destroy(player.GetChild(i).gameObject);
            }
        }
    }
}
