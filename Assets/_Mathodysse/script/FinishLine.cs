using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika yang menyentuh kotak ini adalah Player
        if (collision.CompareTag("Player"))
        {
            // Panggil fungsi akhir level dari skrip LevelProgress
            if (LevelProgress.Instance != null)
            {
                LevelProgress.Instance.CapaiAkhirLevel();
            }
        }
    }
}