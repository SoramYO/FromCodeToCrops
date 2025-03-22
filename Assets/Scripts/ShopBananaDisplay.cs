using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBananaDisplay : MonoBehaviour
{
	[Header("UI Elements")]

	public TMP_Text amountText; // Text hiển thị số lượng chuối
	public TMP_Text priceText; // Text hiển thị giá chuối
	public Button sellButton; // Nút bán chuối

	[Header("Banana Info")]
	public int bananaPrice = 5; // Giá mỗi quả chuối

	private void Start()
	{
		// Kiểm tra xem BananaController đã được khởi tạo chưa
		if (BananaController.instance == null)
		{
			Debug.LogError("⚠ BananaController.instance bị null trong ShopBananaDisplay!");
			return;
		}


		// Cập nhật hiển thị UI
		UpdateDisplay();

		// Gán sự kiện cho nút bán chuối
		if (sellButton != null)
		{
			sellButton.onClick.AddListener(SellBanana);
		}
	}

	public void UpdateDisplay()
	{
		// Kiểm tra cả BananaController.instance và amountText
		if (BananaController.instance != null)
		{
			if (amountText != null)
			{
				amountText.text = "x" + BananaController.instance.GetBananaCount();
			}
			else
			{
				Debug.LogWarning("amountText chưa được gán trong Inspector");
			}

			if (priceText != null)
			{
				priceText.text = "$" + bananaPrice + " each";
			}
			else
			{
				Debug.LogWarning("priceText chưa được gán trong Inspector");
			}
		}
		else
		{
			Debug.LogWarning("BananaController.instance là null trong UpdateDisplay");
		}
	}

	public void SellBanana()
	{
		if (BananaController.instance != null && BananaController.instance.GetBananaCount() > 0)
		{
			// Trừ 1 chuối khỏi inventory (đang trừ 10 chuối, đổi thành -1 nếu muốn bán 1 chuối)
			BananaController.instance.AddBanana(-1); // Sửa từ -10 thành -1

			// Cộng tiền - SỬA DÒNG NÀY
			if (CurrencyController.instance != null) // Sử dụng CurrencyController thay vì MoneyManager
			{
				CurrencyController.instance.AddMoney(bananaPrice);

				// Phát âm thanh bán hàng (tùy chọn)
				AudioManager.instance?.PlaySFXPitchAdjusted(5);
			}

			// Cập nhật UI
			UpdateDisplay();
		}
		else
		{
			Debug.Log("Không đủ chuối để bán!");
			if (UIController.instance != null)
			{
				UIController.instance.ShowMessage("You don't have any bananas to sell!");
			}
		}
	}
}