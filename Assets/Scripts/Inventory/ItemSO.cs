using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public GameObject itemPrefab;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public bool UseItem()
    {
        if (statToChange == StatToChange.health)
        {
            PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
            if (playerStats.currentHealth == playerStats.maxHealth)
            {
                return false;
            }
            else
            {
                playerStats.RestoreHealth(amountToChangeStat);
                return true;
            }
        }

        return false;

        // change sanity
        /*if (statToChange == StatToChange.sanity)
        {
            GameObject.Find("Player").GetComponent<PlayerStats>().RestoreHealth(amountToChangeStat);
        }*/
    }

    public enum StatToChange
    {
        none,
        health,
        sanity,
    };
}
