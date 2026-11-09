using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ClickDebug : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current == null) { Debug.Log("Mouse.current NULL"); return; }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current == null) { Debug.Log("EventSystem.current NULL"); return; }

            var data = new PointerEventData(EventSystem.current);
            data.position = Mouse.current.position.ReadValue();

            var hasil = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, hasil);

            Debug.Log("Klik di " + data.position + ", objek kena: " + hasil.Count);
            foreach (var r in hasil) Debug.Log(" - " + r.gameObject.name);
        }
    }
}