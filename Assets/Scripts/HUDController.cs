using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;

public class HUDController : MonoBehaviour
{
    public GameObject panelJugadores;
    public TextMeshProUGUI lifePoints;

    public GameObject PlayerListPanel, team1content, team2content, contenedorPrefab;

    public List<GameObject> playerList = new List<GameObject>();
    public List<string> gamename = new List<string>();
    public List<float> gamehealth = new List<float>();

    public static HUDController instance;
    void Awake()
    {
        instance = this;
        
    }

    public void Update()
    {
        
    }

    // Update is called once per frame
    public void UpdateHealth(int PlayerLife)
    {
        lifePoints.text = PlayerLife.ToString();
    }

    public void ShowPanelPlayers()
    {
        panelJugadores.SetActive(true);        
    }

    public void HidePanelPlayer()
    {
        panelJugadores.SetActive(false);
    }

    private void AddPlayertoList()
    {
        playerList.ForEach(x => Destroy(x));
        playerList.Clear();
        gamename.Clear();
        gamehealth.Clear();

        Debug.Log("AddPlayerToList: " + PhotonNetwork.PlayerList.Length);


        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object vida = p.CustomProperties["Health"];
            int equip = (int)p.CustomProperties["equipo"];
            GameObject obj = Instantiate(contenedorPrefab, equip == 1 ? team1content.transform : team2content.transform);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = p.NickName + " -----> " + (int)vida;
            playerList.Add(obj);
            
        }
    }

    public void ShowPlayers()
    {

            PlayerListPanel.SetActive(true);
            AddPlayertoList();
    }
}
