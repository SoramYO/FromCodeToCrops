using UnityEngine;
using UnityEngine.SceneManagement;

public class FishingSpotTrigger : MonoBehaviour
{
	public GameObject FishingUIPanel;
	public FishingManager fishingManager;
	private PlayerController playerController;

	// Singleton pattern để quản lý instance
	private static FishingSpotTrigger instance;

	private void Awake()
	{
		// Kiểm tra xem đã có instance chưa
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);

			// Đảm bảo UI panel và fishingManager cũng không bị destroy
			if (FishingUIPanel != null)
				DontDestroyOnLoad(FishingUIPanel);

			// Đảm bảo fishingManager cũng không bị destroy
			if (fishingManager != null && fishingManager.gameObject != gameObject)
				DontDestroyOnLoad(fishingManager.gameObject);

			// Đăng ký sự kiện khi scene load để reset trạng thái nếu cần
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
		else
		{
			// Nếu đã có instance, hủy instance mới
			Destroy(gameObject);
		}
	}
	private void OnEnable()
	{
		// Nếu fishingManager bị mất sau khi chuyển cảnh, tìm lại nó
		if (fishingManager == null)
		{
			fishingManager = FindObjectOfType<FishingManager>();
		}
	}

	private void OnDestroy()
	{
		// Hủy đăng ký sự kiện khi object bị destroy
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// Reset trạng thái khi load scene mới
		ResetFishingState();
	}

	private void ResetFishingState()
	{
		if (playerController != null)
		{
			playerController.ExitFishingArea();
			playerController = null;
		}

		if (FishingUIPanel != null)
			FishingUIPanel.SetActive(false);

		if (fishingManager != null)
			fishingManager.CancelFishing();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playerController = other.GetComponent<PlayerController>();
			if (playerController != null)
			{
				playerController.EnterFishingArea();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (playerController != null)
			{
				playerController.ExitFishingArea();
				playerController = null;
			}

			FishingUIPanel.SetActive(false);
			fishingManager.CancelFishing();
		}
	}

	void Update()
	{
		if (playerController != null && Input.GetKeyDown(KeyCode.Alpha5))
		{
			FishingUIPanel.SetActive(true);
		}
	}

	public void FishYes()
	{
		FishingUIPanel.SetActive(false);
		if (playerController != null)
		{
			playerController.StartFishing();
		}
	}

	public void FishNo()
	{
		FishingUIPanel.SetActive(false);
	}
}