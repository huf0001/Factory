using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPart : Scalable
{
	// Use this for initialization
	void Start ()
    {
        Initialise(false, false, false, false, true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateScaling(2);
	}

    public void Reveal()
    {
        Expanding = true;
    }
}
