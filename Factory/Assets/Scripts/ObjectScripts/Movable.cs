using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : Identifiable
{
    [SerializeField] private Identifier uniqueID = Identifier.Attachable;
    [SerializeField] private float lifespan = 40f;

    private Spawner spawner;
    private InputController inputController = null;
    private List<GameObject> tempLeftParents = new List<GameObject>();
    private List<GameObject> tempRightParents = new List<GameObject>();
    private List<Transform> leftGuides = new List<Transform>();
    private List<Transform> rightGuides = new List<Transform>();
    private Rigidbody body;

    // Use this for initialization
    void Start ()
    {
        AddIdentifier(uniqueID);
        int playerCount = 0;
        body = this.gameObject.GetComponent<Rigidbody>();
        body.useGravity = true;
        inputController = GameObject.Find("GameControllerCamera").GetComponent<InputController>();

        if (inputController != null)
        {
            playerCount = inputController.PlayerCount;

            for (int i = 0; i < playerCount; i++)
            {
                tempLeftParents.Add(inputController.LeftHand(i + 1));
                leftGuides.Add(tempLeftParents[i].transform);

                tempRightParents.Add(inputController.RightHand(i + 1));
                rightGuides.Add(tempRightParents[i].transform);
            }
        }
    }

    public Spawner Spawner
    {
        get
        {
            return spawner;
        }

        set
        {
            spawner = value;
        }
    }

    void Update()
    {
        if (!HasIdentifier(Identifier.PlayerMoving))
        {
            lifespan -= Time.deltaTime;
        }

        if (lifespan <= 0)
        {
            Destroy(this.gameObject);
            Destroy(this);
        }
    }

    public void HandlePickUp(int p, Hand h)
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
    }

    public void HandleDrop(int p, Hand h)
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
        this.gameObject.layer = 0;
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

    private void OnDestroy()
    {
        spawner.ReStock(uniqueID);
    }
}
