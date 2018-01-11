using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterControlProgram : MonoBehaviour {

    public static MasterControlProgram mcp;

    [SerializeField]
    private int maxLives = 3;
    private static int _livesRemaining;
    public static int LivesRemaing
    {
        get { return _livesRemaining;  }    
    }

    [SerializeField]
    private int startingCredits;
    public static int Credits;

    void Awake()
    {
        if (mcp == null)
        {
            mcp = GameObject.FindGameObjectWithTag("MCP").GetComponent<MasterControlProgram>();
        }    
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public Transform spawnFX;
    // public AudioClip respawnAudio;
    public float spawnDelay = 2;
    public string spawnCountdown = "SpawnCountdown"; // do we want a spawncountdown?
    public string respawnSoundName = "Respawn";
    public string gainCreditSoundName = "CreditSoundFX";
    public string gameOverSoundName = "GameOver";

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

        [SerializeField]
    private WaveSpawner waveSpawner;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onUpgradeMenuToggle;


    private KeyCode upgradeKey = KeyCode.U;
    private KeyCode upgradeKeyAlt = KeyCode.Escape;

    //Cache
    private AudioManager audioManager;

    void Start()
    {
        if (cameraShake == null)
        {
            Debug.LogError("no camera shake referenced in MCP");
        }

        _livesRemaining = maxLives;

        Credits = startingCredits;

        //Caching
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("NO AUDIO MANAGER FOUND IN SCENE!!!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(upgradeKey))
        {
            ToggleUpgradeMenu();
        }
    }


    //Enable/Disable Upgrade Menu
    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        onUpgradeMenuToggle.Invoke(upgradeMenu.activeSelf);
    }

    public void EndOfLine ()
    {
        audioManager.PlaySound(gameOverSoundName);

        Debug.Log("GAME OVER MAN, GAME OVER!!!");
        gameOverUI.SetActive(true);

    }

    public IEnumerator _RespawnPlayer()
    {
        // audioManager.PlaySound(spawnCountdown); // do we want a countdown?
        yield return new WaitForSeconds(spawnDelay);

        audioManager.PlaySound(respawnSoundName);    
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform particleclone = Instantiate (spawnFX, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(particleclone.gameObject, 3f);
    }


    public static void KillPlayer (Player player)
    {
        Destroy(player.gameObject);
        _livesRemaining -= 1;
        if (_livesRemaining <= 0)
        {
            mcp.EndOfLine();
        }
        else
        {
            mcp.StartCoroutine(mcp._RespawnPlayer());
        }
    }

    public static void KillEnemy (Enemy enemy)
    {
        mcp._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        //Make Explody sounds
        audioManager.PlaySound(_enemy.deathSoundName);

        //Get Credits
        Credits += _enemy.creditDrop;
        // audioManager.PlaySound(gainCreditSoundName);
        // Fix this ^^^^
        audioManager.PlaySound("CreditSoundFX");

        //Add Particles
        Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
        Destroy(_clone.gameObject, 5f);

        //Camera Shake
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeTime);
        Destroy(_enemy.gameObject);
    }
}
