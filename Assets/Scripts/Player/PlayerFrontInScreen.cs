using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFrontInScreen : MonoBehaviour
{
    private SpriteRenderer sr;

    private PhotonView pv;

    void Update(){
        if (sr == null) {
            sr = GetComponent<SpriteRenderer>();
        }

        if (pv == null) {
            pv = GetComponent<PhotonView>();
        }

        if (!pv.IsMine) {
            return;
        }

        sr.sortingLayerName = "Players";
        sr.sortingOrder = 1;
    }
}
