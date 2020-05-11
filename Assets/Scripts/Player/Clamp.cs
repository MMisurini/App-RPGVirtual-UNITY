using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clamp : MonoBehaviour
{
    public float YMaxValue = 0;
    public float YMinValue = 0;
    public float XMaxValue = 0;
    public float XMinValue = 0;

    private PhotonView pv;

    void Start() {
        pv = GetComponent<PhotonView>();
    }

    void Update(){
        if (!pv.IsMine) {
            return;
        }

        var limitXMax = Screen.width - (Screen.width * 0.15f);
        var limitXMin = Screen.width * 0.15f;
        XMaxValue = Camera.main.ScreenToWorldPoint(new Vector3(limitXMax,0)).x;
        XMinValue = Camera.main.ScreenToWorldPoint(new Vector3(limitXMin,0)).x;

        var limitYMax = Screen.height - (Screen.height * 0.3f);
        var limitYMin = 0;
        YMaxValue = Camera.main.ScreenToWorldPoint(new Vector3(0,limitYMax)).y;
        YMinValue = Camera.main.ScreenToWorldPoint(new Vector3(0,limitYMin)).y;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, XMinValue ,XMaxValue), Mathf.Clamp(transform.position.y, YMinValue, YMaxValue), 0);

    }
}
