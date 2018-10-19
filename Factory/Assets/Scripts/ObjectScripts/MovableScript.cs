using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableScript : IdentifiableScript
{
    [SerializeField] private Identifier uniqueID = Identifier.Attachable;

    private GameControllerScript gameController = null;
    //private BuildSchemaScript schema = null;
    private List<GameObject> tempLeftParents = new List<GameObject>();
    private List<GameObject> tempRightParents = new List<GameObject>();
    private List<Transform> leftGuides = new List<Transform>();
    private List<Transform> rightGuides = new List<Transform>();
    private List<PickUpScript> hands = new List<PickUpScript>();
    private Rigidbody body;

    // Use this for initialization
    void Start ()
    {
        HandleStart();   
    }

    protected virtual void HandleStart()
    {
        AddIdentifier(uniqueID);
        int playerCount = 0;
        body = this.gameObject.GetComponent<Rigidbody>();
        body.useGravity = true;
        gameController = GameObject.Find("GameControllerCamera").GetComponent<GameControllerScript>();

        if (gameController != null)
        {
            playerCount = gameController.PlayerCount;

            for (int i = 0; i < playerCount; i++)
            {
                tempLeftParents.Add(gameController.LeftHand(i + 1));
                leftGuides.Add(tempLeftParents[i].transform);

                tempRightParents.Add(gameController.RightHand(i + 1));
                rightGuides.Add(tempRightParents[i].transform);

                hands.Add(tempLeftParents[i].GetComponentInParent<PickUpScript>());
            }
        }
    }

    /*public BuildSchemaScript Schema
    {
        get
        {
            return schema;
        }

        set
        {
            schema = value;
        }
    }*/

    public virtual void HandlePickUp(int p, Hand h)
    {
        body.useGravity = false;
        body.isKinematic = true;

        if (h == Hand.Left)
        {
            transform.position = leftGuides[p - 1].transform.position;
            transform.rotation = leftGuides[p - 1].transform.rotation;
            transform.parent = tempLeftParents[p - 1].transform;
        }
        else
        {
            transform.position = rightGuides[p - 1].transform.position;
            transform.rotation = rightGuides[p - 1].transform.rotation;
            transform.parent = tempRightParents[p - 1].transform;
        }

        AddIdentifier(Identifier.PlayerMoving);
        this.gameObject.layer = 2;

        //if (schema != null)
        //{
            //schema.RemoveObject(this.gameObject);
            //schema = null;
        //}
    }

    public virtual void HandleDrop(int p, Hand h)
    {
        body.useGravity = true;
        body.isKinematic = false;
        transform.parent = null;

        if (h == Hand.Left)
        {
            transform.position = leftGuides[p - 1].transform.position;
        }
        else
        {
            transform.position = rightGuides[p - 1].transform.position;
        }

        RemoveIdentifier(Identifier.PlayerMoving);
        AddIdentifier(Identifier.Dropped);
        this.gameObject.layer = 0;
    }

    void OnTriggerStay(Collider other)
    {
        HandleOnTriggerStay(other);
    }

    protected virtual void HandleOnTriggerStay(Collider other)
    {
        if (HasIdentifier(Identifier.Dropped))
        {
            RemoveIdentifier(Identifier.Dropped);
        }
    }

    protected Rigidbody Body
    {
        get
        {
            return body;
        }
        set
        {
            body = value;
        }
    }
}
