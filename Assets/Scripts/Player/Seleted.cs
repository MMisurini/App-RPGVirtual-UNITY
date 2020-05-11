using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Seleted : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    private PhotonView pv;

    void Start() {
        pv = GetComponent<PhotonView>();
    }

    private void OnMouseDown() {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;

        mOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag() {
        pv.RPC("Drag", RpcTarget.All);
    }

    [PunRPC]
    private void Drag() {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;

        transform.position = Camera.main.ScreenToWorldPoint(mousePoint) + mOffset;
    }
}
