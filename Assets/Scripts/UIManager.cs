using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InteractionHoverChangeListener))]
[RequireComponent(typeof(GameStartListener),
                  typeof(GameSuspendListener),
                  typeof(GameEndListener))]
[RequireComponent(typeof(NotificationListener),
                  typeof(PlayerHealthChangeListener),
                  typeof(GunAmmoChangeListener))]
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject tooltip;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject hudPanel;
    [SerializeField] TMPro.TextMeshProUGUI notificationText;
    [SerializeField] TMPro.TextMeshProUGUI goalText;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject endPanel;
    [SerializeField] GameObject diedPanel;
    [SerializeField] GameObject hintPanel;
    [SerializeField] Image healthImage;
    [SerializeField] Sprite[] healthSprites;
    [SerializeField] TMPro.TextMeshProUGUI ammoText;

    // Listeners
    GameStartListener gameStartListener;
    GameSuspendListener gameSuspendListener;
    GameEndListener gameEndListener;
    InteractionHoverChangeListener interactionHoverChangeListener;
    NotificationListener notificationListener;
    PlayerHealthChangeListener playerHealthChangeListener;
    GunAmmoChangeListener gunAmmoChangeListener;

    // Start is called before the first frame update
    void Start()
    {
        tooltip.SetActive(false);
        loadingPanel.SetActive(true);
        hudPanel.SetActive(false);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        diedPanel.SetActive(false);
        notificationText.gameObject.SetActive(false);
        goalText.gameObject.SetActive(true);

        goalText.text = $"Hints: {0}/{2}";

        Utils.GetListener(this, out gameStartListener);
        gameStartListener.Register(() => ToggleHUDPanel(true));

        Utils.GetListener(this, out gameSuspendListener);
        gameSuspendListener.Register(TogglePausePanel);

        Utils.GetListener(this, out gameEndListener);
        gameEndListener.Register(() => ToggleEndPanel(true));

        Utils.GetListener(this, out interactionHoverChangeListener);
        interactionHoverChangeListener.Register(ToggleInteractPrompt);

        Utils.GetListener(this, out notificationListener);
        notificationListener.Register(ShowNotification);

        Utils.GetListener(this, out playerHealthChangeListener);
        playerHealthChangeListener.Register((int value) =>
        {
            ChangeHealthBar(value);

            if (value <= 0)
            {
                gameSuspendListener.baseEvent.Raise(true);

                loadingPanel.SetActive(false);
                hudPanel.SetActive(false);
                pausePanel.SetActive(false);
                endPanel.SetActive(false);
                diedPanel.SetActive(true);
            }
        });

        Utils.GetListener(this, out gunAmmoChangeListener);
        gunAmmoChangeListener.Register(UpdateAmmoText);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowGoal((int, int) ratioTuple)
    {
        goalText.text = $"Hints: {ratioTuple.Item1}/{ratioTuple.Item2}";
    }

    public void ShowGoal(string text)
    {
        goalText.text = text;
    }

    public void ShowNotification(string text)
    {
        StartCoroutine(ProcessNotification(text));
    }

    IEnumerator ProcessNotification(string text)
    {
        notificationText.gameObject.SetActive(true);
        notificationText.text = text;

        float duration = 0;
        float originalPosition = notificationText.rectTransform.position.y;
        float startPosition = originalPosition - 50;
        while (duration <= 2f)
        {
            duration += Time.deltaTime;
            notificationText.rectTransform.position = new Vector3(
                notificationText.rectTransform.position.x,
                Mathf.Lerp(startPosition, originalPosition, duration / 2f),
                notificationText.rectTransform.position.z);
            yield return null;
        }

        duration = 0f;
        while (duration <= 1f)
        {
            duration += Time.deltaTime;
            yield return null;
        }

        duration = 0f;
        while (duration <= 1f)
        {
            duration += Time.deltaTime;
            notificationText.alpha = Mathf.Lerp(1f, 0f, duration / 1f);
            yield return null;
        }

        notificationText.gameObject.SetActive(false);
        notificationText.alpha = 1f;

        yield return null;
    }

    void ToggleInteractPrompt(bool value)
    {
        tooltip.SetActive(value);
    }

    void ToggleHUDPanel(bool value)
    {
        loadingPanel.SetActive(false);
        hudPanel.SetActive(value);
    }

    void TogglePausePanel(bool value)
    {
        hudPanel.SetActive(!value);
        endPanel.SetActive(false);

        bool isHint = false;
        // First check if game suspension is because of hints
        if (value)
        {
            foreach (Transform hint in hintPanel.transform)
            {
                if (hint.gameObject.activeInHierarchy)
                {
                    isHint = true;
                    break;
                }
            }

            if (!isHint)
                pausePanel.SetActive(value);

            return;
        }

        foreach (Transform hint in hintPanel.transform)
        {
            if (hint.gameObject.activeInHierarchy)
            {
                hint.gameObject.SetActive(false);
                isHint = true;
            }
        }

        if (!isHint)
            pausePanel.SetActive(value);

    }

    void ToggleEndPanel(bool value)
    {
        hudPanel.SetActive(!value);
        pausePanel.SetActive(!value);
        endPanel.SetActive(value);
    }

    void ChangeHealthBar(int value)
    {
        value = Math.Clamp(value, 0, 10);

        healthImage.sprite = healthSprites[value];
    }

    void UpdateAmmoText((int, int) ratio)
    {
        ammoText.text = $"{ratio.Item1} / {ratio.Item2}";
    }

    public void ContinueToExplore()
    {
        tooltip.SetActive(false);
        loadingPanel.SetActive(false);
        hudPanel.SetActive(true);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        notificationText.gameObject.SetActive(false);
        goalText.gameObject.SetActive(false);

        gameSuspendListener.baseEvent.Raise(false);
    }

    public void MainMenu()
    {
        NetworkManager.instance.MainMenu();
    }
}
