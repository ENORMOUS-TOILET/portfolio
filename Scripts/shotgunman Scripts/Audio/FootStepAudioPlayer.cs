using UnityEngine;

public class FootStepAudioPlayer : MonoBehaviour
{
    public AudioClip[] footstepClips; // �洢����Ų�����AudioClip
    public AudioSource audioSource;   // ���ڲ�����Ƶ��AudioSource���

    private int currentClipIndex = 0; // ��ǰ���ŵ�AudioClip����

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // ������һ���Ų���
    public void PlayNextFootstep()
    {
        if (footstepClips.Length == 0)
        {
            Debug.LogWarning("No footstep clips assigned!");
            return;
        }

        // ���ŵ�ǰ������AudioClip
        audioSource.clip = footstepClips[currentClipIndex];
        audioSource.Play();

        // ����������׼��������һ��AudioClip
        currentClipIndex = (currentClipIndex + 1) % footstepClips.Length;
    }
}
