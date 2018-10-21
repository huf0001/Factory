using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
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
        Identifiable ids = other.gameObject.GetComponent<Identifiable>();

        if (other.gameObject.tag == "Player")
        {
            return;
        }

        if (ids != null)
        {
            if (ids.HasIdentifier(Identifier.PlayerMoving))
            {
                return;
            }
        }
        
        Destroy(other.gameObject.GetComponent<Movable>());
        Destroy(other.gameObject);
        audioSource.Play();
    }
}
