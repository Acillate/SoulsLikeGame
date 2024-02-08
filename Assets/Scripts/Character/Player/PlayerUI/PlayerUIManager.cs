using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    [Header("NETWORK JOIN")]
    [SerializeField] bool startGameAsClient;

    private void Awake() {
        //There can only be one instance of this script at one time if another exists destroy it
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
    private void Update() 
    {
        if(startGameAsClient){
            startGameAsClient = false;
            //We must first shutdown because we hae started as a host during the title screen
            NetworkManager.Singleton.Shutdown();
            // We restart as a client
            NetworkManager.Singleton.StartClient();
        }
    }
}
