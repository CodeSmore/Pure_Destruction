using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A manager for the PlayerPrefs        
public class PlayerPrefsManager : MonoBehaviour {
    // basic stats
    const string IDLE_PROJECTILE_RATE_OF_FIRE_LEVEL_KEY = "idle_projectile_rate_of_fire_level";
    const string IDLE_PROJECTILE_DAMAGE_LEVEL_KEY = "idle_projectile_damage_level";

    const string IN_HAND_DOLLARS_KEY = "in_hand_dollars";

    public static void SaveGameData(int idleRateOfFireLevel, int idleDamageLevel, int inHandDollars)
    {
        PlayerPrefs.SetInt(IDLE_PROJECTILE_RATE_OF_FIRE_LEVEL_KEY, idleRateOfFireLevel);
        PlayerPrefs.SetInt(IDLE_PROJECTILE_DAMAGE_LEVEL_KEY, idleDamageLevel);

        PlayerPrefs.SetInt(IN_HAND_DOLLARS_KEY, inHandDollars);
    }

    public static int GetIdleProjectileRateOfFire()
    {
        return PlayerPrefs.GetInt(IDLE_PROJECTILE_RATE_OF_FIRE_LEVEL_KEY, 1);
    }

    public static int GetIdleProjectileDamage()
    {
        return PlayerPrefs.GetInt(IDLE_PROJECTILE_DAMAGE_LEVEL_KEY, 1);
    }

    public static int GetInHandDollars()
    {
        return PlayerPrefs.GetInt(IN_HAND_DOLLARS_KEY, 0);
    }
}
