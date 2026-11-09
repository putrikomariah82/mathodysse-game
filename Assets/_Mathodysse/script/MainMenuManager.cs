using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panel / Tombol Suara yang Disembunyikan")]
    public GameObject soundToggleGroup; // Masukkan objek Soundtoggle di sini

    private bool isSoundMenuOpen = false;

    void Start()
    {
        // Pastikan saat game mulai, menu sound tertutup/tidak aktif
        if (soundToggleGroup != null)
            soundToggleGroup.SetActive(false);
    }

    // Fungsi untuk tombol Main / Play Menu (membuka pilihan sound on/off)
    public void OnMainMenuButtonClicked()
    {
        isSoundMenuOpen = !isSoundMenuOpen;
        if (soundToggleGroup != null)
        {
            soundToggleGroup.SetActive(isSoundMenuOpen);
        }
    }

    // Fungsi untuk tombol Play game (berpindah ke scene levelselect)
    public void PlayGame()
    {
        SceneManager.LoadScene("levelselect");
    }

    // Fungsi untuk tombol Exit / Keluar Game
    public void QuitGame()
    {
        Debug.Log("Game Keluar!");
        Application.Quit();
    }
}