using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class TransformParent : MonoBehaviour
{
    public Vector2 a;

    private RectTransform main;
    private PhotonView pv;

    private void OnEnable() {
        main = GetComponent<RectTransform>();
        pv = GetComponent<PhotonView>();

        transform.SetParent(GameObject.FindGameObjectWithTag("Maps").transform);
    }

    private void Update() {
        pv.RPC("UpdateRect", RpcTarget.AllBuffered, a);
    }

    [PunRPC]
    public void UpdateRect(Vector2 value) {
        main.anchoredPosition = value;
    }


}
