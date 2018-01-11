using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private float healthMultiplier = 1.3f;

    [SerializeField]
    private float movementSpeedMultiplier = 1.3f;

    [SerializeField]
    private int upgradeCost = 50;

    private PlayerStats stats;

    private void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "Health: " + stats.maxHealth.ToString();
        speedText.text = "Speed: " + stats.movementSpeed.ToString();
    }

    public void UpgradeHealth()
    {
        if (MasterControlProgram.Credits < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoCredits");
            return;
        }
        stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);

        MasterControlProgram.Credits -= upgradeCost;
        AudioManager.instance.PlaySound("CreditSoundFX");

        UpdateValues();
    }

    public void UpgradeSpeed()
    {
        if (MasterControlProgram.Credits < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoCredits");
            return;
        }
        stats.movementSpeed = Mathf.Round (stats.movementSpeed * movementSpeedMultiplier);
        // stats.movementSpeed = (int)(stats.movementSpeed * movementSpeedMultiplier);

        MasterControlProgram.Credits -= upgradeCost;
        AudioManager.instance.PlaySound("CreditSoundFX");

        UpdateValues();
    }

}
