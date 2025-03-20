using UnityEngine;
using UnityEngine.InputSystem;

public class PigController : MonoBehaviour
{
    public static PigController instance;

    private int pigletCount = 0; // Số lượng Piglet
    private bool isSad = false; // Trạng thái Pig
    public Sprite pigletSprite; // Ảnh Piglet (Gán trong Inspector)
    public int pigletPrice = 50; // Giá bán Piglet
    private bool playerInRange;
    private int interactionCount;

    private void Awake()
    {
        if (FindObjectsOfType<InventoryController>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        if (instance == null)
        {
            instance = this;
            Debug.Log("✅ PigController đã được khởi tạo!");
        }
        else
        {
            Debug.LogWarning("⚠ Có nhiều hơn 1 PigController, đang xóa bớt.");
            Destroy(gameObject);
        }

    }


    private void Start()
    {
        if (pigletSprite == null)
        {
            Debug.LogError("⚠ pigletSprite chưa được gán trong PigController!");
        }
    }

    public bool IsSadPig()
    {
        Debug.Log($"🔍 Kiểm tra trạng thái Pig: {isSad}");
        return isSad;
    }

    public void SetSad(bool value)
    {
        isSad = value;
        Debug.Log($"😢 Pig trạng thái cập nhật: {isSad}");
    }

    public void CheerUpPig()
    {
        isSad = false;
        Debug.Log("Pig đã được dỗ dành!");
    }

    public void AddPiglet()
    {
        pigletCount++;
        Debug.Log($"Một Piglet mới được sinh ra! Tổng số Piglet: {pigletCount}");
    }

    public int GetPigletCount()
    {
        return pigletCount;
    }

    public bool SellPiglet()
    {
        if (pigletCount > 0)
        {
            pigletCount--;
            CurrencyController.instance?.AddMoney(pigletPrice); // Đảm bảo CurrencyController không bị null
            Debug.Log($"Bạn đã bán 1 Piglet! Còn lại: {pigletCount}");
            return true;
        }
        else
        {
            Debug.Log("Không có Piglet để bán!");
            return false;
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

            if (PigController.instance.IsSadPig()) // Kiểm tra nếu Pig đang buồn
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

}
