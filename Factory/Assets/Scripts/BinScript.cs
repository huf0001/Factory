using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinScript : MonoBehaviour
{
    [SerializeField] private AudioClip destroySound;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource.clip = destroySound;
    }

    // Should be physically impossible for anything to enter the bin except via falling in, so it can't be triggered from the sides.
    // Will check that the item in question is not the player though.
    private void OnTriggerStay(Collider other)
    {
        // bool okToDestroy = true;
        // float increment = -1f;
        IdentifiableScript ids = other.gameObject.GetComponent<IdentifiableScript>();

        if (other.gameObject.tag == "Player")
        {
            return;
        }

        if (ids != null)
        {
            if (ids.HasIdentifier(Identifier.Hand) || ids.HasIdentifier(Identifier.PlayerMoving))
            {
                return;
            }
        }
        
        Destroy(other.gameObject.GetComponent<MovableScript>());
        Destroy(other.gameObject);
        audioSource.Play();
    }
}
