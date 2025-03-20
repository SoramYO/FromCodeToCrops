using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class PigMove : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeTargetTime = 3f;
    public float moveRadius = 3f;

    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip walkSound; // Âm thanh di chuyển

    // 🎭 Cảm xúc
    public SpriteRenderer emotionRenderer;
    public Sprite sadIcon;
    public Sprite heartIcon;
    private bool isSad = false;

//    private Vector2 defaultPosition = new Vector2(-5.368f, -11.558f);

//    void Awake()
//{
//    if (FindObjectsOfType<PigMove>().Length > 2)
//    {
//        Destroy(gameObject);
//        return;
//    }
//        transform.position = defaultPosition;
//        DontDestroyOnLoad(gameObject);
//}


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        StartCoroutine(ChangeTargetRoutine());
        rb.freezeRotation = true;
        audioSource = GetComponent<AudioSource>();
        AudioManager.instance.sfx = AudioManager.instance.sfx.Append(audioSource).ToArray();

        //transform.position = defaultPosition;

        // Load trạng thái từ PlayerPrefs
        isSad = PlayerPrefs.GetInt("PigIsSad", 0) == 1;
        int interactions = PlayerPrefs.GetInt("PigInteractions", 0);

        if (isSad)
        {
            ShowEmotion(sadIcon);
        }
        else
        {
            emotionRenderer.gameObject.SetActive(false);
        }

        // Đặt lịch Pig sẽ buồn sau thời gian ngẫu nhiên nếu chưa buồn
        if (!isSad)
        {
            Invoke(nameof(BecomeSad), Random.Range(10f, 20f));
        }
    }

    void Update()
    {
        MoveToTarget();
        // Nếu Pig đang buồn và người chơi ấn "G" khi đứng gần
        if (isSad && Input.GetKeyDown(KeyCode.G) && PlayerIsNear())
        {
            CheerUp();
        }
    }

    void MoveToTarget()
    {
        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance > 0.2f)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;


            if (Mathf.Abs(direction.x) > 0.1f)
            {
                spriteRenderer.flipX = direction.x > 0;
            }

            animator.SetBool("isWalking", true);
            if (!audioSource.isPlaying)
            {
                audioSource.clip = walkSound;
                audioSource.volume = 3.0f;
                audioSource.Play();
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
    }

    IEnumerator ChangeTargetRoutine()
    {
        while (true)
        {
            ChangeTargetPosition();
            animator.SetBool("isWalking", true);

            yield return new WaitForSeconds(changeTargetTime);

            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);

            yield return new WaitForSeconds(4f);
        }
    }

    void ChangeTargetPosition()
    {
        Vector2 newTarget;
        do
        {
            float x = transform.position.x + Random.Range(-moveRadius, moveRadius);
            float y = transform.position.y + Random.Range(-moveRadius, moveRadius);
            newTarget = new Vector2(x, y);
        } while (Vector2.Distance(newTarget, transform.position) < 0.5f);

        targetPosition = newTarget;
    }

    // 😢 Pig trở nên buồn
    void BecomeSad()
    {
        if (isSad) return;

        isSad = true;
        PlayerPrefs.SetInt("PigIsSad", 1); // Lưu trạng thái Pig buồn
        PlayerPrefs.Save();

        PigController.instance.SetSad(true);
        Debug.Log("😢 Pig đã chuyển sang trạng thái buồn!");
        ShowEmotion(sadIcon);

        Invoke(nameof(ScheduleBecomeSad), Random.Range(10f, 20f));
    }
    void ScheduleBecomeSad()
    {
        if (!isSad) // Chỉ chạy nếu Pig không còn buồn
        {
            BecomeSad();
        }
    }

    // 💖 Người chơi dỗ dành Pig
    void CheerUp()
    {
        isSad = false;
        PlayerPrefs.SetInt("PigIsSad", 0); // Lưu trạng thái Pig vui trở lại
        PlayerPrefs.Save();

        ShowEmotion(heartIcon);
        StartCoroutine(HideEmotionAfterSeconds(2f));

        CancelInvoke(nameof(ScheduleBecomeSad));
        Invoke(nameof(ScheduleBecomeSad), Random.Range(20f, 40f));
    }

    // 📌 Hiển thị icon cảm xúc
    void ShowEmotion(Sprite icon)
    {
        emotionRenderer.sprite = icon;
        emotionRenderer.gameObject.SetActive(true);
    }

    IEnumerator HideEmotionAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        emotionRenderer.gameObject.SetActive(false);
    }

    // 📏 Kiểm tra người chơi có đứng gần không
    bool PlayerIsNear()
    {
        return Vector2.Distance(PlayerController.instance.transform.position, transform.position) < 2f;
    }
}
