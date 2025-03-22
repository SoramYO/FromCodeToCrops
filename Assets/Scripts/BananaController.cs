using UnityEngine;

using UnityEngine;

public class BananaController : MonoBehaviour
{
    public static BananaController instance; // Singleton instance

    private int bananaCount = 0; // Số lượng chuối
    public Sprite bananaSprite; // Sprite chuối (gán trong Inspector)

    private void Awake()
    {
        // Đảm bảo chỉ có một instance của BananaController
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddBanana(int amount)
    {
        bananaCount += amount;
        Debug.Log($"Đã thêm {amount} chuối. Tổng số chuối: {bananaCount}");

        // Cập nhật hiển thị UI
        FindObjectOfType<BananaDisplay>()?.UpdateDisplay();
    }

    public int GetBananaCount()
    {
        return bananaCount;
    }
}