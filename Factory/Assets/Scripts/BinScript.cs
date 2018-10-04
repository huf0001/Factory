using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinScript : MonoBehaviour
{
    // Should be physically impossible for anything to enter the bin except via falling in, so it can't be triggered from the sides.
    // Will check that the item in question is not the player though.
    private void OnTriggerStay(Collider other)
    {
        bool okToDestroy = true;
        IdentifiableScript ids = other.gameObject.GetComponent<IdentifiableScript>();

        if (ids != null)
        {
            if (ids.HasIdentifier(Identifier.Hand) || ids.HasIdentifier(Identifier.PlayerMoving))
            {
                okToDestroy = false;
            }
        }
        
        if (okToDestroy)
        {
            Destroy(other.gameObject.GetComponent<MovableScript>());
            //Destroy(other.gameObject.GetComponent<AttachScript>());
            Destroy(other.gameObject);
        }
    }
}
