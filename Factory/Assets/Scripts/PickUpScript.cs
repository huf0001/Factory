using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PickUpScript: MonoBehaviour
{
    [SerializeField] private GameObject leftHandGuide;
    [SerializeField] private GameObject rightHandGuide;
    [SerializeField] private Camera myCamera;

    private IdentifiableScript leftIDs;
    private IdentifiableScript rightIDs;

    private GameObject movingInLeft = null;
    private GameObject movingInRight = null;

    private bool leftHandInput = false;
    private bool rightHandInput = false;

    [SerializeField] private float throwForce = 5f;
    [SerializeField] private Vector3 throwOffset;
    [SerializeField] private Transform headDirection;

    private GameControllerScript gameController = null;

    [SerializeField] private AudioClip pickUpSound;
    [SerializeField] private AudioClip dropSound;

    //[SerializeField] private AudioClip throwSound;
    //[SerializeField] private AudioClip toggleThrowSound;
    //[SerializeField] private bool throwOn = false;
    //private bool toggleThrow = false;

    private AudioSource audioSource = new AudioSource();

    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();

        leftIDs = leftHandGuide.GetComponent<IdentifiableScript>();
        rightIDs = rightHandGuide.GetComponent<IdentifiableScript>();

        leftIDs.AddIdentifier(Identifier.HandEmpty);
        rightIDs.AddIdentifier(Identifier.HandEmpty);

        gameController = GameObject.Find("GameController").GetComponent<GameControllerScript>();

        if (gameController == null)
        {
            Debug.Log("Why is there no object in the scene named GameController? There needs to be an object with a GameControllerScript called" +
                " 'GameController'. Fix it. NOW!!");
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
        if (gameController.GetButton("LeftArm"))
        {
            if (IsEmpty(Hand.Left))
            {
                HandlePickUp(Hand.Left);
            }
        }
        else if (IsHolding(Hand.Left))
        {
            HandleDrop(Hand.Left, movingInLeft);
        }

        if (gameController.GetButton("RightArm"))
        {
            if (IsEmpty(Hand.Right))
            {
                HandlePickUp(Hand.Right);
            }
        }
        else if (IsHolding(Hand.Right))
        {
            HandleDrop(Hand.Right, movingInRight);
        }

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

    private void HandlePickUp(Hand hand)
    {
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject item = hit.transform.gameObject;
            bool handledClick = false;

            if (item.GetComponent<AttachableScript>() != null)
            {
                item.GetComponent<AttachableScript>().HandlePickUp(hand);
                handledClick = true;
            }
            else if (item.GetComponent<MovableScript>() != null)
            {
                item.GetComponent<MovableScript>().HandlePickUp(hand);
                handledClick = true;
            }

            if (handledClick)
            {
                PlaySoundEffect(pickUpSound);

                if (hand == Hand.Left)
                {
                    movingInLeft = item;
                }
                else
                {
                    movingInRight = item;
                }

                ChangeIDsFromPickUp(hand);
            }
        }
    }

    private void HandleDrop(Hand hand, GameObject movingInHand)
    {
        bool handledClick = false;
        AttachableScript attachable = null;
        MovableScript movable = null;
        GameObject item = null;

        if (movingInHand.GetComponent<AttachableScript>() != null)
        {
            attachable = movingInHand.GetComponent<AttachableScript>();
        }
        else if (movingInHand.GetComponent<MovableScript>() != null)
        {
            movable = movingInHand.GetComponent<MovableScript>();
        }

        if (attachable != null)
        {
            attachable.HandleDrop(hand);
            handledClick = true;
        }
        else if (movable != null)
        {
            movable.HandleDrop(hand);
            handledClick = true;
        }

        if (handledClick)
        {
            item = movingInHand;

            if (hand == Hand.Left)
            {
                movingInLeft = null;
            }
            else
            {
                movingInRight = null;
            }

            /*if (throwOn)
            {
                ThrowObject(item, hand);
                PlaySoundEffect(throwSound);
            }
            else
            {*/
                PlaySoundEffect(dropSound);
            //}

            ChangeIDsFromDrop(hand);
        }
    }

    /*private void ThrowObject(GameObject projectile, Hand hand)
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
    }*/

    private void PlaySoundEffect(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }
}
