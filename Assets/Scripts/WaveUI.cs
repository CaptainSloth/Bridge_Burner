using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

    [SerializeField]
    WaveSpawner spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountdownText;

    [SerializeField]
    Text waveCountText;

    private WaveSpawner.SpawnState prevState;

	// Use this for initialization
	void Start () {
		if (spawner == null)
        {
            Debug.LogError("No Spawner regerenced!");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("No WaveAnimator referenced");
            this.enabled = false;
        }
        if (waveCountdownText == null)
        {
            Debug.LogError("No waveCounttext referenced");
            this.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		switch (spawner.State)
        {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountdownUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawnUI();
                break;

        }

        prevState = spawner.State;
	}

    void UpdateCountdownUI()
    {
        if (prevState != WaveSpawner.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
            Debug.Log("COUNTING");
        }
        waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
    }

    void UpdateSpawnUI()
    {
        if (prevState != WaveSpawner.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveCountdown", false);
            waveAnimator.SetBool("WaveIncoming", true);

            waveCountText.text = spawner.NextWave.ToString();

            Debug.Log("SPAWNING");
        }

    }
}
