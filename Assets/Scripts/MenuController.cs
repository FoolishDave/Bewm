using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public List<Transform> SpawnPoints;
    public GameObject PlayerPrefab;
    public GameObject SceneCamera;
    private int spawnPoint;
    private PlayerDetails playerDetails;
    private List<GameObject> players = new List<GameObject>();

    public enum MenuState
    {
        Main,
        CharacterSelect,
        MultiplayerCharacterSelect,
        Options,
        Credits,
        Other
    }

    public MenuState CurrentState;

    // Use this for initialization
	void Start ()
	{
	    CurrentState = MenuState.Main;
	    playerDetails = GameObject.Find("PlayerDetails").GetComponent<PlayerDetails>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (CurrentState == MenuState.CharacterSelect)
	    {
	        
	        for (int i = 0; i < 5; i++)
	        {
	            bool controllerActive = Input.GetButtonDown("Jump " + i);
	            if (controllerActive && GameObject.Find("Player " + i) == null)
	            {
                    Debug.Log("Spawning player at spawn point " + spawnPoint);
	                GameObject newPlayer = Instantiate(PlayerPrefab, SpawnPoints[spawnPoint]);
	                newPlayer.name = "Player " + i;
	                newPlayer.GetComponentInChildren<Camera>().enabled = false;
	                newPlayer.GetComponentInChildren<PlayerInfo>().ControllerType = (PlayerInfo.InputMethod) i;
	                newPlayer.GetComponent<Renderer>().material = playerDetails.Materials[i];
                    playerDetails.Controller.Add(i);
                    playerDetails.Colors.Add(newPlayer.GetComponent<Renderer>().material.color);
                    players.Add(newPlayer);
	                spawnPoint++;
	                if (spawnPoint >= SpawnPoints.Count)
	                {
	                    spawnPoint = 0;
	                }
	            }
	        }

	        foreach (int i in playerDetails.Controller)
	        {
	            if (Input.GetAxisRaw("ColorUp " + i) >= 0.99f)
	            {
	                Material playerMat = players[playerDetails.Controller.IndexOf(i)].GetComponent<Renderer>().material;
	                Color existingColor = playerMat.color;
	                float h;
                    float s;
	                float v;
                    Color.RGBToHSV(existingColor,out h, out s, out v);
	                h += 0.01f;
	                if (h > 1f)
	                    h = 0f;
	                playerDetails.Colors[playerDetails.Controller.IndexOf(i)] = Color.HSVToRGB(h, s, v);
                    playerMat.color = playerDetails.Colors[playerDetails.Controller.IndexOf(i)];
	            } else if
	                (Input.GetAxisRaw("ColorDown " + i) >= 0.99f)
	            {
	                Material playerMat = players[playerDetails.Controller.IndexOf(i)].GetComponent<Renderer>().material;
	                Color existingColor = playerMat.color;
	                float h;
	                float s;
	                float v;
	                Color.RGBToHSV(existingColor, out h, out s, out v);
	                h -= 0.01f;
	                if (h <= 0.01f)
	                    h = 0.99f;
	                playerDetails.Colors[playerDetails.Controller.IndexOf(i)] = Color.HSVToRGB(h, s, v);
	                playerMat.color = playerDetails.Colors[playerDetails.Controller.IndexOf(i)];
                }
	        }
	    }
	}
}
