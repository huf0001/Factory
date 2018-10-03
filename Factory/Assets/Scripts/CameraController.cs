using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform[] players;
    private Transform gameCamera;
    private float startHeight = 0f;
    [SerializeField] private float boundaryXAxis = 12f;
    [SerializeField] private float boundaryZAxis = 6f;
    private float xRatio = 0f;
    private float zRatio = 0f;
    private float heightMultiplier = 0f;

	// Use this for initialization
	void Start ()
    {
        if (players.Length == 0)
        {
            Debug.Log("CameraController needs the player objects");
        }
        else if (players.Length == 2)
        {
            //boundaryZAxis = Vector3.Distance(players[0].position, players[1].position);
        }
        else if (players.Length > 2)
        {
            Debug.Log("Why are there more than two players attached to the Camera Controller?");
        }

        gameCamera = this.transform;
        startHeight = gameCamera.position.y;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (players.Length == 1)
        {
            gameCamera.position = new Vector3(players[0].position.x, gameCamera.position.y, players[0].position.z);
        }
        else if (players.Length == 2)
        {
            Vector3 midpoint = (players[0].position + players[1].position) / 2f;

            xRatio = (players[0].position.x - players[1].position.x) / boundaryXAxis;
            zRatio = (players[0].position.z - players[1].position.z) / boundaryZAxis;

            if (xRatio < 0)
            {
                xRatio = -xRatio;
            }

            if (zRatio < 0)
            {
                zRatio = -zRatio;
            }


            if (xRatio > zRatio)
            {
                heightMultiplier = xRatio;
            }
            else
            {
                heightMultiplier = zRatio;
            }

            if (heightMultiplier < 1)
            {
                heightMultiplier = 1;
            }

            Debug.Log("xRatio: " + xRatio + ". zRatio: " + zRatio + ". heightMultiplier: " + heightMultiplier + ".");

            gameCamera.position = new Vector3(midpoint.x, startHeight * heightMultiplier, midpoint.z);
        }
	}
}
