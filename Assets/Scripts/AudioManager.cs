using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;  // Singleton instance
    public AudioSource audioSource;       // AudioSource component to play music
    public float fadeDuration = 1.0f;     // Duration of fade transitions
    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;          // Optional name for clarity
        public AudioClip[] musicClips;    // Array of music clips for the scene
    }
    public SceneMusic[] sceneMusicPools;  // Music pools for each scene

    private int currentSceneIndex = -1;   // Tracks the current scene
    private AudioClip lastPlayedClip;     // To avoid repeating the same clip
    private System.Random random = new System.Random(); // Random instance for shuffling

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusicForScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        PlayMusicForScene(scene.buildIndex);
    }

    public void PlayMusicForScene(int sceneIndex)
    {
        if (sceneIndex == currentSceneIndex || sceneIndex >= sceneMusicPools.Length) return;

        currentSceneIndex = sceneIndex;

        // Get a random clip from the pool for the current scene
        AudioClip randomClip = GetRandomClip(sceneMusicPools[sceneIndex].musicClips);

        if (randomClip != null)
        {
            StartCoroutine(FadeToNewClip(randomClip));
        }
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips.Length == 0) return null;

        // Shuffle the music clips randomly
        ShuffleArray(clips);

        // Pick the first clip from the shuffled array
        AudioClip selectedClip = clips[0];

        // Ensure it's not the same as the last played clip
        if (clips.Length > 1 && selectedClip == lastPlayedClip)
        {
            selectedClip = clips[1]; // Pick the next one if repeated
        }

        lastPlayedClip = selectedClip;
        return selectedClip;
    }

    private void ShuffleArray(AudioClip[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = random.Next(i + 1);
            AudioClip temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private IEnumerator FadeToNewClip(AudioClip newClip)
    {
        // Fade out current music
        if (audioSource.isPlaying)
        {
            yield return StartCoroutine(FadeVolume(0));
        }

        // Change the music clip
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in new music
        yield return StartCoroutine(FadeVolume(1));
    }

    private IEnumerator FadeVolume(float targetVolume)
    {
        float startVolume = audioSource.volume;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
