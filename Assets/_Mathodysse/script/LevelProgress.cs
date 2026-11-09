using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgress : MonoBehaviour
{
    public static LevelProgress Instance;

    [Header("Pengaturan Soal")]
    public int totalSoalDiLevel = 8; // Target wajib 8 soal benar
    private int soalBenarTerkumpul = 0;

    [Header("UI Panel Selesai (Level Clear)")]
    public GameObject levelClearPanel; // Masukkan LevelClearPanel di sini

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // PERINTAH SEMENTARA: Menghapus memori data game lama
        // PlayerPrefs.DeleteAll();

        if (levelClearPanel != null)
        {
            levelClearPanel.SetActive(false); // Sembunyikan panel saat awal game
        }
    }

    // 1. Dipanggil setiap kali pemain menjawab soal dengan benar
    public void TambahSoalBenar()
    {
        soalBenarTerkumpul++;
        Debug.Log("Soal benar terkumpul: " + soalBenarTerkumpul + " / " + totalSoalDiLevel);

        // Jika target 8 soal benar tercapai di tengah permainan
        if (soalBenarTerkumpul >= totalSoalDiLevel)
        {
            SelesaikanLevel(true); // true = buka level 2
        }
    }

    // 2. Dipanggil ketika pemain menyentuh garis finish di ujung level (meskipun belum 8 soal)
    public void CapaiAkhirLevel()
    {
        Debug.Log("Pemain mencapai akhir level!");

        // Cek apakah syarat 8 soal sudah terpenuhi atau belum
        bool cukupSyarat = soalBenarTerkumpul >= totalSoalDiLevel;
        SelesaikanLevel(cukupSyarat);
    }

    // Logika Utama Penyelesaian Level
    void SelesaikanLevel(bool bukaLevelBerikutnya)
    {
        if (bukaLevelBerikutnya)
        {
            Debug.Log("Selamat! Target 8 soal tercapai. Level 2 Terbuka!");
            PlayerPrefs.SetInt("Level2_Unlocked", 1); // Membuka Level 2 di Select Menu
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Mencapai akhir level, tetapi belum 8 soal benar. Level 2 belum terbuka.");
        }

        // Munculkan panel pilihan (Level Clear) dan jeda permainan
        if (levelClearPanel != null)
        {
            levelClearPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            GoToSelectMenu("selectmenu"); // Fallback jika panel belum dipasang
        }
    }

    // --- FUNGSI UNTUK TOMBOL DI PANEL CLEAR ---

    // Hubungkan ke Tombol "Select Menu"
    public void GoToSelectMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    // Hubungkan ke Tombol "Restart / Ulangi Level"
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}