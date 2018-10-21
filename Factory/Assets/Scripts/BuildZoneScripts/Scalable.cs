using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scalable : MonoBehaviour
{
    private float scaleTick = 0.05f;
    private float scaleTimer = 0f;
    private int scaleCount = 0;
    private float rotationSpeed = 1.5f;
    private float rotationFactorWhenScaling = 10f;

    private Vector3 startScale;
    private bool expanding = true;
    private bool shrinking = false;
    private bool dropping = false;

    // Use this for initialization
    protected void Initialise ()
    {
        startScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    public bool Shrinking
    {
        get
        {
            return shrinking;
        }

        set
        {
            shrinking = value;
        }
    }

    public bool Dropping
    {
        get
        {
            return dropping;
        }

        set
        {
            dropping = value;
        }
    }

    protected void Rotate()
    {
        Rotate(1f);
    }

    protected void Rotate(float speedFactor)
    {
        transform.Rotate(new Vector3(0f, rotationSpeed * speedFactor), Space.World);
    }

    protected void CheckScaling()
    {
        if (expanding)
        {
            Expand();
        }
        else if (shrinking)
        {
            Shrink();
        }
        else if (dropping)
        {
            Drop();
        }
    }

    private void Expand()
    {
        scaleTimer += Time.deltaTime;

        if (scaleTimer > (scaleCount + 1) * scaleTick)
        {
            scaleCount += 1;
            transform.localScale += startScale * scaleTick;

            Rotate(rotationFactorWhenScaling);
        }

        if (transform.localScale.x > startScale.x)
        {
            transform.localScale = startScale;
            scaleCount = 0;
            scaleTimer = 0f;
            expanding = false;
        }
    }

    private void Shrink()
    {
        if (!FinishedShrinking())
        {
            scaleTimer += Time.deltaTime;

            if (scaleTimer > (scaleCount + 1) * scaleTick)
            {
                scaleCount += 1;
                transform.localScale -= startScale * scaleTick;
                Rotate(rotationFactorWhenScaling);
            }
        }
    }

    protected bool FinishedShrinking()
    {
        if (shrinking && (scaleCount > (1 / scaleTick)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void Failed()
    {
        Dropping = true;
    }

    private void Drop()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - scaleTick, transform.position.z);

        if (FinishedDropping())
        {
            dropping = false;
        }
    }

    protected bool FinishedDropping()
    {
        if (transform.position.y <= -2)
        {
            return true;
        }

        return false;
    }
}
