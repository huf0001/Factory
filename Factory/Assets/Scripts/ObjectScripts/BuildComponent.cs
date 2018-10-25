using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildComponent : Scalable
{
	// Use this for initialization
	void Start ()
    {
        Initialise(false, false, false, false, false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateScaling(2);
	}
}
