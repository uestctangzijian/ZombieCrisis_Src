using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    private List<UpgradeInfo> allUpgrades;
    [SerializeField]
    private List<Weapon> weaponsToUnlock;
    [SerializeField]
    private PlayerController player;

    private int maxMultiplierReached = 1; // 已经到达的最大连杀数

    private Dictionary<int, UpgradeInfo> upgradeDict = new Dictionary<int, UpgradeInfo>();
    
    private void OnEnable()
    {
        ScoreManager.OnMultiplierIncreased += handleMultiplierIncrease;
    }

    private void OnDisable()
    {
        ScoreManager.OnMultiplierIncreased -= handleMultiplierIncrease;
    }

    // Start is called before the first frame update
    void Start()
    {
        makeUpgradeDict();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void makeUpgradeDict()
    {
        foreach (UpgradeInfo upgrade in allUpgrades)
        {
            if (upgradeDict.ContainsKey(upgrade.UnlockMultiplier))
            {
                Debug.Log("已获得此升级：" + upgrade.DisplayName);
            } else
            {
                upgradeDict.Add(upgrade.UnlockMultiplier, upgrade);
            }
        }
    }

    private void handleMultiplierIncrease(int multiplier)
    {
        if (multiplier > maxMultiplierReached)
        {
            maxMultiplierReached = multiplier;
            if (upgradeDict.ContainsKey(multiplier))
            {
                TriggerUpgrade(upgradeDict[multiplier]);
            }
        }
    }

    private void TriggerUpgrade(UpgradeInfo upgrade)
    {
        if (upgrade.type == UpgradeType.UnlockWeapon)
        {
            Weapon weapon = Instantiate(weaponsToUnlock.Find(w => w.GetWeaponDef().type == upgrade.weaponType));
            player.UnlockWeapon(weapon);
        } else
        {
            player.MakeWeaponUpgrade(upgrade);
        }
    }
}
