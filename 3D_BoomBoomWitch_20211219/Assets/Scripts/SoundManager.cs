using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource aud;

    private void Awake()
    {
        instance = this;

        aud = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放音效並且隨機音量
    /// </summary>
    /// <param name="sound">要播放的音效</param>
    /// <param name="min">最小音量</param>
    /// <param name="max">最大音量</param>
    public void PlaySoundRandomVolue(AudioClip sound, float min, float max)
    {
        float randomVolume = Random.Range(min, max);
        aud.PlayOneShot(sound, randomVolume);
    }
}
