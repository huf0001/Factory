using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPart : Scalable
{
    [SerializeField] private GameObject greyGhost;
    private bool revealed = false;

	// Use this for initialization
	private void Start ()
    {
        Initialise(false, false, false, false, true);
	}

    // Update is called once per frame
    private void Update()
    {
        if (revealed && !Expanding)
        {
            Destroy(greyGhost);
            greyGhost = null;
            revealed = false;
        }
    }

    private void FixedUpdate ()
    {
        UpdateScaling(2);
	}

    public void Reveal()
    {
        Expanding = true;
        revealed = true;
    }
}
