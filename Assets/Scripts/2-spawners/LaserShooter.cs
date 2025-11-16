using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

/**
 * This component spawns the given laser-prefab whenever the player clicks a given key.
 * It also updates the "scoreText" field of the new laser.
 */
public class LaserShooter: ClickSpawner {
    [SerializeField]
    [Tooltip("How many points to add to the shooter, if the laser hits its target")]
    int pointsToAdd = 1;
    [SerializeField] int ammo;
    [SerializeField] float cooldown;
    [SerializeField] Text ammoText; //Text field to display current ammo
    
    // A reference to the field that holds the score that has to be updated when the laser hits its target.
    private NumberField scoreField;  

    private void Start()
    {
        ammoText.text = "Ammo: " + ammo; //set initial ammo text display
        timer = cooldown; //make the first shot ignore weapon cooldown
        scoreField = GetComponentInChildren<NumberField>();
        if (!scoreField)
            Debug.LogError($"No child of {gameObject.name} has a NumberField component!");
    }
    

    private void AddScore()
    {
        scoreField.AddNumber(pointsToAdd);
    }
    
    protected override GameObject spawnObject()
    { 
        //check ammo
        if (ammo<1)
        {
            Debug.Log("No ammo");
            return null;
        }
        
        //check cooldown
        if (timer < cooldown)
        {
            Debug.Log(this.gameObject.name + " on cooldown");
            return null;
        }

        timer = 0; //reset cooldown
        ammo--; //reduce ammo by 1
        ammoText.text = "Ammo: " + ammo; //update ammo text display
        GameObject newObject = base.spawnObject();  // base = super
        DestroyOnTrigger2D newObjectDestroyer = newObject.GetComponent<DestroyOnTrigger2D>();
        if (newObjectDestroyer)
            newObjectDestroyer.onHit += AddScore;
        return newObject;
    }
}
