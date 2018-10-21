using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class ItemWeightPair
    {
        [SerializeField] private GameObject item;
        [SerializeField] private int itemWeighting;

        public GameObject Item
        {
            get
            {
                return item;
            }
        }

        public int ItemWeighting
        {
            get
            {
                return itemWeighting;
            }
        }
    }

    [SerializeField] private float secBetweenSpawns = 2f;
    [SerializeField] ItemWeightPair[] easyItems;
    [SerializeField] ItemWeightPair[] mediumItems;
    [SerializeField] ItemWeightPair[] hardItems;

    private List<GameObject> weightedObjects = new List<GameObject>();
    private float time = 0f;
    
    // Use this for initialization
	void Start ()
    {
        int difficulty;

        switch (PlayerPrefs.GetString("difficulty"))
        {
            case "hard":
                difficulty = 1;
                break;
            case "medium":
                difficulty = 2;
                break;
            default:
                difficulty = 1;
                break;
        }

        if (difficulty > 0 && easyItems.Length > 0)
        { foreach (ItemWeightPair p in easyItems)
            {
                for (int i = 0; i < p.ItemWeighting; i++)
                {
                    weightedObjects.Add(p.Item);
                }
            }
        }

        if (difficulty > 1 && mediumItems.Length > 0)
        {
            foreach (ItemWeightPair p in mediumItems)
            {
                for (int i = 0; i < p.ItemWeighting; i++)
                {
                    weightedObjects.Add(p.Item);
                }
            }
        }

        if (difficulty > 2 && hardItems.Length > 0)
        {
            foreach (ItemWeightPair p in hardItems)
            {
                for (int i = 0; i < p.ItemWeighting; i++)
                {
                    weightedObjects.Add(p.Item);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (time > secBetweenSpawns)
        {
            Spawn();
            time = Time.deltaTime;
        }
        else
        {
            time += Time.deltaTime;
        }
	}

    private void Spawn()
    {
        int i = Random.Range(0, weightedObjects.Count);
        GameObject spawning = Instantiate(weightedObjects[i]);
        spawning.transform.position = this.gameObject.transform.position;
        spawning.transform.rotation = this.gameObject.transform.rotation;
    }
}
