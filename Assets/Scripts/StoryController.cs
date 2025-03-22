using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI storyText;
    public Button continueButton;
    public Button skipButton;
    public Image fadePanel;

    [Header("Story Settings")]
    public float typingSpeed = 0.05f;
    public float delayBetweenPages = 1.0f;
    public string nextSceneName = "Main"; // Tên scene trò chơi chính

    [TextArea(5, 10)]
    public string[] storyPages;

    private int currentPage = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    [Header("Background Images")]
    public Image[] backgroundImages;
    public float imageTransitionSpeed = 1.0f;

    [Header("Audio")]
    public AudioClip[] pageSounds; // Âm thanh nền cho mỗi trang
    public AudioClip typingSound; // Âm thanh đánh máy
    public AudioSource audioSource; // AudioSource hiện tại

    [Header("Voice Over")]
    public AudioClip[] voiceClips; // Thêm voice clips cho mỗi trang
    public AudioSource voiceSource; // AudioSource riêng cho voice
    public float voiceVolume = 1.5f; // Âm lượng voice
    public bool useVoiceOver = true; // Tùy chọn bật/tắt voice

    private void Start()
    {
        // Ẩn nút Continue ban đầu
        continueButton.gameObject.SetActive(false);

        continueButton.transform.SetAsLastSibling();
        skipButton.transform.SetAsLastSibling();

        // Gán các sự kiện cho nút
        continueButton.onClick.AddListener(NextPage);
        skipButton.onClick.AddListener(SkipToGame);

        // Tạo AudioSource cho voice nếu chưa có
        if (voiceSource == null && useVoiceOver)
        {
            voiceSource = gameObject.AddComponent<AudioSource>();
            voiceSource.volume = voiceVolume;
            voiceSource.playOnAwake = false;
        }

        // Bắt đầu hiển thị trang đầu tiên
        ShowCurrentPage();

        // Thêm hai dòng này để hiển thị hình nền và âm thanh ban đầu
        TransitionBackground(0);
        PlayPageAudio(0);

    }

    private void ShowCurrentPage()
    {
        if (currentPage < storyPages.Length)
        {
            isTyping = true;
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeText(storyPages[currentPage]));

            // Phát voice-over nếu có
            PlayVoiceOver(currentPage);
        }
        else
        {
            StartCoroutine(FadeAndLoadGame());
        }
    }

    private void PlayVoiceOver(int pageIndex)
    {
        if (useVoiceOver && voiceSource != null && voiceClips != null && pageIndex < voiceClips.Length && voiceClips[pageIndex] != null)
        {
            // Tắt nhạc nền khi bắt đầu phát voice over
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PauseMusic();
            }

            // Tăng âm lượng voice over
            voiceSource.volume = voiceVolume;
            voiceSource.Stop();
            voiceSource.clip = voiceClips[pageIndex];
            voiceSource.Play();

            // Đăng ký event khi voice over kết thúc để bật lại nhạc
            StartCoroutine(ResumeBackgroundMusicAfterVoice(voiceClips[pageIndex].length));
        }
    }
    private IEnumerator ResumeBackgroundMusicAfterVoice(float delay)
    {
        yield return new WaitForSeconds(delay + 0.1f); // Thêm 0.1s để đảm bảo voice đã kết thúc

        // Kiểm tra xem có đang ở trang khác không (để tránh bật nhạc khi đang phát voice khác)
        if (!voiceSource.isPlaying && AudioManager.instance != null)
        {
            AudioManager.instance.ResumeMusic();
        }
    }

    private IEnumerator TypeText(string text)
    {
        storyText.text = "";
        continueButton.gameObject.SetActive(false);

        bool isTag = false;
        //bool isItalic = false;
        //bool isBold = false;

        foreach (char c in text)
        {
            // Xử lý các tag đặc biệt
            if (c == '<')
                isTag = true;
            else if (c == '>')
                isTag = false;

            // Chỉ thêm chữ và phát âm thanh nếu không phải tag
            if (!isTag)
            {
                storyText.text += c;

                // Phát âm thanh đánh máy
                if (audioSource != null && typingSound != null && c != ' ' && !char.IsPunctuation(c))
                {
                    audioSource.PlayOneShot(typingSound, Random.Range(0.1f, 0.2f));
                }

                // Tạm dừng lâu hơn sau câu
                if (c == '.' || c == '!' || c == '?')
                    yield return new WaitForSeconds(typingSpeed * 8);
                else if (c == ',')
                    yield return new WaitForSeconds(typingSpeed * 4);
                else
                    yield return new WaitForSeconds(typingSpeed);
            }
        }

        isTyping = false;
        continueButton.gameObject.SetActive(true);
    }

    public void NextPage()
    {
        if (isTyping)
        {
            // Nếu đang đánh chữ, hiển thị toàn bộ văn bản ngay lập tức
            StopCoroutine(typingCoroutine);
            storyText.text = storyPages[currentPage];
            isTyping = false;
            continueButton.gameObject.SetActive(true);

            // Dừng voice over hiện tại nếu đang skip
            if (voiceSource != null && voiceSource.isPlaying)
            {
                voiceSource.Stop();

                // Bật lại nhạc nền
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.ResumeMusic();
                }
            }
        }
        else
        {
            currentPage++;

            // Dừng voice over trước khi chuyển trang
            if (voiceSource != null && voiceSource.isPlaying)
            {
                voiceSource.Stop();
            }

            // Hiệu ứng fade giữa các trang
            StartCoroutine(FadeTextOut(() =>
            {
                ShowCurrentPage();
                PlayPageAudio(currentPage);
                TransitionBackground(currentPage);
            }));
        }
    }
    private IEnumerator FadeTextOut(System.Action onComplete)
    {
        // Code giữ nguyên
        float fadeTime = 0.5f;
        float startAlpha = storyText.color.a;
        float time = 0;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, time / fadeTime);
            storyText.color = new Color(storyText.color.r, storyText.color.g, storyText.color.b, alpha);
            yield return null;
        }

        if (onComplete != null)
            onComplete();

        time = 0;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, startAlpha, time / fadeTime);
            storyText.color = new Color(storyText.color.r, storyText.color.g, storyText.color.b, alpha);
            yield return null;
        }
    }

    private void SkipToGame()
    {
        // Dừng voice over khi skip
        if (voiceSource != null && voiceSource.isPlaying)
        {
            voiceSource.Stop();

            // Bật lại nhạc nền
            if (AudioManager.instance != null)
            {
                AudioManager.instance.ResumeMusic();
            }
        }

        StartCoroutine(FadeAndLoadGame());
    }


    private IEnumerator FadeAndLoadGame()
    {
        // Dừng voice over khi kết thúc
        if (voiceSource != null && voiceSource.isPlaying)
        {
            voiceSource.Stop();

            // Bật lại nhạc nền
            if (AudioManager.instance != null)
            {
                AudioManager.instance.ResumeMusic();
            }
        }

        // Code giữ nguyên
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            float alpha = 0;

            while (alpha < 1)
            {
                alpha += Time.deltaTime;
                fadePanel.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            Debug.LogWarning("fadePanel không được gán trong Inspector");
        }


        SceneManager.LoadScene(nextSceneName);
    }

    // Các phương thức khác giữ nguyên
    private void TransitionBackground(int pageIndex)
    {
        // Code giữ nguyên
        StartCoroutine(FadeBackground(pageIndex));

        foreach (Image img in backgroundImages)
        {
            img.transform.SetAsFirstSibling();
        }

        storyText.transform.SetAsLastSibling();
        continueButton.transform.SetAsLastSibling();
        skipButton.transform.SetAsLastSibling();
    }

    private IEnumerator FadeBackground(int index)
    {
        // Code giữ nguyên
        foreach (Image img in backgroundImages)
        {
            img.CrossFadeAlpha(0, 0, true);
        }

        if (index < backgroundImages.Length)
        {
            backgroundImages[index].gameObject.SetActive(true);
            backgroundImages[index].CrossFadeAlpha(1, imageTransitionSpeed, true);
        }

        yield return null;
    }

    private void PlayPageAudio(int pageIndex)
    {
        // Code giữ nguyên
        if (audioSource != null && pageIndex < pageSounds.Length && pageSounds[pageIndex] != null)
        {
            audioSource.Stop();
            audioSource.clip = pageSounds[pageIndex];
            audioSource.Play();
        }
    }
}