using UnityEngine;
using UnityEngine.InputSystem;

public class PigInteraction : MonoBehaviour
{
    private bool playerInRange = false;
    private int interactionCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            UIController.instance.ShowMessage("Nhấn G để tương tác với Pig");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && (Keyboard.current.eKey.wasPressedThisFrame || Input.GetKeyDown(KeyCode.G)))
        {
            Debug.Log("👀 Player đã nhấn G để tương tác với Pig!");

            if (PigController.instance == null)
            {
                Debug.LogError("❌ PigController.instance bị null trong PigInteraction!");
                return;
            }

            if (PigController.instance.IsSadPig()) // Kiểm tra Pig có đang buồn không
            {
                interactionCount++;
                Debug.Log($"✅ Số lần tương tác với Pig: {interactionCount}");

                PigController.instance.CheerUpPig(); // Dỗ dành Pig

                if (interactionCount >= 3)
                {
                    Debug.Log("🎉 Đã đủ 3 lần tương tác, sẽ sinh Piglet!");
                    AddPiglet();
                    interactionCount = 0;
                }
            }
            else
            {
                Debug.Log("ℹ Pig hiện không buồn, không thể tương tác.");
            }
        }
    }


    void AddPiglet()
    {
        if (PigController.instance != null)
        {
            PigController.instance.AddPiglet();
        }

        PigDisplay pigDisplay = FindObjectOfType<PigDisplay>();
        if (pigDisplay != null)
        {
            pigDisplay.UpdateDisplay();
        }
    }
}
