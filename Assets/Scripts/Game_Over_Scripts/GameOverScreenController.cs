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

    bool gameover = false;

    public void Start()
    {
        // ���ӿ��� ȭ�� �ʱ⿡ ���� ó��
        gameOverCanvasGroup.alpha = 0f;
        //ShowGameOverScreen();
        //textToBlink.DOFade(0f, blinkingDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameover)
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

    public void ShowGameOverScreen()
    {
        // ���� �ð��� ����ϴ�.
        //Time.timeScale = 0f;

        // ���ӿ��� ȭ���� ǥ���մϴ�.
        StartCoroutine(FadeInGameOverScreen());

        gameover = true;

        // DOTween�� SetUpdate(true)�� ����Ͽ� Time.timeScale�� ������ ���� �ʵ��� �մϴ�.
        gameOverText.DOFade(1f, textAppearDuration).From(0f).SetUpdate(true);
        gameOverText.transform.DOScale(1.2f, textAppearDuration / 2f).From(1.125f).SetEase(Ease.OutBack).SetUpdate(true);
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
