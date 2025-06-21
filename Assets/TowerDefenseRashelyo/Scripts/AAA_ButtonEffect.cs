using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class AAAUIButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private Outline outline;
    private Vector2 originalPosition;
    private Tween hoverTween, scaleTween, clickTween, outlineTween;

    [Header("Hover Animation")]
    public float hoverSlideDistance = 8f;
    public float hoverScale = 1.08f;
    public float hoverDuration = 0.2f;

    [Header("Click Animation")]
    public float clickScale = 0.92f;
    public float clickDuration = 0.1f;

    [Header("Outline")]
    public Color hoverOutlineColor = Color.white;
    public float outlineFadeDuration = 0.15f;

    [Header("Audio")]
    public string hoverSound = "hoverDefender";
    public string clickSound = "klikDefenderWolrd";

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.effectColor = new Color(hoverOutlineColor.r, hoverOutlineColor.g, hoverOutlineColor.b, 0f);
            outline.enabled = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverTween?.Kill();
        scaleTween?.Kill();
        outlineTween?.Kill();

        // Slide Up
        hoverTween = rectTransform.DOAnchorPos(originalPosition + Vector2.up * hoverSlideDistance, hoverDuration)
            .SetEase(Ease.OutCubic);

        // Scale Up
        scaleTween = rectTransform.DOScale(hoverScale, hoverDuration)
            .SetEase(Ease.OutBack);

        // Outline Fade In
        if (outline != null)
        {
            outlineTween = DOTween.ToAlpha(
                () => outline.effectColor,
                x => outline.effectColor = x,
                1f,
                outlineFadeDuration
            );
        }

        // Play hover sound
        AudioEventSystem.PlayAudio(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverTween?.Kill();
        scaleTween?.Kill();
        outlineTween?.Kill();

        // Slide Down
        hoverTween = rectTransform.DOAnchorPos(originalPosition, hoverDuration)
            .SetEase(Ease.InCubic);

        // Scale Normal
        scaleTween = rectTransform.DOScale(1f, hoverDuration)
            .SetEase(Ease.InOutSine);

        // Outline Fade Out
        if (outline != null)
        {
            outlineTween = DOTween.ToAlpha(
                () => outline.effectColor,
                x => outline.effectColor = x,
                0f,
                outlineFadeDuration
            );
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickTween?.Kill();

        // Scale down quickly then bounce back
        clickTween = rectTransform.DOScale(clickScale, clickDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                rectTransform.DOScale(hoverScale, clickDuration)
                    .SetEase(Ease.OutBack);
            });

        // Play click sound
        AudioEventSystem.PlayAudio(clickSound);
    }

    void OnEnable()
    {
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.localScale = Vector3.one;
        if (outline != null)
        {
            var c = outline.effectColor;
            outline.effectColor = new Color(c.r, c.g, c.b, 0f);
        }
    }
}
