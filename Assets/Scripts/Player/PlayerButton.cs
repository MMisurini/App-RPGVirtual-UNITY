using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    public void setupBtn() {
        GetComponent<Button>().onClick.AddListener(delegate { btnClicked(transform.GetChild(0).GetChild(0).GetComponent<Text>().text); });
    }

    public void btnClicked(string param) {
        GameObject.Find("Controller").GetComponent<Game>().PlayerSelectedGameobject(param);
        GameObject.Find("Controller").GetComponent<Game>().PlayerSelectedPlayer(param);
        GameObject.Find("Controller").GetComponent<Game>().PlayerSelectedListing(param);
    }
}
