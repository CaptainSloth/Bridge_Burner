using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour {

    public int fallBoundry = -20;

    public string dethSoundName = "DethVoice";
    public string damageSoundName = "PlayerGrunt";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    private PlayerStats stats;

    void Start()
    {
        stats = PlayerStats.instance;

        stats.curHealth = stats.maxHealth;
        if (statusIndicator == null)
        {
            Debug.LogError("NO STATUS INDICATOR REFERENCED, WTF YO!");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }

        MasterControlProgram.mcp.onUpgradeMenuToggle += OnMenuUpgradeToggle;

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("NO AUDIOMANAGER IN SCENE MOTHERFUCKER!!!");
        }

        InvokeRepeating("RegenHealth", 1f/stats.healthRegenRate, 1f/stats.healthRegenRate);
    }

    void RegenHealth()
    {
        stats.curHealth += 1;
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    void Update()
    {
        if (transform.position.y <= fallBoundry)
            DamagePlayer(999999);
    }

    void OnMenuUpgradeToggle (bool active)
    {
        GetComponent<Platformer2DUserControl>().enabled = !active;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if (_weapon != null)
            _weapon.enabled = !active;
    }

    void OnDestroy()
    {
        MasterControlProgram.mcp.onUpgradeMenuToggle -= OnMenuUpgradeToggle;
    }

    public void DamagePlayer (int damage)
    {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0)
        {
            //play deth sound
            audioManager.PlaySound(dethSoundName);

            //kill player
            MasterControlProgram.KillPlayer(this);
        }
        else
        {
            //play damage sound
            audioManager.PlaySound(damageSoundName);
        }

        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }
    
}
