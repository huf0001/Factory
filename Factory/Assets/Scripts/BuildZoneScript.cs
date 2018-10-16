using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildZoneScript : MonoBehaviour
{
    [SerializeField] private GameObject buildPoint;
    [SerializeField] private int buildZoneNumber;
    [SerializeField] private GameObject[] buildSchemaObjects;

    private List<BuildSchemaScript> schemas = new List<BuildSchemaScript>();

    [SerializeField] private AudioClip loadedSound;
    [SerializeField] private AudioClip builtSound;

    private BuildSchemaScript currentSchema;

    private AudioSource audioSource;

    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();

        foreach (GameObject o in buildSchemaObjects)
        {
            schemas.Add(o.GetComponent<BuildSchemaScript>());
        }

        currentSchema = schemas[0];
        currentSchema.ActivateGhost();
	}

    private void OnTriggerStay(Collider other)
    {                
        if (other.gameObject.GetComponent<IdentifiableScript>() != null)
        {
            IdentifiableScript ids = other.gameObject.GetComponent<IdentifiableScript>();

            if (currentSchema != null && !ids.HasIdentifier(Identifier.PlayerMoving) && other.gameObject.tag != "Player")
            {
                if (currentSchema.BelongsToSchema(ids) && !currentSchema.IsLoaded(ids))
                {
                    if (!ids.HasIdentifier(Identifier.InBuildZone))
                    {
                        other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        ids.AddIdentifier(Identifier.InBuildZone);
                        currentSchema.HandleValidObject(other.gameObject);

                        return;
                    }
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

    public void SchemaComplete(BuildSchemaScript schema)
    {
        schemas.Remove(schema);
        Destroy(schema.gameObject);
        Destroy(schema);

        if (buildZoneNumber == 1)
        {
            GameObject.Find("GameControllerCamera").GetComponent<GameControllerScript>().IncrementPlayer1BuildCount();
        }
        else if (buildZoneNumber == 2)
        {
            GameObject.Find("GameControllerCamera").GetComponent<GameControllerScript>().IncrementPlayer2BuildCount();
        }
        else
        {
            Debug.Log("Invalid build zone number");
        }
        
        PlayBuiltSound();
        //Particle effect
    }

    public void ChangeCurrentSchema(GameObject builtObject, BuiltScript script)
    {
        DestroyBuiltObject(builtObject, script);

        if (schemas.Count > 0)
        {
            currentSchema = schemas[0];
            currentSchema.ActivateGhost();
        }
        else
        {
            currentSchema = null;
        }
    }

    public void DestroyBuiltObject(GameObject builtObject, BuiltScript script)
    {
        Destroy(script);
        Destroy(builtObject);
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
