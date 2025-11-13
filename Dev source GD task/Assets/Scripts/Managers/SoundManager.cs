using UnityEngine;

public class SoundManager :MonoBehaviour {

    public static SoundManager Instance { get; private set; }

    [Header("Robot Sounds")]
    [SerializeField] private AudioSource robotMoveAudioSource;
    [SerializeField] private AudioSource robotJumpAudioSource;
    [SerializeField] private AudioSource robotLoseAudioSource;
    [SerializeField] private AudioSource robotWinAudioSource;

    [Header("Others")]
    [SerializeField] private AudioSource coinPickupAudioSource;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        GameManager.Instance.OnCoinCollected += ctx => { PlayCoinPickupSound(); };
    }

    private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        if (e.gameOverType == GameManager.GameOverType.Victory)
        {
            PlayWinSound();
        }
        else
        {
            PlayLoseSound();
        }
    }

    // Robot Sounds
    public void PlayMoveSound()
    {
        PlaySound(robotMoveAudioSource);
    }
    public void StopMoveSound()
    {
        robotMoveAudioSource.Stop();
    }
    public void PlayJumpSound()
    {
        PlaySound(robotJumpAudioSource);
    }
    public void PlayLoseSound()
    {
        PlaySound(robotLoseAudioSource);
    }
    public void PlayWinSound()
    {
        PlaySound(robotWinAudioSource);
    }
    public void PlayCoinPickupSound()
    {
        PlaySound(coinPickupAudioSource);
    }
    private void PlaySound(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }
}
