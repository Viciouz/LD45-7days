using System.Collections;
using UnityEngine;


public class SoundManager : MonoBehaviour {

    public AudioSource sfxSource;
    public AudioSource musicSource;

    public float lowPitchRange = 0.9f;
    public float highPitchRange = 1.1f;

    public static SoundManager Instance { get; private set; }

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    //Used to play single sound clips.
    public static void PlaySingle(AudioClip clip) {
        if (clip != null) {
            //Play the clip.
            Instance.sfxSource.PlayOneShot(clip);
        }
    }

    public static void PlayWithLoop(AudioClip clip) {
        if (clip != null) {
            Instance.sfxSource.loop = true;
            Instance.sfxSource.clip = clip;
            Instance.sfxSource.Play();
        }

    }

    internal static void StopLoop() {
        Instance.sfxSource.Stop();
        Instance.sfxSource.loop = false;
        Instance.sfxSource.clip = null;
    }


    //PlayRandomSfx chooses randomly between various audio clips and slightly changes their pitch.
    public static void PlayRandomSfx(params AudioClip[] clips) {
        if (clips.Length != 0) {
            //Generate a random number between 0 and the length of our array of clips passed in.
            int randomIndex = UnityEngine.Random.Range(0, clips.Length);

            //Choose a random pitch to play back our clip at between our high and low pitch ranges.
            float randomPitch = UnityEngine.Random.Range(Instance.lowPitchRange, Instance.highPitchRange);

            //Set the pitch of the audio source to the randomly chosen pitch.
            Instance.sfxSource.pitch = randomPitch;

            //Play the clip
            Instance.sfxSource.PlayOneShot(clips[randomIndex]);
        }
    }


    //PlayRandomSfx chooses randomly between various audio clips and slightly changes their pitch.
    public static void PlayRandomSfx(AudioSource src, params AudioClip[] clips) {
        if (clips.Length != 0) {
            //Generate a random number between 0 and the length of our array of clips passed in.
            int randomIndex = UnityEngine.Random.Range(0, clips.Length);

            //Choose a random pitch to play back our clip at between our high and low pitch ranges.
            float randomPitch = UnityEngine.Random.Range(Instance.lowPitchRange, Instance.highPitchRange);

            //Set the pitch of the audio source to the randomly chosen pitch.
            src.pitch = randomPitch;

            //Play the clip
            src.PlayOneShot(clips[randomIndex]);
        }
    }


    public void PlayMusic(AudioClip clip, bool loop) {
        if (!loop) {
            Instance.musicSource.PlayOneShot(clip);
        } else {
            Instance.musicSource.clip = clip;
            Instance.musicSource.loop = loop;
            Instance.musicSource.Play();
        }

    }

    public void MusicSettingChanged(int newValue) {
        Debug.Log("SoundManager Music changed");
        musicSource.volume = newValue;
    }


    public void FadeInAudioSource(AudioSource audioSource, float fadeTime) {
        StartCoroutine(FadeIn(audioSource, fadeTime));
    }

    public void FadeOutAudioSource(AudioSource audioSource, float fadeTime) {
        StartCoroutine(FadeOut(audioSource, fadeTime));
    }

    private IEnumerator FadeIn(AudioSource audioSource, float FadeTime) {
        float startVolume = 0.2f;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 1.0f) {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 1f;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }


}
