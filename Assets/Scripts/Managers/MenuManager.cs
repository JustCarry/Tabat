using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject lobbyMenu;
    [SerializeField] private GameObject roomsMenu;
    [SerializeField] private GameObject loading;

    public static MenuManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        lobbyMenu.SetActive(false);
        roomsMenu.SetActive(false);
        loading.SetActive(true);
        
    }

    void Awake(){
        Instance = this;
    }


    public void OpenLobbyMenu(){
        lobbyMenu.SetActive(true);
        roomsMenu.SetActive(false);
        loading.SetActive(false);
    }
    public void OpenRoomsMenu(){
        roomsMenu.SetActive(true);
        loading.SetActive(false);
        lobbyMenu.SetActive(false);
    }
    public void Loading(){
        loading.SetActive(true);
        roomsMenu.SetActive(false);
        lobbyMenu.SetActive(false);
        
    }
}
