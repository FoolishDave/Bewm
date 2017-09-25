using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStart : MonoBehaviour
{

    public List<Transform> SpawnPoints;
    public GameObject PlayerPrefab;
    public List<GameObject> Canvases;
    private PlayerDetails playerDetails;

	// Use this for initialization
	void Start ()
	{
	    Cursor.lockState = CursorLockMode.Locked;
        playerDetails = GameObject.Find("PlayerDetails").GetComponent<PlayerDetails>();
	    List<Transform> usableSpawns = new List<Transform>(SpawnPoints);
        List<GameObject> players = new List<GameObject>();
	    int pNum = 0;
	    foreach (int controller in playerDetails.Controller)
	    {
	        Transform currentSpawnPoint = usableSpawns[Mathf.FloorToInt(Random.Range(0f, usableSpawns.Count - 1))];
	        GameObject newPlayer = Instantiate(PlayerPrefab, currentSpawnPoint);
            players.Add(newPlayer);
	        usableSpawns.Remove(currentSpawnPoint);
	        newPlayer.name = "Player " + controller;
	        newPlayer.GetComponent<PlayerInfo>().ControllerType = (PlayerInfo.InputMethod) controller;
	        newPlayer.GetComponent<Renderer>().material = playerDetails.Materials[pNum];
	        newPlayer.GetComponent<Renderer>().material.color = playerDetails.Colors[pNum];
	        pNum++;
	    }
	    int horizontalCameras = 1;
	    int verticalCameras = 2;
	    if (players.Count > 2)
	        horizontalCameras = 2;
	    int player = players.Count-1;
        for (float hor = 0.0f ; hor < horizontalCameras / 2.0f; hor += 0.5f)
	    {
	        for (float ver = 0.0f; ver < verticalCameras / 2.0f; ver += 0.5f)
	        {
	            Camera pCam = players[player].GetComponentInChildren<Camera>();
                pCam.rect = new Rect(hor,ver,1f/horizontalCameras,1f/verticalCameras);
	            player--;
	        } 
	    }

	    for (int i = 0; i < Canvases.Count; i++)
	    {
	        if (i >= players.Count)
                Canvases[i].SetActive(false);
	        else
	        {
	            Canvases[i].GetComponent<Canvas>().worldCamera = players[i].GetComponentInChildren<Camera>();
	            Canvases[i].GetComponent<Canvas>().planeDistance = 1;
	            players[i].GetComponent<PlayerInfo>().PlayerCanvasObject = Canvases[i];
	        }
	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
