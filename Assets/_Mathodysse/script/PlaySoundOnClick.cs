using UnityEngine;

public class PlaySoundOnClick : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Mengambil komponen AudioSource yang ada di objek ini secara otomatis
        audioSource = GetComponent<AudioSource>();
    }

    // Fungsi publik ini yang akan dipanggil saat tombol diklik
    public void PlayAudio()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
            Debug.Log("Suara diputar!");
        }
        else
        {
            Debug.LogWarning("AudioSource atau AudioClip belum ada/kosong!");
        }
    }
}