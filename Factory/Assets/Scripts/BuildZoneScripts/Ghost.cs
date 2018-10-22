using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Scalable
{
    [System.Serializable]
    public class GhostIdentifierPair
    {
        [SerializeField] private GhostPart ghostPart;
        [SerializeField] private Identifier identifier;

        public GhostPart GhostPart
        {
            get
            {
                return ghostPart;
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
        Initialise(true);
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
                p.GhostPart.Reveal();
            }
        }
    }

    private void Update()
    {
        UpdateScaling();

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
