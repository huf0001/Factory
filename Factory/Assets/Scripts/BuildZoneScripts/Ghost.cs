using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Scalable
{
    [System.Serializable]
    public class GhostIdentifierPair
    {
        [SerializeField] private GameObject ghost;
        [SerializeField] private Identifier identifier;

        public GameObject Ghost
        {
            get
            {
                return ghost;
            }
        }

        public Identifier Identifier
        {
            get
            {
                return identifier;
            }
        }
    }

    [SerializeField] GhostIdentifierPair[] ghostComponents;
    private Schema schema = null;

    private void Start()
    {
        foreach (GhostIdentifierPair p in ghostComponents)
        {
            p.Ghost.SetActive(false);
        }

        Initialise();
    }

    public Schema Schema
    {
        get
        {
            return Schema;
        }

        set
        {
            schema = value;
        }
    }

    public void Reveal(Identifier i)
    {
        foreach (GhostIdentifierPair p in ghostComponents)
        {
            if (p.Identifier == i)
            {
                p.Ghost.SetActive(true);
            }
        }
    }

    public override void Failed()
    {
        schema = null;
        base.Failed();
    }

    private void Update()
    {
        Rotate();
        CheckScaling();

        if (FinishedShrinking() && schema != null)
        {
            schema.SchemaComplete();
        }
        else if (FinishedDropping())
        {
            DestroyGhost();
        }
    }

    public void DestroyGhost()
    {
        Destroy(this.gameObject);
        Destroy(this);
    }
}
