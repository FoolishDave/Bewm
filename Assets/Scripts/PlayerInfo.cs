using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {
    public enum InputMethod
    {
        Keyboard,
        Controller1,
        Controller2,
        Controller3,
        Controller4,
        Controller5,
        Controller6,
        Controller7,
        Controller8,
        Controller9,
        Controller10,
        Controller11,
        MindControl
    }

    public InputMethod ControllerType;
    public GameObject PlayerCanvasObject;
    public int Lives;

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Kill"))
        {
            Lives--;
            if (Lives <= 0)
            {
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<PlayerAttack>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                PlayerCanvasObject.GetComponentInChildren<Text>().text = "Super Dead";
            }
            else
            {
                List<Transform> respawnPoints = GameObject.Find("GameMaster").GetComponent<MatchStart>().SpawnPoints;
                Transform respawnPoint = respawnPoints[Mathf.FloorToInt(Random.Range(0, respawnPoints.Count - 1))];
                transform.position = respawnPoint.position;
                PlayerCanvasObject.GetComponentInChildren<Text>().text = "Lives: " + Lives;
            }
        }
    }
}
