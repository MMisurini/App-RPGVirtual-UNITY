using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Resize : MonoBehaviour
{
    public Type typeForm;

    public CameraAtributtes cameraAtributtes;

    private RaycastHit hit;
    private Vector3 camPosition;
    private Vector3 origin;
    private Vector3 direction;

    private bool col;
    private bool stopCheck;


    private int i = 0;
    private void OnEnable() {
        camPosition = Camera.main.transform.localPosition;
        direction = new Vector3(0, 0, camPosition.z);
        stopCheck = false;

        cameraAtributtes = new CameraAtributtes();

        switch (typeForm) {
            case Type.Camera:
                Cam();
                break;
            case Type.Player:
                Player();
                break;
            case Type.Menu:
                StartCoroutine(Menu());
                break;
            case Type.Slider:
                Slider();
                break;
            case Type.ListPlayer:
                ListPlayer();        
                break;
            case Type.ListPlayerPrefab:
                ListPlayerPrefab();
                break;
            case Type.ListPlayerRollDice:
                ListPlayerRollDice();
                break;
            case Type.ListPlayerRollDicePrefab:
                ListPlayerRollDicePrefab();
                break;
        }
    }

    void Cam() {
        Camera.main.orthographicSize = (Screen.height / 100) / 2;
    }

    void ListPlayer() {
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.15f, 0);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(-(Screen.width * 0.15f) / 2, 0);
    }
    void ListPlayerPrefab() {
        GetComponent<RectTransform>().sizeDelta = new Vector2((Screen.width * 0.15f) - 10, 40);
    }
    void ListPlayerRollDice() {
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.5f, 0);
        GetComponent<RectTransform>().anchoredPosition = new Vector2((Screen.width * 0.5f) / 2, 0);
    }
    void ListPlayerRollDicePrefab() {
        GetComponent<RectTransform>().sizeDelta = new Vector2((Screen.width * 0.5f), 80);
    }

    IEnumerator Menu() {
        yield return new WaitForEndOfFrame();
        origin = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * cameraAtributtes.indexY, -(Camera.main.transform.position.z - 1)));
        Debug.DrawRay(origin, direction, Color.green, 500f);
        if (Physics.Raycast(origin, direction, out hit, 500)) {
            col = true;
        } else {
            col = false;
        }

        stopCheck = false;
    }

    void Slider() {
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.15f,0);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(-(Screen.width * 0.15f) / 2,0);
    }

    private void Update() {

        if (typeForm == Type.Menu) {
            if (!stopCheck) {
                origin = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/ 2, Screen.height * cameraAtributtes.indexY, -(Camera.main.transform.position.z - 1)));
                Debug.DrawRay(origin, direction, Color.green, 500f);
                if (Physics.Raycast(origin, direction, out hit, 500)) {
                    if (!col) {
                        float valueX = Mathf.RoundToInt(Screen.height - ((Screen.height * cameraAtributtes.indexY) * 2));
                        //float valueXX = Screen.width * (cameraAtributes.indexX * 2);
                        Resolution.CheckResolution(Mathf.RoundToInt(valueX));

                        Camera.main.orthographicSize = (Resolution.camValueY / 100) / 2;
                        cameraAtributtes.pixelImageSizeX = Resolution.camValueX;
                        cameraAtributtes.pixelImageSizeY = Resolution.camValueY;
                        cameraAtributtes.size = Camera.main.orthographicSize;
                        stopCheck = true;
                    } else {
                        cameraAtributtes.indexY -= 0.01f;
                    }
                } else {
                    if (!col) {
                        cameraAtributtes.indexY += 0.01f;
                    } else {
                        float valueX = Mathf.RoundToInt(Screen.height - ((Screen.height * cameraAtributtes.indexY) * 2));
                        //float valueXX = Screen.width * (cameraAtributes.indexX * 2);
                        Resolution.CheckResolution(Mathf.RoundToInt(valueX));

                        Camera.main.orthographicSize = (Resolution.camValueY / 100f) / 2f;
                        cameraAtributtes.pixelImageSizeX = Resolution.camValueX; 
                        cameraAtributtes.pixelImageSizeY = Resolution.camValueY; 
                        cameraAtributtes.size = Camera.main.orthographicSize;
                        stopCheck = true;
                    }
                }
            }
        }
    }

    void Player() {
        float scaleX = (float)(Screen.width - (Screen.width * 0.95));
        float valueX = scaleX / Screen.width;

        transform.localScale = new Vector2(valueX, valueX);

        float posX = (float)(Screen.height * 0.3);
        var valuePosX = posX / Screen.height;

        transform.position = new Vector2(0, -(valuePosX * 7.2f) / 2);
    }

}

public enum Type { Menu, Player, Camera, Slider, Null, ListPlayer, ListPlayerPrefab, ListPlayerRollDice, ListPlayerRollDicePrefab}
