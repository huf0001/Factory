using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSchemaScript : MonoBehaviour
{
    [SerializeField] private BuildZoneScript buildZone;
    [SerializeField] private Transform buildPoint;
    [SerializeField] private GameObject ghostObject;
    private GhostScript ghostScript = null;
    [SerializeField] Identifier[] components;
    [SerializeField] private GameObject finalObject;

    private List<Identifier> pendingComponents = new List<Identifier>();
    private List<Identifier> loadedComponents = new List<Identifier>();

    // Use this for initialization
    void Start()
    {
        if (buildPoint == null)
        {
            Debug.Log("Warning: Couldn't find the build point. It should be childed to the build" +
                "zone that this schema is attached to.");
        }

        if (buildZone == null)
        {
            Debug.Log("Warning: Couldn't find the build zone. Schemas need build zones.");
        }

        ghostScript = ghostObject.GetComponent<GhostScript>();

        foreach (Identifier i in components)
        {
            pendingComponents.Add(i);
        }
    }

    public void ActivateGhost()
    {
        ghostObject = Instantiate(ghostObject);
        CentreInBuildZone(ghostObject);
        ghostScript = ghostObject.GetComponent<GhostScript>();
    }

    private void CentreInBuildZone(GameObject item)
    {
        if (buildPoint != null)
        {
            item.transform.position = buildPoint.position;
            item.transform.rotation = buildPoint.rotation;
        }
    }

    public bool BelongsToSchema(IdentifiableScript ids)
    {
        foreach (Identifier i in components)
        {
            if (ids.HasIdentifier(i))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsLoaded(IdentifiableScript ids)
    {
        foreach (Identifier id in components)
        {
            if (ids.HasIdentifier(id))
            {
                foreach (Identifier i in loadedComponents)
                {
                    if (i == id)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void HandleValidObject(GameObject item)
    {
        LoadObject(item);

        if (pendingComponents.Count == 0)
        {
            Build();
        }
    }

    private void LoadObject(GameObject item)
    {
        IdentifiableScript itemIds = item.GetComponent<IdentifiableScript>();

        if (!itemIds.HasIdentifier(Identifier.Attached))
        {
            foreach (Identifier i in pendingComponents)
            {
                if (itemIds.HasIdentifier(i))
                {
                    DestroyComponentObject(item);
                    ghostScript.Reveal(i);
                    pendingComponents.Remove(i);
                    loadedComponents.Add(i);
                    this.transform.parent.gameObject.GetComponent<BuildZoneScript>().PlayLoadedSound();

                    return;
                }
            }
        }
    }

    private void DestroyComponentObject(GameObject item)
    {
        Destroy(item.GetComponent<MovableScript>());
        Destroy(item.GetComponent<IdentifiableScript>());
        Destroy(item);
    }

    private void Build()
    {
        DestroyGhost();
        SpawnBuiltObject();
        buildZone.SchemaComplete(this);
    }

    private void DestroyGhost()
    {
        Destroy(ghostScript);
        Destroy(ghostObject);
    }

    private void SpawnBuiltObject()
    {
        GameObject spawning = Instantiate(finalObject);
        CentreInBuildZone(spawning);
        spawning.GetComponent<BuiltScript>().BuildZone = buildZone;
    }
}
