using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

[RequireComponent(typeof (CharacterController))]

public class PlayerController : MonoBehaviour
{
    //Script needs to know which player it is
    private int playerNumber = 0;
    
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody leftHand, rightHand;
    [SerializeField] private Transform leftTarget, rightTarget;
    [SerializeField] private float armSpeed = 150f;

    private bool movingRightArm = false;
    private bool movingLeftArm = false;
    
    [SerializeField] private float rotateSpeed = 0.15f;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float walkSensitivity = 0.1f;
    [SerializeField] private float consoleWalkSensitivityMultiplier = 10f;
    [SerializeField] private float jumpSpeed = 10;
    [SerializeField] private float stickToGroundForce = 10;
    [SerializeField] private float gravityMultiplier = 2;
    [SerializeField] private LerpControlledBob jumpBob = new LerpControlledBob();
    [SerializeField] private float stepInterval = 5;
    [SerializeField] private AudioClip[] footStepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip jumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip landSound;           // the sound played when character touches back on ground.

    private bool jump;
    private Vector3 walkInput;
    private Vector3 jumpMovement = Vector3.zero;
    private CharacterController characterController;
    private CollisionFlags collisionFlags;
    private bool previouslyGrounded;
    private float stepCycle;
    private float nextStep;
    private bool jumping;
    [SerializeField] private AudioSource feetAudioSource;
    private InputController inputController;

    // Use this for initialization
    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        stepCycle = 0f;
        nextStep = stepCycle/2f;
        jumping = false;

        inputController = GameObject.Find("GameControllerCamera").GetComponent<InputController>();

        if (inputController == null)
        {
            Debug.Log("Why is there no object in the scene named GameControllerCamera? There needs to be an object with a InputController script called" +
                " 'GameControllerCamera'. Fix it. NOW!!");
        }
        else
        {
            walkSensitivity = walkSensitivity * consoleWalkSensitivityMultiplier;
            playerNumber = inputController.GetPlayerNumber(this.gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // the jump state needs to read here to make sure it is not missed
        if (!jump)
        {
            jump = inputController.GetButtonDown(playerNumber, "Jump");
        }

        if (!previouslyGrounded && characterController.isGrounded)
        {
            StartCoroutine(jumpBob.DoBobCycle());
            PlayLandingSound();
            jumpMovement.y = 0f;
            jumping = false;
        }

        if (!characterController.isGrounded && !jumping && previouslyGrounded)
        {
            jumpMovement.y = 0f;
        }

        previouslyGrounded = characterController.isGrounded;

        //Gathering input for hand animation
        if (inputController.GetButton(playerNumber, "LeftArm"))
        {
            movingLeftArm = true;
        }

        if (inputController.GetButton(playerNumber, "RightArm"))
        {
            movingRightArm = true;
        }

        walkInput = GetWalkInput();

        //Updating the Robot so that it looks cool
        RotatePlayer(walkInput, rotateSpeed);
        UpdateWalkingAnimation(walkInput);
        UpdateArms();
    }

    private Vector3 GetWalkInput()
    {
        Vector3 input = new Vector3(inputController.GetAxis(playerNumber, "MoveHorizontal"), 0f, inputController.GetAxis(playerNumber, "MoveVertical"));

        // normalize input if it exceeds 1 in combined length:
        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }

        return input;
    }

    private void RotatePlayer(Vector3 movement, float speed)
    {
        if (movement.x != 0 || movement.z != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), speed);
        }
        else if (movement.x == 0 && movement.z == 0 && inputController.GetAxis(playerNumber, "LookHorizontal") != 0)
        {
            transform.Rotate(new Vector3(0f, inputController.GetAxis(playerNumber, "LookHorizontal"), 0f), Space.World);
        }
    }

    private void PlayLandingSound()
    {
        feetAudioSource.clip = landSound;
        feetAudioSource.Play();
        nextStep = stepCycle + .5f;
    }

    private void UpdateWalkingAnimation(Vector3 movement)
    {
        if ((movement.x != 0f) || (movement.z != 0f))
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }

    //Moves hands based on players head rotation 
    private void UpdateArms()
    {
        if (movingLeftArm)
        {
            leftHand.AddForce(leftTarget.forward * armSpeed, ForceMode.Acceleration);
            movingLeftArm = false;
            leftHand.velocity = Vector3.zero;
        }

        if (movingRightArm)
        {
            rightHand.AddForce(rightTarget.forward * armSpeed, ForceMode.Acceleration);
            movingRightArm = false;
            rightHand.velocity = Vector3.zero;
        }
    }

    // Note: google says FixedUpdate runs 0+ times per frame depending on how many physics frames
    // per second are set in the time settings, and how fast/slow the framerate is. As such, it's
    // better for physics operations as it will be executed in sync with the physics engine
    private void FixedUpdate()
    {
        MovePlayer(walkInput, characterController.transform.position);
        HandleJump();
    }

    private void MovePlayer(Vector3 movement, Vector3 position)
    {
        characterController.transform.position = new Vector3(position.x + (movement.x * walkSensitivity), position.y, position.z + (movement.z * walkSensitivity));
    }

    private void HandleJump()
    {
        if (characterController.isGrounded)
        {
            jumpMovement.y = -stickToGroundForce;

            if (jump)
            {
                jumpMovement.y = jumpSpeed;
                PlaySoundEffect(jumpSound);
                jump = false;
                jumping = true;
            }
        }
        else
        {
            jumpMovement += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
        }

        collisionFlags = characterController.Move(jumpMovement * Time.fixedDeltaTime);
        ProgressStepCycle(walkSpeed, walkInput);
    }

    private void PlaySoundEffect(AudioClip sound)
    {
        feetAudioSource.clip = sound;
        feetAudioSource.Play();
    }

    private void ProgressStepCycle(float speed, Vector3 movement)
    {
        if (characterController.velocity.sqrMagnitude > 0 && (movement.x != 0 || movement.z != 0))
        {
            stepCycle += (characterController.velocity.magnitude + speed) * Time.fixedDeltaTime;
        }

        if (!(stepCycle > nextStep))
        {
            return;
        }

        nextStep = stepCycle + stepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio()
    {
        if (!characterController.isGrounded)
        {
            return;
        }

        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, footStepSounds.Length);
        feetAudioSource.clip = footStepSounds[n];
        feetAudioSource.PlayOneShot(feetAudioSource.clip);

        // move picked sound to index 0 so it's not picked next time
        footStepSounds[n] = footStepSounds[0];
        footStepSounds[0] = feetAudioSource.clip;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() != null)
        {
            transform.Translate((transform.position - other.transform.position) * 0.03f, Space.World);
        }
    }
}