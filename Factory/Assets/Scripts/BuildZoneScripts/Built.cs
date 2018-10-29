using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Built : Scalable
{
    [SerializeField] private float lifespan = 2f;
    private float time = 0f;
    private BuildZone buildZone = null;
    private Schema schema = null;

    // Use this for initialization
    void Start()
    {
        Initialise(true);
    }

    public Schema Schema
    {
        get
        {
            return schema;
        }

        set
        {
            schema = value;
        }
    }

    public BuildZone BuildZone
    {
        get
        {
            return buildZone;
        }

        set
        {
            buildZone = value;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Dropping)
        {
            if (time < lifespan)
            {
                time += Time.deltaTime;
            }
            else
            {
                Shrinking = true;

                if (buildZone.Number == 1)
                {
                    GameObject.Find("GameControllerCamera").GetComponent<GameController>().P1BuiltObjectShowing = false;
                }
                else if (buildZone.Number == 2)
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
        else
        {
            if (FinishedDropping())
            {
                Destroy(this.gameObject);
                Destroy(this);
            }
        }
    }

    private void FixedUpdate ()
    {
        UpdateScaling();
    }
}
