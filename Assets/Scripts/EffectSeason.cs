using UnityEngine;

public class EffectSeason : MonoBehaviour
{
	public static EffectSeason instance;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public ParticleSystem snowflakeEffect;
	public ParticleSystem leafEffect;
	public ParticleSystem blossomEffect;
	private bool _effectsEnabled = true;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	void AdjustParticleSettings(ParticleSystem ps, float size, float emissionRate)
	{
		if (ps != null)
		{
			var main = ps.main;
			main.startSize = size;

			var emission = ps.emission;
			emission.rateOverTime = emissionRate;
		}
	}
	public void UpdateSeasonEffects()
	{
		// Không làm gì nếu hiệu ứng đang tắt (đang trong nhà)
		if (!_effectsEnabled)
		{
			Debug.Log("Hiệu ứng đang tắt, không cập nhật");
			return;
		}

		// Tắt tất cả các hiệu ứng trước
		DisableAllEffects();

		// Bật hiệu ứng tương ứng với mùa hiện tại
		Season currentSeason = (Season)SeasonSystem.instance.currentSeason;
		Debug.Log($"Cập nhật hiệu ứng cho mùa: {currentSeason}");

		switch (currentSeason)
		{
			case Season.Spring:
				blossomEffect?.Play();
				break;
			case Season.Summer:
				leafEffect?.Play();
				break;
			case Season.Autumn:
				leafEffect?.Play();
				break;
			case Season.Winter:
				snowflakeEffect?.Play();
				break;
		}
	}
	public void SetEffectsActive(bool active)
	{
		_effectsEnabled = active;
		Debug.Log($"SetEffectsActive được gọi: {active}");

		if (!active)
		{
			// Tắt tất cả hiệu ứng khi vào nhà
			DisableAllEffects();
		}
		else
		{
			// Bật lại hiệu ứng theo mùa hiện tại nếu ra ngoài
			if (SeasonSystem.instance != null)
			{
				UpdateSeasonEffects();
			}
		}
	}
	private void DisableAllEffects()
	{
		blossomEffect?.Stop();
		leafEffect?.Stop();
		snowflakeEffect?.Stop();
		Debug.Log("Đã tắt tất cả hiệu ứng mùa");
	}

}
public enum Season
{
	Spring,
	Summer,
	Autumn,
	Winter
}
