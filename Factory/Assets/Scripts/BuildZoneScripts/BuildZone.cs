using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildZone : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject buildPoint;
    [SerializeField] private int buildZoneNumber;
    [SerializeField] private GameObject[] buildSchemaObjects;

    private List<Schema> schemas = new List<Schema>();

    [SerializeField] private AudioClip loadedSound;
    [SerializeField] private AudioClip builtSound;

    private Schema currentSchema;

    private AudioSource audioSource;

    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();

        int difficulty;

        switch (PlayerPrefs.GetString("difficulty"))
        {
            case "hard":
                difficulty = 4;
                break;
            case "medium":
                difficulty = 3;
                break;
            default:
                difficulty = 2;
                break;
        }

        for (int i = 0; i < buildSchemaObjects.Length; i++)
        {
            if (difficulty > i)
            {
                schemas.Add(buildSchemaObjects[i].GetComponent<Schema>());
            }
        }

        currentSchema = schemas[0];
        currentSchema.SpawnGhost();
	}

    public int Number
    {
        get
        {
            return buildZoneNumber;
        }
    }

    public string GetCurrentSchemaName(int index) {
        return schemas[index].gameObject.name;
    }

    private void Update()
    {
        if (gameController.PlayerLost(buildZoneNumber) && currentSchema != null)
        {
            currentSchema.Failed();
            DeleteSchema(currentSchema);
            currentSchema = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {                
        if (other.gameObject.GetComponent<Identifiable>() != null)
        {
            Identifiable ids = other.gameObject.GetComponent<Identifiable>();

            if (currentSchema != null && !ids.HasIdentifier(Identifier.PlayerMoving) && other.gameObject.tag != "Player")
            {
                if (currentSchema.BelongsToSchema(ids) && !currentSchema.IsLoaded(ids))
                {
                    other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    currentSchema.HandleValidObject(other.gameObject, buildZoneNumber);
                    return;
                }

                EjectFromBuildPoint(other.gameObject);
            }
        }
    }

    private void EjectFromBuildPoint(GameObject item)
    {
        float increment = -0.1f;

        item.transform.position = Vector3.MoveTowards(item.transform.position, buildPoint.transform.position, increment);
    }

    public void ChangeCurrentSchema(Schema s, GameObject o, Built b)
    {
        DeleteSchema(s);
        DestroyBuiltObject(o, b);
        NextSchema();
    }

    private void DeleteSchema(Schema s)
    {
        schemas.Remove(s);
        Destroy(s.gameObject);
        Destroy(s);
    }

    private void DestroyBuiltObject(GameObject o, Built b)
    {
        Destroy(b);
        Destroy(o);
    }

    private void NextSchema()
    {
        if (schemas.Count > 0)
        {
            currentSchema = schemas[0];
            currentSchema.SpawnGhost();
        }
        else
        {
            currentSchema = null;
        }
    }

    public void PlayLoadedSound()
    {
        PlaySoundEffect(loadedSound);
    }

    public void PlayBuiltSound()
    {
        PlaySoundEffect(builtSound);
    }

    private void PlaySoundEffect(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }
}