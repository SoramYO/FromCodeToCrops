using UnityEngine;
using UnityEngine.UI;

public class FishingUIController : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;
    public GameObject fishingUIPanel;
    public FishingManager fishingManager;

    void Start()
    {
        yesButton.onClick.AddListener(YesFishing);
        noButton.onClick.AddListener(NoFishing);
        DontDestroyOnLoad(gameObject);
        
        // Tìm fishingManager nếu chưa được gán
        if (fishingManager == null)
        {
            fishingManager = FindObjectOfType<FishingManager>();
        }
    }
    void OnEnable()
    {
        // Tìm và gán lại FishingManager mỗi khi UI panel được hiển thị
        if (fishingManager == null)
        {
            // Tìm từ FishingSpotTrigger
            FishingSpotTrigger trigger = FindObjectOfType<FishingSpotTrigger>();
            if (trigger != null && trigger.fishingManager != null)
            {
                fishingManager = trigger.fishingManager;
            }
            else
            {
                // Tìm trực tiếp
                fishingManager = FindObjectOfType<FishingManager>();
            }
        }
    }

    void YesFishing()
    {
        fishingUIPanel.SetActive(false);
        if (fishingManager != null)
        {
            fishingManager.CatchFish();
        }
        else
        {
            Debug.LogError("FishingManager chưa được gán vào FishingUIController!");
        }
    }

    void NoFishing()
    {
        fishingUIPanel.SetActive(false);
        Debug.Log("Fishing canceled.");
    }
}
