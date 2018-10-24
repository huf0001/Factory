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
    private bool expanding;
    private bool shrinking;
    private bool dropping;
    private bool rotating;

    // Use this for initialization
    protected void Initialise(bool minimise)
    {
        Initialise(true, false, false, true, minimise);
    }

    protected void Initialise (bool expand, bool shrink, bool drop, bool rotate, bool minimise)
    {
        startScale = transform.localScale;
        expanding = expand;
        shrinking = shrink;
        dropping = drop;
        rotating = rotate;

        if (minimise)
        {
            transform.localScale = Vector3.zero;
        }
    }

    public bool Expanding
    {
        get
        {
            return expanding;
        }

        set
        {
            expanding = value;
        }
    }

    public bool Rotating
    {
        get
        {
            return rotating;
        }

        set
        {
            rotating = value;
        }
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

    protected void UpdateScaling()
    {
        UpdateScaling(1);
    }

    protected void UpdateScaling(int n)
    {
        if (PlayerPrefs.GetString("active") != "false")
        {
            for (int i = 0; i < n; i++)
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

                if (rotating)
                {
                    if (expanding || shrinking)
                    {
                        Rotate(rotationFactorWhenScaling);
                    }
                    else
                    {
                        Rotate(1);
                    }
                }
            }
        }
    }

    private void Expand()
    {
        scaleTimer += Time.deltaTime;

        if (scaleTimer > (scaleCount + 1) * scaleTick)
        {
            scaleCount += 1;
            transform.localScale += startScale * scaleTick;
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
                Vector3 newScale = transform.localScale - (startScale * scaleTick);

                if (newScale.x < 0 || newScale.y < 0 || newScale.z < 0)
                {
                    transform.localScale = Vector3.zero;
                }
                else
                {
                    transform.localScale = newScale;
                }

                scaleCount += 1;
            }
        }
    }

    protected void Rotate(float speedFactor)
    {
        transform.Rotate(new Vector3(0f, rotationSpeed * speedFactor), Space.World);
    }

    private void Drop()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - scaleTick, transform.position.z);

        if (FinishedDropping())
        {
            dropping = false;
        }
    }

    public bool FinishedShrinking()
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

    protected bool FinishedDropping()
    {
        if (transform.position.y <= -2)
        {
            return true;
        }

        return false;
    }
}
