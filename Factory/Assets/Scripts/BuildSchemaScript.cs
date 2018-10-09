using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSchemaScript : MonoBehaviour
{
    [SerializeField] private string buildPointName = "BuildPoint";
    [SerializeField] private float distanceLimit;
    [SerializeField] private List<Identifier> components = new List<Identifier>();
    [SerializeField] private GameObject finalObject;

    private List<Identifier> pendingComponents = new List<Identifier>();
    private Dictionary<Identifier, GameObject> loadedComponents = new Dictionary<Identifier, GameObject>();
    private Transform buildPoint = null;

    // Use this for initialization
    void Start()
    {
        buildPoint = this.transform.parent.Find(buildPointName);

        if (buildPoint == null)
        {
            Debug.Log("Warning: Couldn't find the game object '" + buildPointName + "'. " + buildPointName + " should be childed to the build" +
                "zone that this schema is attached to.");
        }

        foreach (Identifier i in components)
        {
            pendingComponents.Add(i);
        }
    }

    public bool BelongsToSchema(GameObject item)
    {
        IdentifiableScript ids = null;
        ids = item.GetComponent<IdentifiableScript>();

        if (ids != null)
        {
            return BelongsToSchema(ids);
        }

        return false;
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
        foreach (Identifier i in components)
        {
            if (ids.HasIdentifier(i))
            {
                foreach (KeyValuePair<Identifier, GameObject> kvp in loadedComponents)
                {
                    if (kvp.Key == i)
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
            this.transform.parent.GetComponent<BuildZoneScript>().DeleteSchema(this);
            Destroy(this);
        }
    }

    private void LoadObject(GameObject item)
    {
        IdentifiableScript itemIds = item.GetComponent<IdentifiableScript>();

        if (!itemIds.HasIdentifier(Identifier.Attached))
        {
            foreach (Identifier i in components)
            {
                if (itemIds.HasIdentifier(i))
                {
                    if (item.GetComponent<MovableScript>() != null)
                    {
                        item.GetComponent<MovableScript>().Schema = this;
                    }

                    pendingComponents.Remove(i);
                    loadedComponents.Add(i, item);
                    itemIds.RemoveIdentifier(Identifier.HasNotBeenLoadedInBuildZoneYet);

                    this.transform.parent.gameObject.GetComponent<BuildZoneScript>().PlayLoadedSound();

                    MoveTowardsBuildPoint(item);

                    return;
                }
            }
        }
    }

    private void MoveTowardsBuildPoint(GameObject item)
    {
        float increment = 0.1f;
        float distance = 0f;

        do
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, buildPoint.transform.position, increment);
            distance = Vector3.Distance(item.transform.position, buildPoint.position);
        } while (distance > distanceLimit);
    }

    private bool CheckIsLoaded(GameObject item)
    {
        foreach (KeyValuePair<Identifier, GameObject> p in loadedComponents)
        {
            if (p.Value == item)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveObject(GameObject item)
    {
        foreach (KeyValuePair<Identifier, GameObject> p in loadedComponents)
        {
            if (p.Value == item)
            {
                pendingComponents.Add(p.Key);
                loadedComponents.Remove(p.Key);
                p.Value.GetComponent<IdentifiableScript>().RemoveIdentifier(Identifier.InBuildZone);                
                return;
            }
        }
    }

    private void Build()
    {
        DestroyComponentObjects();
        SpawnBuiltObject();
        //Particle effect
    }


    private void DestroyComponentObjects()
    {
        List<GameObject> items = new List<GameObject>();
        List<Identifier> keys = new List<Identifier>();

        foreach (KeyValuePair<Identifier, GameObject> p in loadedComponents)
        {
            items.Add(p.Value);
            keys.Add(p.Key);
        }

        do
        {
            loadedComponents.Remove(keys[0]);
            Destroy(items[0].GetComponent<MovableScript>());
            Destroy(items[0].GetComponent<IdentifiableScript>());
            Destroy(items[0]);
            items.Remove(items[0]);
            keys.Remove(keys[0]);
        } while (items.Count > 0);
    }

    private void SpawnBuiltObject()
    {
        GameObject spawning = Instantiate(finalObject);
        CentreInBuildZone(spawning);
        spawning.GetComponent<BuiltScript>().BuildZone = this.transform.parent.GetComponent<BuildZoneScript>();
    }

    private void CentreInBuildZone(GameObject item)
    {
        if (buildPoint != null)
        {
            item.transform.position = buildPoint.position;
            item.transform.rotation = buildPoint.rotation;
        }
    }
}
