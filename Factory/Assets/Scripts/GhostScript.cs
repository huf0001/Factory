using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : ScalableScript
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
    private BuildSchemaScript schema = null;

    private void Start()
    {
        foreach (GhostIdentifierPair p in ghostComponents)
        {
            p.Ghost.SetActive(false);
        }

        Initialise();
    }

    private void Update()
    {
        Rotate();
        CheckScaling();

        if (FinishedShrinking())
        {
            schema.SchemaComplete();
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

    public void ToggleShrinking(BuildSchemaScript s)
    {
        Shrinking = true;
        schema = s;
    }
}
