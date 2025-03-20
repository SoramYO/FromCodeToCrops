// Sửa lại HouseExitController.cs
using UnityEngine;
using System.Collections;

public class HouseExitController : MonoBehaviour
{
    private bool hasTriggeredForecast = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tránh kích hoạt nhiều lần
        if (hasTriggeredForecast) return;
        
        Debug.Log($"Collider đã phát hiện va chạm với: {collision.gameObject.name}, Tag: {collision.tag}");
        
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Va chạm với PLAYER đã được xác nhận");
            
            if (TimeController.instance != null)
            {
                // Nếu không có dự báo đang chờ, tạo một dự báo mới

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
        }
        
        // Reset để có thể kích hoạt lại nếu cần
        yield return new WaitForSeconds(5.0f);
        hasTriggeredForecast = false;
    }
}