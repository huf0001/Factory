﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schema : MonoBehaviour
{
    [SerializeField] private BuildZone buildZone;
    [SerializeField] private Transform buildPoint;
    [SerializeField] private GameObject ghostObjectPrefab;
    private GameObject ghostObject = null;
    private Ghost ghost = null;
    [SerializeField] Identifier[] components;
    [SerializeField] private GameObject finalObject;
    private Built built = null;

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

        foreach (Identifier i in components)
        {
            pendingComponents.Add(i);
        }
    }

    public void SpawnGhost()
    {
        //Debug.Log("SpawningGhost");
        ghostObject = Instantiate(ghostObjectPrefab);
        CentreInBuildZone(ghostObject);
        ghost = ghostObject.GetComponent<Ghost>();
        ghost.Schema = this;
    }

    private void CentreInBuildZone(GameObject item)
    {
        if (buildPoint != null)
        {
            item.transform.position = buildPoint.position;
            item.transform.rotation = buildPoint.rotation;
        }
    }

    public bool BelongsToSchema(Identifiable ids)
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

    public bool IsLoaded(Identifiable ids)
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

    public void HandleValidObject(GameObject item, int zoneNumber)
    {
        LoadObject(item);

        if (pendingComponents.Count == 0)
        {
            buildZone.PlayBuiltSound();
            IncrementBuildCount(zoneNumber);

            if (ghost != null)
            {
                ShrinkGhost();
            }
        }
    }

    private void LoadObject(GameObject item)
    {
        Identifiable itemIds = item.GetComponent<Identifiable>();
        Scalable itemScaler = item.GetComponent<Scalable>();

        foreach (Identifier i in pendingComponents)
        {
            if (itemIds.HasIdentifier(i) && itemScaler != null)
            {
                MoveIntoBuildZone(item);

                if (!itemScaler.Shrinking)
                {
                    itemScaler.Shrinking = true;
                    itemScaler.Rotating = true;
                }
                else if (itemScaler.FinishedShrinking())
                {
                    //Debug.Log("Build component finished shrinking");
                    DestroyComponentObject(item);
                    ghost.Reveal(i);
                    pendingComponents.Remove(i);
                    loadedComponents.Add(i);
                    this.transform.parent.gameObject.GetComponent<BuildZone>().PlayLoadedSound();
                }

                return;
            }
        }
    }

    private void MoveIntoBuildZone(GameObject item)
    {
        float increment = 0.05f;

        item.transform.position = Vector3.MoveTowards(item.transform.position, buildPoint.transform.position, increment);
    }

    private void DestroyComponentObject(GameObject item)
    {
        Destroy(item.GetComponent<BuildComponent>());
        Destroy(item.GetComponent<Scalable>());
        Destroy(item.GetComponent<Movable>());
        Destroy(item.GetComponent<Identifiable>());
        Destroy(item);
    }

    public void IncrementBuildCount(int n)
    {
        if (n == 1)
        {
            GameObject.Find("GameControllerCamera").GetComponent<GameController>().P1BuiltObjectShowing = true;
            GameObject.Find("GameControllerCamera").GetComponent<GameController>().IncrementPlayer1BuildCount();
        }
        else if (n == 2)
        {
            GameObject.Find("GameControllerCamera").GetComponent<GameController>().P2BuiltObjectShowing = true;
            GameObject.Find("GameControllerCamera").GetComponent<GameController>().IncrementPlayer2BuildCount();
        }
        else
        {
            Debug.Log("Invalid build zone number");
        }
    }

    private void ShrinkGhost()
    {
        ghost.Shrinking = true;
    }

    public void SchemaComplete()
    {
        ghost.DestroyGhost();
        ghost = null;
        built = SpawnBuiltObject();
    }

    private Built SpawnBuiltObject()
    {
        Built spawning = Instantiate(finalObject).GetComponent<Built>();
        CentreInBuildZone(spawning.gameObject);
        spawning.Schema = this;
        spawning.BuildZone = buildZone;
        return spawning;
    }

    public void Failed()
    {
        if (ghost != null)
        {
            ghost.Dropping = true;
            ghost.Schema = null;
        }

        if (built != null)
        {
            built.Dropping = true;
            built.Schema = null;
        }
    }

    public void ResetSchema()
    {
        int count = loadedComponents.Count;

        for (int i = 0; i < count; i++)
        {
            pendingComponents.Add(loadedComponents[0]);
            loadedComponents.Remove(loadedComponents[0]);
        }

        built = null;
    }
}
