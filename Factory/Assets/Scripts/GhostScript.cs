using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
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

    private void Start()
    {
        foreach (GhostIdentifierPair p in ghostComponents)
        {
            p.Ghost.SetActive(false);
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
}
