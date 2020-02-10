using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUtils
{
    public static int heroCostMultiplier = -9;
    public static int GetImprovementBonus(int level, bool checkNextLevel = false)
    {
        int multiplier = 1;
        foreach (KeyValuePair<int, int> p in improvementBonusDic)
        {
            if (checkNextLevel == false && level >= p.Key)
            {
                multiplier += (p.Value - 1);
            }
            else if (checkNextLevel == true && (level + 1) >= p.Key)
            {
                multiplier += (p.Value - 1);
            }
        }
        return multiplier;
    }
    public static float[] CalculateUpgrade(string name, int level, float baseCost, int heroUnlockOrder)
    {
        float[] results = new float[2];
        if (name == "Player")
        {
            //Cost
            results[0] = Mathf.Round(5 * (Mathf.Pow(1.062f, level + 1) - Mathf.Pow(1.062f, level)) / 0.062f);
            //Damage
            results[1] = ((level + 1) * GetImprovementBonus(level, true)) - ((level) * GetImprovementBonus(level)); // difference between current damage and what it would be +1 level
        }
        else
        {
            //Cost
            results[0] = Mathf.Round(baseCost * (Mathf.Pow(1.082f, level)) * (Mathf.Pow(1.082f, 1) - 1) / 0.82f * (1 - heroCostMultiplier));
            //Damage
            float currentDamage = ((baseCost / 10 * (1 - 23 / 1000 * Mathf.Pow(Mathf.Min(heroUnlockOrder, 34), Mathf.Min(heroUnlockOrder, 34))) * (level) * GetImprovementBonus(level)));
            results[1] = ((baseCost / 10 * (1 - 23 / 1000 * Mathf.Pow(Mathf.Min(heroUnlockOrder, 34), Mathf.Min(heroUnlockOrder, 34))) * (level + 1) * GetImprovementBonus(level, true)) - currentDamage);
        }
        return results;
    }
    public static float GetTotalHeroDPS()
    {
        float dps = 0;
        for (int i = 0; i < GameData.sessionData.heroUpgrades.Length; i++)
        {
            dps += GameData.sessionData.heroUpgrades[i].currentDamage;
        }
        return dps;
    }
    public static void UnlockNextHero(GameObject panel)
    {
        Upgrade[] upgrades = panel.GetComponentsInChildren<Upgrade>(true);
        for (int i = upgrades.Length * 2; i > upgrades.Length; i--)
        {
            Upgrade hero = upgrades[i - upgrades.Length - 1];
            if (hero.gameObject.activeSelf == false)
            {
                if (GameData.sessionData.playerData.gold >= CalculateUpgrade(hero.upgradeName, hero.currentLevel - 1, hero.baseCost, hero.heroUnlockOrder)[0] * 0.10f || GameData.sessionData.heroUpgrades[hero.heroUnlockOrder - 1].unlocked == true)
                {
                    hero.gameObject.SetActive(true);
                    GameData.sessionData.heroUpgrades[hero.heroUnlockOrder - 1].unlocked = true;
                    break;
                }
            }
        }
    }

    public UpgradeUtils()
    {
        InitImprovementBonus();
    }

    private static Dictionary<int, int> improvementBonusDic;
    private void InitImprovementBonus()
    {
        improvementBonusDic = new Dictionary<int, int>();
        improvementBonusDic.Add(10, 2); // Level 10 = 200% Damage (100% increase)
        improvementBonusDic.Add(30, 2);
        improvementBonusDic.Add(50, 2);
        improvementBonusDic.Add(70, 2);
        improvementBonusDic.Add(90, 2);
        improvementBonusDic.Add(110, 2);
        improvementBonusDic.Add(130, 2);
        improvementBonusDic.Add(150, 2);
        improvementBonusDic.Add(170, 2);
        improvementBonusDic.Add(190, 2);
        improvementBonusDic.Add(210, 3);
        improvementBonusDic.Add(230, 3);
        improvementBonusDic.Add(250, 2);
        improvementBonusDic.Add(270, 2);
        improvementBonusDic.Add(290, 2);
        improvementBonusDic.Add(310, 3);
        improvementBonusDic.Add(330, 2);
        improvementBonusDic.Add(350, 2);
        improvementBonusDic.Add(370, 2);
        improvementBonusDic.Add(400, 2);
        improvementBonusDic.Add(420, 3);
        improvementBonusDic.Add(440, 3);
        improvementBonusDic.Add(460, 3);
        improvementBonusDic.Add(480, 2);
        improvementBonusDic.Add(500, 2);
        improvementBonusDic.Add(520, 2);
        improvementBonusDic.Add(540, 2);
        improvementBonusDic.Add(560, 2);
        improvementBonusDic.Add(580, 2);
        improvementBonusDic.Add(600, 2);
    }
}
