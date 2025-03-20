using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PigDisplay : MonoBehaviour
{
    public Image pigImage;
    public TMP_Text amountText;

    private void Start()
    {
        if (PigController.instance == null)
        {
            Debug.LogError("⚠ PigController.instance bị null trong PigDisplay!");
            return;
        }

        if (PigController.instance.pigletSprite == null)
        {
            Debug.LogError("⚠ pigletSprite chưa được gán trong Inspector!");
            return;
        }

        pigImage.sprite = PigController.instance.pigletSprite;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (PigController.instance != null)
        {
            amountText.text = "x" + PigController.instance.GetPigletCount();
        }
    }
}
