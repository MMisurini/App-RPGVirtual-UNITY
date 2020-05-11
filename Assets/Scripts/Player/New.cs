using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class New : MonoBehaviour
{
    public Slider slider;
    public Button button;
    public Toggle toggle;

    public void Clear() {
        foreach (GameObject a in GameObject.FindGameObjectsWithTag("Brushes")) {
            a.GetComponent<Destroy>().isDestroy = true;
        }
    }

}
