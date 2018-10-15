using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltScript : MonoBehaviour
{
    [SerializeField] private float lifespan = 3f;
    private float time = 0f;
    private BuildZoneScript buildZone = null;

    // Use this for initialization
    void Start()
    {

    }

    public BuildZoneScript BuildZone
    {
        set
        {
            buildZone = value;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (time < lifespan)
        {
            time += Time.deltaTime;
        }
        else
        {
            buildZone.ChangeCurrentSchema(this.gameObject, this);
        }
    }
}
