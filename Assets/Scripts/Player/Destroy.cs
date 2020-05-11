using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destroy : MonoBehaviour
{
    private PhotonView pv;
    public bool isDestroy = false;

    void OnEnable() {
        pv = GetComponent<PhotonView>();    
    }

    void Update() {
        if (isDestroy) {
            PhotonNetwork.Destroy(pv);
        }
    }
}
