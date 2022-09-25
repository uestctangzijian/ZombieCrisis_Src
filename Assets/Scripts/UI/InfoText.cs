using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPrefab;
    [SerializeField]
    private float showTime;
    [SerializeField]
    private float fadeOutSpeed = 5f;
    [SerializeField]
    private int maxAmountEntries = 8;
    [SerializeField]
    private PlayerController player;

    private string pickedUpTextString = "Picked up {0}";
    private string upgradeTextString = "{0}+: {1}";
    private string unlockTextString = "New Weapon:{0}";
    private string enquipedTextString = "{0} selected!";
    private string levelString = "-+-+-+- LEVEL {0} -+-+-+-";
    private string outOfAmmoString = "{0} is out of Ammo!";

    private List<TextLogEntry> textLog = new List<TextLogEntry>();

    private void OnEnable()
    {
        player.onPickUp += AddPickupTextEntry;
        player.onWeaponUpgrade += AddUpradeTextEntry;
        player.onWeaponUnlocked += AddUnlockWeaponTextEntry;
        player.onWeaponEquipped += AddWeaponEnquipedTextEntry;
        player.attack.onAmmoRunout += AddWeaponOutOfAmmoTextEntry;
    }

    private void OnDisable()
    {
        player.onPickUp -= AddPickupTextEntry;
        player.onWeaponUpgrade -= AddUpradeTextEntry;
        player.onWeaponUnlocked -= AddUnlockWeaponTextEntry;
        player.onWeaponEquipped -= AddWeaponEnquipedTextEntry;
        player.attack.onAmmoRunout -= AddWeaponOutOfAmmoTextEntry;
    }

    // 武器解锁文本
    private void AddUnlockWeaponTextEntry(Weapon weapon)
    {
        TextMeshProUGUI textMesh = Instantiate(textPrefab, transform);
        TextLogEntry logEntry = new TextLogEntry(textMesh, textLog.Count, Color.green);
        logEntry.SetText(string.Format(unlockTextString, weapon.GetWeaponDef().name));
        AddAndShowTextLogEntry(logEntry);
    }

    // 武器升级文本
    private void AddUpradeTextEntry(Weapon weapon, UpgradeInfo upgrade)
    {
        TextMeshProUGUI textMesh = Instantiate(textPrefab, transform);
        TextLogEntry logEntry = new TextLogEntry(textMesh, textLog.Count, Color.green);
        logEntry.SetText(string.Format(upgradeTextString, weapon.GetWeaponDef().name, upgrade.DisplayName));
        AddAndShowTextLogEntry(logEntry);
    }

    // 物品拾取文本
    private void AddPickupTextEntry(Weapon weapon)
    {
        TextMeshProUGUI textMesh = Instantiate(textPrefab, transform);
        TextLogEntry logEntry = new TextLogEntry(textMesh, textLog.Count, Color.white);
        logEntry.SetText(string.Format(pickedUpTextString, weapon.GetWeaponDef().name));
        AddAndShowTextLogEntry(logEntry);
    }

    // 武器切换文本
    private void AddWeaponEnquipedTextEntry(Weapon weapon)
    {
        TextMeshProUGUI textMesh = Instantiate(textPrefab, transform);
        TextLogEntry logEntry = new TextLogEntry(textMesh, textLog.Count, Color.white);
        logEntry.SetText(string.Format(enquipedTextString, weapon.GetWeaponDef().name));
        AddAndShowTextLogEntry(logEntry);
    }

    // 子弹用尽文本
    private void AddWeaponOutOfAmmoTextEntry(Weapon weapon)
    {
        TextMeshProUGUI textMesh = Instantiate(textPrefab, transform);
        TextLogEntry logEntry = new TextLogEntry(textMesh, textLog.Count, Color.red);
        logEntry.SetText(string.Format(outOfAmmoString, weapon.GetWeaponDef().name));
        AddAndShowTextLogEntry(logEntry);
    }

    // 关卡升级文本
    public void AddLevelTextEntry(int level)
    {
        TextMeshProUGUI textMesh = Instantiate(textPrefab, transform);
        TextLogEntry logEntry = new TextLogEntry(textMesh, textLog.Count, Color.white);
        logEntry.SetText(string.Format(levelString, level));
        AddAndShowTextLogEntry(logEntry);
    }

    private void AddAndShowTextLogEntry(TextLogEntry logEntry)
    {
        if (textLog.Count == maxAmountEntries)
            FadeOutEntryAfterDelay(0f, fadeOutSpeed, textLog[0]);

        textLog.Add(logEntry);
        logEntry.MoveToIndex(0);

        StartCoroutine(FadeOutEntryAfterDelay(showTime, fadeOutSpeed, logEntry));
    }

    private IEnumerator FadeOutEntryAfterDelay(float delay, float speed, TextLogEntry entry)
    {
        yield return new WaitForSeconds(delay);
        while(entry.TextMesh.color.a > 0.0f)
        {
            Color modified = entry.TextMesh.color;
            modified.a -= (Time.deltaTime * speed);
            entry.TextMesh.color = modified;
            yield return null;
        }
        entry.Destroy();
        textLog.Remove(entry);
    }
}
