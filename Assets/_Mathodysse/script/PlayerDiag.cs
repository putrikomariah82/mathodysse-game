using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDiag : MonoBehaviour
{
    Rigidbody2D rb;
    int n;

    void Awake() { rb = GetComponent<Rigidbody2D>(); }

    void FixedUpdate()
    {
        var kb = Keyboard.current;
        if (kb == null || !(kb.dKey.isPressed || kb.aKey.isPressed)) return;
        if (++n % 25 != 0) return;

        var contacts = new Collider2D[10];
        int c = rb.GetContacts(contacts);
        string names = "";
        for (int i = 0; i < c; i++) names += contacts[i].name + ", ";

        Debug.Log("pos=" + rb.position + " vel=" + rb.linearVelocity +
                  " type=" + rb.bodyType + " sim=" + rb.simulated +
                  " constraints=" + rb.constraints +
                  " timeScale=" + Time.timeScale +
                  " physicsMode=" + Physics2D.simulationMode +
                  " kontak=" + c + " [" + names + "]");
    }
}