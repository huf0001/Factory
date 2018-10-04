using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PickUpScript: MonoBehaviour
{
    //Script needs to know which player it is
    private int playerNumber = 0;

    [SerializeField] private GameObject leftHandGuide;
    [SerializeField] private GameObject rightHandGuide;
    private Vector3 leftHandStartPoint;
    private Vector3 rightHandStartPoint;
    
    private IdentifiableScript leftIDs;
    private IdentifiableScript rightIDs;

    private GameObject movingInLeft = null;
    private GameObject movingInRight = null;

    private GameControllerScript gameController = null;

    [SerializeField] private AudioClip pickUpSound;
    [SerializeField] private AudioClip dropSound;

    [SerializeField] private float throwForce = 5f;
    [SerializeField] private Vector3 throwOffset;
    [SerializeField] private Transform headDirection;
    [SerializeField] private AudioClip throwSound;
    private bool throwOn = false;

    private AudioSource audioSource = new AudioSource();

    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();

        leftHandStartPoint = leftHandGuide.transform.localPosition;
        rightHandStartPoint = rightHandGuide.transform.localPosition;

        leftIDs = leftHandGuide.GetComponent<IdentifiableScript>();
        rightIDs = rightHandGuide.GetComponent<IdentifiableScript>();

        leftIDs.AddIdentifier(Identifier.HandEmpty);
        leftIDs.AddIdentifier(Identifier.Hand);
        rightIDs.AddIdentifier(Identifier.HandEmpty);
        rightIDs.AddIdentifier(Identifier.Hand);

        gameController = GameObject.Find("GameController").GetComponent<GameControllerScript>();

        if (gameController == null)
        {
            Debug.Log("Why is there no object in the scene named GameController? There needs to be an object with a GameControllerScript called" +
                " 'GameController'. Fix it. NOW!!");
        }
        else
        {
            playerNumber = gameController.GetPlayerNumber(this.gameObject);
        }
    }

    public bool ThrowOn
    {
        get
        {
            return throwOn;
        }

        set
        {
            throwOn = value;
        }
    }

    private void ChangeIDsFromPickUp(Hand h)
    {
        if (h == Hand.Left)
        {
            leftIDs.RemoveIdentifier(Identifier.HandEmpty);
            leftIDs.AddIdentifier(Identifier.HandHolding);
        }
        else if (h == Hand.Right)
        {
            rightIDs.RemoveIdentifier(Identifier.HandEmpty);
            rightIDs.AddIdentifier(Identifier.HandHolding);
        }
    }

    private void ChangeIDsFromDrop(Hand h)
    {
        if (h == Hand.Left)
        {
            leftIDs.RemoveIdentifier(Identifier.HandHolding);
            leftIDs.AddIdentifier(Identifier.HandEmpty);
        }
        else if (h == Hand.Right)
        {
            rightIDs.RemoveIdentifier(Identifier.HandHolding);
            rightIDs.AddIdentifier(Identifier.HandEmpty);
        }
    }

    public bool IsEmpty(Hand h)
    {
        if (h == Hand.Left)
        {
            return leftIDs.HasIdentifier(Identifier.HandEmpty);
        }
        else 
        {
            return rightIDs.HasIdentifier(Identifier.HandEmpty);
        }
    }

    public bool IsHolding(Hand h)
    {
        if (h == Hand.Left)
        {
            return leftIDs.HasIdentifier(Identifier.HandHolding);
        }
        else 
        {
            return rightIDs.HasIdentifier(Identifier.HandHolding);
        }
    }

    private void Update()
    {
        if (gameController.GetButton(playerNumber, "LeftArm"))
        {
            if (IsEmpty(Hand.Left))
            {
                DetectObjectsForPickUp(Hand.Left);
            }
        }
        else if (IsHolding(Hand.Left))
        {
            HandleDrop(Hand.Left, movingInLeft);
        }

        if (gameController.GetButton(playerNumber, "RightArm"))
        {
            if (IsEmpty(Hand.Right))
            {
                DetectObjectsForPickUp(Hand.Right);
            }
        }
        else if (IsHolding(Hand.Right))
        {
            HandleDrop(Hand.Right, movingInRight);
        }

        UpdateHandGuidePosition();

        /*if (toggleThrow)
        {
            throwOn = !throwOn;
            toggleThrow = false;
            PlaySoundEffect(toggleThrowSound);
        }
        else
        {
            toggleThrow = gameController.GetButtonDown("ToggleThrow");
        }*/
    }

    private void UpdateHandGuidePosition()
    {
        if (leftHandGuide.transform.localPosition != leftHandStartPoint)
        {
            leftHandGuide.transform.localPosition = leftHandStartPoint;
        }

        if (rightHandGuide.transform.localPosition != rightHandStartPoint)
        {
            rightHandGuide.transform.localPosition = rightHandStartPoint;
        }
    }

    private Vector3 GetHandColliderPosition(Hand hand)
    {
        if (hand == Hand.Left)
        {
            return leftHandGuide.transform.position;
        }
        else
        {
            return rightHandGuide.transform.position;
        }
    }

    private float GetHandColliderRadius(Hand hand)
    {
        if (hand == Hand.Left)
        {
            return leftHandGuide.GetComponent<SphereCollider>().radius;
        }
        else
        {
            return rightHandGuide.GetComponent<SphereCollider>().radius;
        }
    }

    private void DetectObjectsForPickUp(Hand hand)
    {
        Vector3 center;
        float radius = 0f;

        center = GetHandColliderPosition(hand);
        radius = GetHandColliderRadius(hand);

        Collider[] colliders = Physics.OverlapSphere(center, radius);

        if (colliders.Length > 0)
        {
            GetMovableObjectsForPickUp(hand, colliders);
        }
    }

    private void GetMovableObjectsForPickUp(Hand hand, Collider[] colliders)
    {
        List<GameObject> movableItems = new List<GameObject>();

        foreach (Collider c in colliders)
        {
            if (c.gameObject.GetComponent<MovableScript>() != null)
            {
                if (!c.gameObject.GetComponent<IdentifiableScript>().HasIdentifier(Identifier.Built))
                {
                    movableItems.Add(c.gameObject);
                }
            }
        }

        if (movableItems.Count > 0)
        {
            SelectObjectForPickUp(hand, movableItems);
        }
    }

    private void SelectObjectForPickUp(Hand hand, List<GameObject> items)
    {
        GameObject item = null;
        float distance = GetHandColliderRadius(hand) + 1f;
        float tempDistance = 0f;

        foreach (GameObject i in items)
        {
            tempDistance = Vector3.Distance(i.transform.position, GetHandColliderPosition(hand));

            if (tempDistance < distance)
            {
                distance = tempDistance;
                item = i;
            }
        }
        
        if (item != null)
        {
            HandlePickUp(hand, item);
        }
    }

    private void HandlePickUp(Hand hand, GameObject item)
    {
        if (item.GetComponent<MovableScript>() != null)
        {
            item.GetComponent<MovableScript>().HandlePickUp(playerNumber, hand);
            PlaySoundEffect(pickUpSound);
            ChangeIDsFromPickUp(hand);

            if (hand == Hand.Left)
            {
                movingInLeft = item;
            }
            else
            {
                movingInRight = item;
            }
        }
    }

    private void HandleDrop(Hand hand, GameObject movingInHand)
    {
        MovableScript movable = null;
        GameObject item = null;

        if (movingInHand.GetComponent<MovableScript>() != null)
        {
            movable = movingInHand.GetComponent<MovableScript>();
            movable.HandleDrop(playerNumber, hand);
            item = movingInHand;
            ChangeIDsFromDrop(hand);

            if (hand == Hand.Left)
            {
                movingInLeft = null;
            }
            else
            {
                movingInRight = null;
            }

            if (throwOn)
            {
                ThrowObject(item, hand);
                PlaySoundEffect(throwSound);
            }
            else
            {
                PlaySoundEffect(dropSound);
            }
        }
    }

    private void ThrowObject(GameObject projectile, Hand hand)
    {
        // Adjusting the throwOffset for the hand that's doing the throwing
        if (hand == Hand.Left)
        {
            throwOffset.x = 1f;
        }
        else
        {
            throwOffset.x = -1f;
        }
        
        //transforms the instantiate position into world space based on the head rotation
        Vector3 origin = headDirection.TransformDirection(throwOffset);

        //adds force to the objecct
        if (projectile.GetComponent<Rigidbody>() != null)
        {
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.AddForce(origin * throwForce, ForceMode.Impulse);
        }
        else
            Debug.LogError("The gameobject you are trying to throw does not have a rigidbody");
    }

    private void PlaySoundEffect(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }
}
