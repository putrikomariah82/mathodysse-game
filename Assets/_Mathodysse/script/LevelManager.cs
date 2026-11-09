using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ada agar Unity bisa memindahkan scene
using UnityEngine.UI; // Wajib ada untuk mengenali komponen Button

public class LevelManager : MonoBehaviour
{
    [Header("Pengaturan Tombol Level")]
    public Button buttonLevel2; // Seret tombol Level 2 dari Hierarchy ke kotak ini di Inspector

    void Start()
    {
        // Pengecekan otomatis saat scene menu dibuka
        // Jika tombolLevel2 dimasukkan di Inspector, jalankan pengecekan kunci
        if (buttonLevel2 != null)
        {
            // Ambil data dari memori (default 0 jika belum pernah tamat/belum 8 soal benar)
            int statusLevel2 = PlayerPrefs.GetInt("Level2_Unlocked", 0);

            if (statusLevel2 == 1)
            {
                // Jika sudah terbuka (bernilai 1), tombol bisa diklik
                buttonLevel2.interactable = true;
            }
            else
            {
                // Jika belum terbuka (bernilai 0, baru 7 soal atau game over), tombol terkunci
                buttonLevel2.interactable = false;
            }
        }
    }

    // Fungsi untuk pindah level berdasarkan nama (Contoh: "01", "02", "03", dst.)
    public void LoadLevelByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Fungsi untuk mengulang level yang sedang dimainkan (Tombol Restart)
    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Fungsi untuk lanjut ke level berikutnya (Tombol Next Level)
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Mengecek apakah index scene selanjutnya masih ada di Build Profiles / Build Settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Ini adalah level terakhir!");
        }
    }

    // Fungsi untuk kembali ke Menu Utama
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("startmenu"); // Sesuaikan dengan nama scene Main Menu Anda
    }
}