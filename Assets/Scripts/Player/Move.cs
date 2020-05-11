using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour
{

    [SerializeField] private Toggle toggle;
    [SerializeField] private Button clear;
    [SerializeField] private Slider slider;
    [Header("Draw")]
    [SerializeField] private GameObject Brush;

    private GameObject canvas;

    public List<GameObject> listBrushs;

    private GameObject player;
    private GameObject playerListing;
    private PhotonView pv;

    private int uiLayer;

    private EventSystem _eventSystem;

    void Start() {
        listBrushs = new List<GameObject>();

        uiLayer = LayerMask.NameToLayer("UI");
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }
    

    private void OnEnable() {
        slider = GameObject.FindGameObjectWithTag("Canvas").GetComponent<New>().slider;
        clear = GameObject.FindGameObjectWithTag("Canvas").GetComponent<New>().button;
        toggle = GameObject.FindGameObjectWithTag("Canvas").GetComponent<New>().toggle;

        canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    void Update() {
        if (!PhotonNetwork.IsMasterClient) {
            if (canvas.activeInHierarchy) {
                GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(1).gameObject.SetActive(false);
            }
        } else {
            Game game = GameObject.Find("Controller").GetComponent<Game>();

            if (game.playerSelected_Gameobject != null) {
                game.PlayerSelectedListing_Reset();

                player = game.playerSelected_Gameobject;
                playerListing = game.playerSelected_Listing;

                if(!playerListing.GetComponent<Image>().IsActive())
                    playerListing.GetComponent<Image>().enabled = true;

                pv = player.GetComponent<PhotonView>();
            }
            if (!toggle.isOn) {
                clear.gameObject.SetActive(false);
                slider.gameObject.SetActive(false);

                Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayLenght;

                if (Input.GetMouseButton(0)) {
                    if (Physics.Raycast(cameraRay, out rayLenght, 1000)) {
                        if (_eventSystem.IsPointerOverGameObject()) {
                            return;
                        }

                        if (game.playerSelected_Gameobject != null) {
                            pv.transform.position = rayLenght.point + new Vector3(0,.3f,0);
                        }
                    }
                }
            } else {
                clear.gameObject.SetActive(true);
                slider.gameObject.SetActive(true);

                Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayLenght;

                if (Input.GetMouseButton(0)) {
                    if (Physics.Raycast(cameraRay, out rayLenght, 1000)) {
                        if (rayLenght.collider.tag == "Maps") {
                            GameObject go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkBrushs"), rayLenght.point + Vector3.up * 0.1f, Brush.transform.rotation, 0);
                            go.transform.localScale = Vector3.one * slider.value;
                        }
                    }
                }
            }

        }
        
    }

}
