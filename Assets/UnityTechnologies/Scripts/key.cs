using UnityEngine;

public class Key : MonoBehaviour
{
    public AudioSource SFX;

    public GameObject player;

    LevelEnding levelEnding = new LevelEnding();
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !levelEnding.IsKeyCollected())
        {
            levelEnding.setKeyFlag(true);
            // Call the method to set the key flag in LevelEnding
            // levelEnding.setKeyFlag(true);

            // Play the sound effect
            if (SFX != null)
            {
                SFX.Play();
            }

            // Destroy the key object after a delay
            Destroy(gameObject, 0.5f);
        }
    }

}
