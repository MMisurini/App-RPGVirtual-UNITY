using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePanel : MonoBehaviour
{
    public GameObject panel;

    private Toggle toggle;
    void Update(){
        if (toggle == null)
            toggle = GetComponent<Toggle>();

        if (toggle.isOn) {
            panel.SetActive(true);
        } else {
            panel.SetActive(false);
        }
    }
}
