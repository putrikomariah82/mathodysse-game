using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Wajib ada untuk merestart atau pindah scene

public class QuestionUI : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform questionPanel;
    public TMP_Text questionText;
    public Button[] optionButtons;

    [Header("Animasi Posisi Panel")]
    public float hiddenPosY = 800f;
    public float visiblePosY = 0f;
    public float animSpeed = 8f;

    [Header("=== FITUR NYAWA (LIVES) ===")]
    public int maxLives = 3;
    private int currentLives;
    public GameObject[] heartIcons; // Masukkan 3 UI Image Love Anda di sini lewat Inspector
    public GameObject gameOverPanel; // Panel Game Over (otomatis disembunyikan saat Start)

    bool isShowing = false;
    bool isAnswering = false;
    int correctAnswerIndex;
    System.Action<bool> onAnswered;

    void Start()
    {
        Time.timeScale = 1f;
        questionPanel.anchoredPosition = new Vector2(questionPanel.anchoredPosition.x, hiddenPosY);

        // Inisialisasi nyawa saat game mulai
        currentLives = maxLives;
        UpdateHearts();

        // OTOMATIS MENYEMBUNYIKAN GAME OVER PANEL SAAT PERMAINAN DIMULAI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        float targetY = isShowing ? visiblePosY : hiddenPosY;
        Vector2 targetPos = new Vector2(questionPanel.anchoredPosition.x, targetY);
        questionPanel.anchoredPosition = Vector2.Lerp(
            questionPanel.anchoredPosition, targetPos, Time.unscaledDeltaTime * animSpeed);
    }

    public void DisplayQuestion(string question, string[] options, int correctIndex, System.Action<bool> callback = null)
    {
        onAnswered = callback;
        questionText.text = question;
        correctAnswerIndex = correctIndex;
        isAnswering = false;

        string[] labels = { "A. ", "B. ", "C. ", "D. " };

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].interactable = true;
                optionButtons[i].image.color = Color.white;

                TMP_Text t = optionButtons[i].GetComponentInChildren<TMP_Text>();
                if (t != null) t.text = labels[i] + options[i];

                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }

        isShowing = true;
        Time.timeScale = 0f;
    }

    void OnAnswerSelected(int selectedIndex)
    {
        if (isAnswering) return;
        isAnswering = true;

        Debug.Log("Tombol diklik: " + selectedIndex);
        StartCoroutine(AnswerRoutine(selectedIndex));
    }

    IEnumerator AnswerRoutine(int selectedIndex)
    {
        foreach (var b in optionButtons) b.interactable = false;

        bool benar = selectedIndex == correctAnswerIndex;
        optionButtons[selectedIndex].image.color = benar ? Color.green : Color.red;

        if (benar)
        {
            Debug.Log("Jawaban Benar!");

            // Tambahkan baris ini agar progres 8 soal bertambah
            if (LevelProgress.Instance != null)
            {
                LevelProgress.Instance.TambahSoalBenar();
            }
        }
        else
        {
            Debug.Log("Jawaban Salah!");
            // KURANGI NYAWA KETIKA JAWABAN SALAH
            DeductLife();
        }

        yield return new WaitForSecondsRealtime(1f);

        foreach (var b in optionButtons)
        {
            b.image.color = Color.white;
            b.interactable = true;
        }

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        isShowing = false;
        isAnswering = false;
        Time.timeScale = 1f;

        ResetPlayerAnimator();

        // Kabari kotak soal (batu) apakah jawabannya benar
        onAnswered?.Invoke(benar);
        onAnswered = null;

        Debug.Log("Game lanjut, timeScale = " + Time.timeScale);
    }

    // --- FUNGSI PENGATUR NYAWA ---
    void DeductLife()
    {
        currentLives--;
        UpdateHearts();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (i < currentLives)
            {
                heartIcons[i].SetActive(true);  // Tampilkan ikon love
            }
            else
            {
                heartIcons[i].SetActive(false); // Sembunyikan ikon love yang habis
            }
        }
    }

    void GameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Memunculkan panel Game Over
            Time.timeScale = 0f; // Menghentikan waktu permainan
        }
        else
        {
            RestartGame();
        }
    }

    // --- FUNGSI TOMBOL GAME OVER ---

    // 1. Hubungkan ke tombol Restart / Ulangi Level
    public void RestartGame()
    {
        Time.timeScale = 1f; // Kembalikan waktu normal sebelum merestart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 2. Hubungkan ke tombol Pilih Level / Select Menu (Masukkan nama scene level select di Inspector)
    public void GoToSelectMenu(string selectMenuSceneName)
    {
        Time.timeScale = 1f; // Kembalikan waktu normal
        SceneManager.LoadScene(selectMenuSceneName);
    }

    void ResetPlayerAnimator()
    {
        var player = FindAnyObjectByType<Playercontroller>();
        if (player == null) return;

        var anim = player.GetComponent<Animator>();
        if (anim == null) return;

        anim.enabled = false;
        anim.enabled = true;
        anim.Rebind();
        anim.Update(0f);
    }
}