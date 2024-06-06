using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class GameOverScreenController : MonoBehaviour
{
    public CanvasGroup gameOverCanvasGroup;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    public float textAppearDuration = 1f;

    public float blinkingDuration = 0.5f;
    public float blinkingInterval = 0.5f;

    public Text gameOverText;
    public Text textToBlink;

    public AudioSource gameOverSound;
    public Image screenOverlay;

    bool isGameover = false;

    public void Start()
    {
        // ���ӿ��� ȭ�� �ʱ⿡ ���� ó��
        gameOverCanvasGroup.alpha = 0f;
        //ShowGameOverScreen();
        //textToBlink.DOFade(0f, blinkingDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    public void Update()
    {
        if (isGameover && screenOverlay.color.a < 0.35f)
        {
            float alphaChange = Time.deltaTime / fadeInDuration;
            float newAlpha = Mathf.Min(screenOverlay.color.a + alphaChange, 0.35f); // 최대 투명도를 0.35로 제한
            screenOverlay.color = new Color(1, 0, 0, newAlpha);
        }


        if (Input.GetMouseButtonDown(0) && isGameover)
        {
            Debug.Log("This is anyKeyDown.");
            //Time.timeScale = 1f;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene("UI");
        }
    }

    //public void ShowGameOverScreen()
    //{
    //    StartCoroutine(FadeInGameOverScreen());
    //    gameover = true;

    //    gameOverText.DOFade(1f, textAppearDuration).From(0f);
    //    gameOverText.transform.DOScale(1.2f, textAppearDuration / 2f).From(1.125f).SetEase(Ease.OutBack);
    //    textToBlink.DOFade(0f, blinkingDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    //}

    // 게임 오버 상태를 설정하고 게임 오버 화면을 표시하는 함수
    public void TriggerGameOver()
    {
        // 게임 오버 사운드 재생
        gameOverSound.Play();

        // 게임 오버 상태를 true로 설정
        isGameover = true;

        // 게임 오버 화면 표시
        ShowGameOverScreen();
    }

    public void ShowGameOverScreen()
    {
        // ���� �ð��� ����ϴ�.
        //Time.timeScale = 0f;

        // ���ӿ��� ȭ���� ǥ���մϴ�.
        StartCoroutine(FadeInGameOverScreen());

        //isGameover = true;

        // DOTween�� SetUpdate(true)�� ����Ͽ� Time.timeScale�� ������ ���� �ʵ��� �մϴ�.
        gameOverText.DOFade(1f, textAppearDuration).From(0f).SetUpdate(true);
        gameOverText.transform.DOScale(1.2f, textAppearDuration).From(1.0f).SetEase(Ease.OutBack).SetUpdate(true);
        textToBlink.DOFade(0f, blinkingDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetUpdate(true);
    }

    private IEnumerator FadeInGameOverScreen()
    {
        // ���̵� �� ȿ��
        gameOverCanvasGroup.gameObject.SetActive(true);
        while (gameOverCanvasGroup.alpha < 1f)
        {
            gameOverCanvasGroup.alpha += Time.deltaTime / fadeInDuration;
            yield return null;
        }
        gameOverCanvasGroup.alpha = 1f;
    }

    public void HideGameOverScreen()
    {
        StartCoroutine(FadeOutGameOverScreen());
    }

    private IEnumerator FadeOutGameOverScreen()
    {
        // ���̵� �ƿ� ȿ��
        while (gameOverCanvasGroup.alpha > 0f)
        {
            gameOverCanvasGroup.alpha -= Time.deltaTime / fadeOutDuration;
            yield return null;
        }
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.gameObject.SetActive(false);
    }
}