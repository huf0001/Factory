using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPart : Scalable
{
    [SerializeField] private GameObject greyGhost;
    private bool revealed = false;

	// Use this for initialization
	void Start ()
    {
        Initialise(false, false, false, false, true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateScaling(2);

        if (revealed && !Expanding)
        {
            Destroy(greyGhost);
            greyGhost = null;
            revealed = false;
        }
	}

    public void Reveal()
    {
        Expanding = true;
        revealed = true;
    }
}
