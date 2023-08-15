using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkUI : NetworkBehaviour
{

    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;
    [SerializeField] private TextMeshProUGUI PlayersCountText;
    [SerializeField] GameObject Menu;
 

    private NetworkVariable<int> playersNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private void Awake()
    {
        HostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            var clientId = NetworkManager.Singleton.LocalClientId;
            //Debug.Log(clientId);
            Menu.SetActive(false);
        });

        ClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            var clientId = NetworkManager.Singleton.LocalClientId;
            //Debug.Log(clientId);
            Menu.SetActive(false);
        });
    }

    private void Update()
    {
        PlayersCountText.text = "PLAYERS: " + playersNum.Value.ToString();
        if (!IsServer) return;
        playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;

    }



}
