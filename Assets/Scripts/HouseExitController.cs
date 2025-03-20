using UnityEngine;
using System.Collections;

public class HouseExitController : MonoBehaviour
{
    private bool hasTriggeredForecast = false;
    
    [Tooltip("True nếu đây là lối ra khỏi nhà, False nếu là lối vào nhà")]
    public bool isExitPoint = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tránh kích hoạt nhiều lần
        if (hasTriggeredForecast) return;
        
        Debug.Log($"Collider đã phát hiện va chạm với: {collision.gameObject.name}, Tag: {collision.tag}");
        
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Va chạm với PLAYER đã được xác nhận");
            
            // Kiểm tra nếu đây là lối ra khỏi nhà
            if (isExitPoint)
            {
                // Bật hiệu ứng mùa khi ra khỏi nhà
                if (EffectSeason.instance != null)
                {
                    EffectSeason.instance.SetEffectsActive(true);
                    Debug.Log("Hiệu ứng mùa đã được bật khi ra khỏi nhà");
                }
            }
            else
            {
                // Tắt hiệu ứng mùa khi vào nhà
                if (EffectSeason.instance != null)
                {
                    EffectSeason.instance.SetEffectsActive(false);
                    Debug.Log("Hiệu ứng mùa đã được tắt khi vào nhà");
                }
            }
            
            // Hiển thị dự báo thời tiết khi đi ra ngoài
            if (isExitPoint && TimeController.instance != null)
            {
                // Đánh dấu đã kích hoạt để tránh kích hoạt nhiều lần
                hasTriggeredForecast = true;
                StartCoroutine(ShowForecastWithDelay());
            }
        }
    }
    
    private IEnumerator ShowForecastWithDelay()
    {
        // Đợi người chơi di chuyển khỏi cửa một chút
        yield return new WaitForSeconds(1.0f);
        
        if (TimeController.instance != null)
        {
            Debug.Log("Hiển thị dự báo sau khi đợi");
            // Thêm code hiển thị dự báo thời tiết tại đây nếu có
            
            // Ví dụ:
            // UIController.instance.ShowMessage("Hôm nay trời nắng đẹp!");
        }
        
        // Reset để có thể kích hoạt lại nếu cần
        yield return new WaitForSeconds(5.0f);
        hasTriggeredForecast = false;
    }
}