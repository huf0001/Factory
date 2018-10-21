using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    [SerializeField] private Transform endpoint;
    [SerializeField] private float speed;
    private float multiplier;

    private void OnTriggerStay(Collider other)
    {
        if (!CheckPlayerMoving(other.gameObject))
        {
            if (CheckPlayer(other.gameObject))
            {
                multiplier = 0.5f;
            }
            else
            {
                multiplier = 1f;
            }

            other.transform.position = Vector3.MoveTowards(other.transform.position, endpoint.position, speed * multiplier * Time.deltaTime);
        }

        //Note: attachment base objects seem to move extra fast;  might need to add a secondary endpoint object
        //for that object to move towards so that it gets kicked off the conveyor belt and doesn't knock other objects off 
    }

    private bool CheckPlayerMoving(GameObject other)
    {
        bool result = false;
        Movable movable = other.gameObject.GetComponent<Movable>();

        if (movable != null)
        {
            if (movable.HasIdentifier(Identifier.PlayerMoving))
            {
                result = true;
            }     
        }

        return result;
    }

    private bool CheckPlayer(GameObject other)
    {
        if (other.tag == "Player")
        {
            return true;
        }

        return false;
    }
}
