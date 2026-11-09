using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;

    private float moveInput;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Keyboard kb = Keyboard.current;

        // Reset input setiap frame
        moveInput = 0f;

        // =========================
        // INPUT GERAK
        // =========================
        if (kb != null)
        {
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed)
            {
                moveInput = 1f;
            }
            else if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)
            {
                moveInput = -1f;
            }
        }

        // =========================
        // CEK GROUND
        // =========================
        if (col != null)
        {
            ContactFilter2D filter = new ContactFilter2D
            {
                useTriggers = false
            };

            RaycastHit2D[] hits = new RaycastHit2D[5];

            isGrounded = col.Cast(
                Vector2.down,
                filter,
                hits,
                0.05f
            ) > 0;
        }

        // =========================
        // JUMP
        // =========================
        if (kb != null &&
            kb.spaceKey.wasPressedThisFrame &&
            isGrounded &&
            rb != null)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );
        }

        // =========================
        // FLIP SPRITE
        // =========================
        if (sr != null)
        {
            if (moveInput > 0)
            {
                sr.flipX = false;
            }
            else if (moveInput < 0)
            {
                sr.flipX = true;
            }
        }

        // =========================
        // ANIMATOR
        // =========================
        SetSpeedAnimation(
            Mathf.Abs(moveInput)
        );
    }

    private void FixedUpdate()
    {
        if (rb == null)
            return;

        // Gerakan horizontal
        rb.linearVelocity = new Vector2(
            moveInput * speed,
            rb.linearVelocity.y
        );
    }

    // =========================================================
    // DIPANGGIL SETELAH SOAL SELESAI
    // =========================================================

    public void ResumeMovement()
    {
        // Pastikan Playercontroller aktif
        enabled = true;

        // Pastikan Rigidbody2D aktif
        if (rb != null)
        {
            rb.simulated = true;

            // Hanya reset kecepatan horizontal.
            // Velocity Y tetap dipertahankan agar player
            // tidak tiba-tiba berhenti jatuh/lompat.
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );
        }

        // Pastikan Animator aktif
        if (anim != null)
        {
            anim.enabled = true;
        }

        // Reset input
        moveInput = 0f;

        // Reset animasi
        SetSpeedAnimation(0f);
    }

    // =========================================================
    // ANIMATOR
    // =========================================================

    private void SetSpeedAnimation(float value)
    {
        if (anim == null)
            return;

        foreach (AnimatorControllerParameter parameter
                 in anim.parameters)
        {
            if (parameter.name == "Speed" &&
                parameter.type ==
                AnimatorControllerParameterType.Float)
            {
                anim.SetFloat("Speed", value);
                return;
            }
        }
    }
}