using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableScript : MonoBehaviour
{
    private float scaleTick = 0.05f;
    private float scaleTimer = 0f;
    private int scaleCount = 0;
    private float rotationSpeed = 1.5f;
    private float rotationFactorWhenScaling = 10f;

    private Vector3 startScale;
    private bool expanding = true;
    private bool shrinking = false;

    // Use this for initialization
    protected void Initialise ()
    {
        startScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    protected bool Shrinking
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
}
