using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour
{
    private AudioSource audio;
    private void OnEnable() {
        audio = transform.parent.parent.GetComponent<AudioSource>();
    }

    public void Disable() {
        if (audio.isPlaying) {
            GetComponent<Image>().color = Color.red;
            transform.GetChild(0).GetComponent<Text>().text = "DESMUTE";
            audio.Stop();
        } else {
            GetComponent<Image>().color = Color.white;
            transform.GetChild(0).GetComponent<Text>().text = "MUTE";
            audio.Play();
        }
    }
}
