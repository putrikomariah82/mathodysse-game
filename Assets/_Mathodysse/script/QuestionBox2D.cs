using UnityEngine;
using UnityEngine.EventSystems;

public class QuestionBox2D : MonoBehaviour
{
    [TextArea]
    public string question = "Berapa hasil dari 5 x 5?";
    public string[] options = { "20", "25", "30", "35" };
    public int correctAnswerIndex = 1;
    public QuestionUI questionUI;

    [Header("Setelah dijawab benar")]
    public bool hideOnCorrect = true;      // batu hilang kalau benar
    public GameObject breakEffect;         // opsional: partikel/efek (boleh kosong)

    bool hasTriggered = false;
    bool isSolved = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSolved || hasTriggered) return;

        if (collision.CompareTag("Player"))
        {
            hasTriggered = true;
            questionUI.DisplayQuestion(question, options, correctAnswerIndex, OnAnswered);
        }
    }

    void OnAnswered(bool benar)
    {
        Debug.Log("QuestionBox: jawaban " + (benar ? "benar" : "salah"));

        if (benar)
        {
            isSolved = true;

            if (breakEffect != null)
                Instantiate(breakEffect, transform.position, Quaternion.identity);

            if (hideOnCorrect)
            {
                // Nonaktifkan collider terlebih dahulu agar tidak nyangkut secara fisik
                foreach (var c in GetComponentsInChildren<Collider2D>())
                    c.enabled = false;

                // Sembunyikan sprite batu
                foreach (var r in GetComponentsInChildren<SpriteRenderer>())
                    r.enabled = false;
            }

            // Memastikan fokus UI dilepaskan agar input player kembali aktif
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
        else
        {
            hasTriggered = false; // salah: soal bisa muncul lagi kalau disentuh ulang
        }
    }
}