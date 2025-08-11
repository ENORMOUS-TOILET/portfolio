using UnityEngine;

public class FootStepAudioPlayer : MonoBehaviour
{
    public AudioClip[] footstepClips; // 存储五个脚步声的AudioClip
    public AudioSource audioSource;   // 用于播放音频的AudioSource组件

    private int currentClipIndex = 0; // 当前播放的AudioClip索引

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // 播放下一个脚步声
    public void PlayNextFootstep()
    {
        if (footstepClips.Length == 0)
        {
            Debug.LogWarning("No footstep clips assigned!");
            return;
        }

        // 播放当前索引的AudioClip
        audioSource.clip = footstepClips[currentClipIndex];
        audioSource.Play();

        // 更新索引，准备播放下一个AudioClip
        currentClipIndex = (currentClipIndex + 1) % footstepClips.Length;
    }
}
