using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // Đăng ký sự kiện để nhận thông báo khi scene được tải
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện khi object bị hủy
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Phương thức mới để xử lý khi scene được tải
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"TimeController: Scene {scene.name} loaded");
        
        // Kiểm tra nếu đang quay lại từ màn hình ngủ
        if ((scene.name == "Main" || scene.name == "House") && 
            PlayerPrefs.GetString("Transition", "") == "Wake Up")
        {
            Debug.Log("TimeController: Waking up, resetting time and enabling time progression");
            PlayerPrefs.DeleteKey("Transition"); // Xóa transition để không tái kích hoạt
            
            // Đặt thời gian về sáng và kích hoạt thời gian
            currentTime = dayStart;
            timeActive = true;
            
            // Cập nhật UI nếu cần
            if (UIController.instance != null)
            {
                UIController.instance.UpdateTimeText(currentTime);
            }
        }
    }

    public float currentTime;
    public float dayStart, dayEnd;
    public float timeSpeed = .25f;
    [SerializeField] // Thêm SerializeField để có thể theo dõi trong Inspector
    private bool timeActive;
    public int currentDay = 1;
    public string dayEndScene;

    void Start()
    {
        currentTime = dayStart;
        timeActive = true;
    }

    void Update()
    {
        if (timeActive == true)
        {
            currentTime += Time.deltaTime * timeSpeed;

            if (currentTime > dayEnd)
            {
                currentTime = dayEnd;
                EndDay();
            }

            if (UIController.instance != null)
            {
                UIController.instance.UpdateTimeText(currentTime);
            }
        }
    }

    public void EndDay()
    {
        timeActive = false;
        currentDay++;
        GridInfo.instance.GrowCrop();

        PlayerPrefs.SetString("Transition", "Wake Up");
        if (SeasonSystem.instance != null)
        {
            SeasonSystem.instance.NewDay();
        }
        
        // Reset time for the new day
        currentTime = dayStart;

        

        // Optionally update any UI that shows time
        if(UIController.instance != null) 
        {
            UIController.instance.UpdateTimeText(currentTime);
        }
        StartDay();
        SceneManager.LoadScene(dayEndScene);
    }

    public void StartDay()
    {
        Debug.Log("StartDay called - activating time");
        timeActive = true;
        currentTime = dayStart;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(6);
        }
    }
}