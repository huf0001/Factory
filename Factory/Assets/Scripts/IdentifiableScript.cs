using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifiableScript : MonoBehaviour
{
    [SerializeField] private bool isPlayer = false;
    private List<Identifier> identifiers = new List<Identifier>();

    void Start()
    {
        AddIdentifier(Identifier.HasNotBeenLoadedInBuildZoneYet);
    }

    public bool IsPlayer
    {
        get
        {
            return isPlayer;
        }
    }

    public bool HasIdentifier(Identifier id)
    {
        bool result = false;

        if (identifiers.Contains(id))
        {
            result = true;
        }

        return result;
    }

    public void AddIdentifier(Identifier id)
    {
        if (!identifiers.Contains(id))
        {
            identifiers.Add(id);
        }
    }

    public void RemoveIdentifier(Identifier id)
    {
        if (identifiers.Contains(id))
        {
            identifiers.Remove(id);
        }
    }

    /*private void OnDestroy()
    {
        Debug.Log(this + " is destroyed");   
    }*/
}
