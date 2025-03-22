using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BananaTreeController : MonoBehaviour
{
    [Header("Banana Settings")]
    public float harvestCooldown = 30f; // Thời gian chờ để hái lại (giây)
    private float currentCooldown = 0f;
    private bool isReadyToHarvest = true;

    [Header("UI Elements")]
    public GameObject timerUI; // UI hiển thị thời gian chờ
    public TMP_Text timerText; // Text hiển thị số giây còn lại

    private bool playerInRange = false; // Kiểm tra xem Player có ở gần cây không

    private void Start()
    {
        // Ẩn UI thời gian chờ ban đầu
        if (timerUI != null)
            timerUI.SetActive(false);
    }

    private void Update()
    {
        // Nếu cây đang trong thời gian chờ, giảm thời gian chờ
        if (!isReadyToHarvest)
        {
            currentCooldown -= Time.deltaTime;

            // Cập nhật UI thời gian chờ
            if (timerUI != null && timerText != null)
            {
                timerText.text = Mathf.Ceil(currentCooldown).ToString();
            }

            // Khi hết thời gian chờ, cho phép hái lại
            if (currentCooldown <= 0)
            {
                isReadyToHarvest = true;
                currentCooldown = 0;

                // Ẩn UI thời gian chờ
                if (timerUI != null)
                    timerUI.SetActive(false);
            }
        }

        // Kiểm tra nếu Player nhấn phím E để hái chuối
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            HarvestBanana();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

            if (timerUI != null)
                timerUI.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            // Ẩn UI khi người chơi rời khỏi cây
            if (timerUI != null)
                timerUI.SetActive(false);
        }
    }

    public void HarvestBanana()
    {
        if (isReadyToHarvest)
        {
            Debug.Log("Player đã hái chuối!");
            UIController.instance.ShowMessage("You picked bananas!");

            // Bắt đầu thời gian chờ
            isReadyToHarvest = false;
            currentCooldown = harvestCooldown;

            // Hiển thị thời gian chờ
            if (timerUI != null)
                timerUI.SetActive(true);

            // Thêm chuối vào inventory
            if (BananaController.instance != null)
            {
                BananaController.instance.AddBanana(1); // Thêm 1 chuối
            }
        }
        else
        {
            Debug.Log("Cây chuối chưa sẵn sàng để hái!");
        }
    }
}