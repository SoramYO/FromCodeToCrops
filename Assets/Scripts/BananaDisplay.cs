using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BananaDisplay : MonoBehaviour
{
    public Image bananaImage; // Hình ảnh chuối (gán trong Inspector)
    public TMP_Text amountText; // Text hiển thị số lượng chuối

    private void Start()
    {
        // Kiểm tra xem BananaController đã được khởi tạo chưa
        if (BananaController.instance == null)
        {
            Debug.LogError("⚠ BananaController.instance bị null trong BananaDisplay!");
            return;
        }

        // Kiểm tra xem sprite chuối đã được gán chưa
        if (BananaController.instance.bananaSprite == null)
        {
            Debug.LogError("⚠ bananaSprite chưa được gán trong BananaController!");
            return;
        }

        // Gán sprite chuối vào UI
        bananaImage.sprite = BananaController.instance.bananaSprite;

        // Cập nhật hiển thị số lượng chuối
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        // Lấy số lượng chuối từ BananaController và cập nhật UI
        if (BananaController.instance != null)
        {
            amountText.text = "x" + BananaController.instance.GetBananaCount();
        }
    }
}