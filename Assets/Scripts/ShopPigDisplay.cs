using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPigDisplay : MonoBehaviour
{
    public Image pigImage;
    public TMP_Text amountText, priceText;

    private void Start()
    {
        if (PigController.instance == null)
        {
            Debug.LogError("⚠ PigController.instance bị null trong ShopPigDisplay!");
            return;
        }

        if (PigController.instance.pigletSprite == null)
        {
            Debug.LogError("⚠ pigletSprite chưa được gán trong Inspector!");
            return;
        }

        pigImage.sprite = PigController.instance.pigletSprite;
        priceText.text = "$" + PigController.instance.pigletPrice;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (PigController.instance != null)
        {
            amountText.text = "x" + PigController.instance.GetPigletCount();
        }
    }

    public void SellPiglet()
    {
        if (PigController.instance.SellPiglet())
        {
            UpdateDisplay();
        }
    }
}
