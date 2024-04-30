using UnityEngine;
using UnityEngine.UI;
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

    private void Start()
    {
        // 게임오버 화면 초기에 숨김 처리
        gameOverCanvasGroup.alpha = 0f;
        ShowGameOverScreen();
        textToBlink.DOFade(0f, blinkingDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("This is anyKeyDown.");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
            


    }

    public void ShowGameOverScreen()
    {
        StartCoroutine(FadeInGameOverScreen());

        gameOverText.DOFade(1f, textAppearDuration).From(0f);
        gameOverText.transform.DOScale(1.2f, textAppearDuration / 2f).From(1.125f).SetEase(Ease.OutBack);
    }

    private IEnumerator FadeInGameOverScreen()
    {
        // 페이드 인 효과
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
        // 페이드 아웃 효과
        while (gameOverCanvasGroup.alpha > 0f)
        {
            gameOverCanvasGroup.alpha -= Time.deltaTime / fadeOutDuration;
            yield return null;
        }
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.gameObject.SetActive(false);
    }


}
