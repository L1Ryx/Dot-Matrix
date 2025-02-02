using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips; // Array to hold multiple clips for round-robin selection
    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(0.1f, 3f)]
    public float minPitch = 1.0f; // Default minimum pitch range set to 1
    [Range(0.1f, 3f)]
    public float maxPitch = 1.0f; // Default maximum pitch range set to 1
    [TextArea]
    public string description; // Description for the sound
    public bool loopIndefinitely = false;
    public List<GameEvent> gameEvents; // List of events to trigger the sound
}
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    public Sound[] sounds;
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();
    private Dictionary<string, AudioSource> loopingSounds = new Dictionary<string, AudioSource>();

    private List<GameObject> listenerObjects = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound sound in sounds)
        {
            soundDictionary[sound.name] = sound;
            if (sound.gameEvents != null && sound.gameEvents.Count > 0)
            {
                foreach (var gameEvent in sound.gameEvents)
                {
                    // Create a new GameObject for the listener
                    GameObject listenerObject = new GameObject("Listener_" + sound.name + "_" + gameEvent.name);
                    listenerObject.transform.SetParent(transform); // Make it a child of the SFXManager
                    GameEventListener listener = listenerObject.AddComponent<GameEventListener>();
                    listener.Event = gameEvent;
                    listener.Response = new UnityEvent();
                    listener.Response.AddListener(() => PlaySound(sound.name));
                    listenerObjects.Add(listenerObject);
                    
                    // Register the listener
                    listener.Register();
                    
                    Debug.Log($"Created listener for sound: {sound.name} for event: {gameEvent.name}");
                }
            }
        }
    }

    void OnDestroy()
    {
        foreach (var listenerObject in listenerObjects)
        {
            GameEventListener listener = listenerObject.GetComponent<GameEventListener>();
            if (listener != null && listener.Event != null)
            {
                listener.Unregister();
                Debug.Log($"Unregistered listener for event: {listener.Event.name}");
            }
        }
    }

    public void PlaySound(string soundName)
    {
        Debug.Log($"Attempting to play sound: {soundName}");
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            AudioSource audioSource = CreateAudioSource(sound);
            if (audioSource == null)
            {
                Debug.LogError($"Failed to create AudioSource for sound: {soundName}");
                return; // AudioSource creation failed, exit the function
            }

            audioSource.Play();
            Debug.Log($"Playing sound: {soundName}");

            if (sound.loopIndefinitely)
            {
                if (!loopingSounds.ContainsKey(soundName))
                {
                    loopingSounds[soundName] = audioSource;
                    StartCoroutine(AdjustPitchAtLoop(audioSource, sound));
                }
            }
            else
            {
                Destroy(audioSource.gameObject, audioSource.clip.length);
            }
        }
        else
        {
            Debug.LogWarning($"Sound name not found in dictionary: {soundName}");
        }
    }

    private IEnumerator AdjustPitchAtLoop(AudioSource source, Sound sound)
    {
        while (source.isPlaying)
        {
            yield return new WaitWhile(() => source.isPlaying && source.time < source.clip.length - 0.01f);
            source.pitch = Random.Range(sound.minPitch, sound.maxPitch);
        }
    }

    public void StopLoopingSound(string soundName)
    {
        if (loopingSounds.TryGetValue(soundName, out AudioSource audioSource))
        {
            StopCoroutine("AdjustPitchAtLoop");  // Stop the coroutine if it's running
            audioSource.Stop();
            Destroy(audioSource.gameObject);
            loopingSounds.Remove(soundName);
        }
    }

    private AudioSource CreateAudioSource(Sound sound)
    {
        if (sound.clips == null || sound.clips.Length == 0)
        {
            Debug.LogError($"No clips assigned for the sound '{sound.name}'.");
            return null; // Early return to prevent further execution and potential crash
        }

        // Safely access an AudioClip using Random.Range
        AudioClip clipToPlay = sound.clips[Random.Range(0, sound.clips.Length)];
        GameObject soundGameObject = new GameObject("SFX_" + sound.name);
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = clipToPlay;
        audioSource.volume = sound.volume;
        audioSource.pitch = Random.Range(sound.minPitch, sound.maxPitch); // Randomize pitch
        audioSource.loop = sound.loopIndefinitely; // Set looping based on the sound settings

        return audioSource;
    }
}