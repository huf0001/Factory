using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltScript : ScalableScript
{
    [SerializeField] private float lifespan = 2f;
    private float time = 0f;
    private BuildZoneScript buildZone = null;
    private BuildSchemaScript schema = null;

    // Use this for initialization
    void Start()
    {
        Initialise();
    }

    public void SetSchemaAndZone(BuildSchemaScript s, BuildZoneScript b)
    {
        schema = s;
        buildZone = b;
    }
	
	// Update is called once per frame
	private void Update ()
    {
        Rotate();
        CheckScaling();

        if (time < lifespan)
        {
            time += Time.deltaTime;
        }
        else
        {
            Shrinking = true;

            if (buildZone.BuildZoneNumber == 1)
            {
                GameObject.Find("GameControllerCamera").GetComponent<GameController>().P1BuiltObjectShowing = false;
            }
            else if (buildZone.BuildZoneNumber == 2)
            {
                GameObject.Find("GameControllerCamera").GetComponent<GameController>().P2BuiltObjectShowing = false;
            }
            else
            {
                Debug.Log("Invalid build zone number");
            }
        }

        if (FinishedShrinking())
        {
            buildZone.ChangeCurrentSchema(schema, this.gameObject, this);
        }
    }
}
