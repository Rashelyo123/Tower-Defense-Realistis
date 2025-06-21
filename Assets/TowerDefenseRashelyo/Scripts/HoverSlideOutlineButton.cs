using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class HoverSlideOutlineButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Tween slideTween;
    private Tween clickTween;
    private Outline outline;

    [Header("Slide Settings")]
    public float slideDistance = 10f;
    public float slideDuration = 0.2f;

    [Header("Outline Settings")]
    public Color outlineHoverColor = Color.white;
    public float outlineFadeDuration = 0.15f;

    [Header("Click Feedback")]
    public float clickScale = 0.9f;
    public float clickDuration = 0.1f;
    public float OriginalScale = 1f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        // Cari komponen Outline
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.effectColor = new Color(outlineHoverColor.r, outlineHoverColor.g, outlineHoverColor.b, 0f);
            outline.enabled = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slideTween?.Kill();
        slideTween = rectTransform.DOAnchorPos(originalPosition + new Vector2(0f, slideDistance), slideDuration)
            .SetEase(Ease.OutCubic);
        AudioEventSystem.PlayAudio("hoverDefender");

        if (outline != null)
        {
            outline.DOKill();
            outline.DOFade(1f, outlineFadeDuration); // Fade in
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slideTween?.Kill();
        slideTween = rectTransform.DOAnchorPos(originalPosition, slideDuration)
            .SetEase(Ease.InCubic);

        if (outline != null)
        {
            outline.DOKill();
            outline.DOFade(0f, outlineFadeDuration); // Fade out
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickTween?.Kill();
        rectTransform.DOKill();
        AudioEventSystem.PlayAudio("klikDefenderWolrd");

        // Feedback klik → scale kecil lalu balik
        clickTween = rectTransform
            .DOScale(clickScale, clickDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                rectTransform.DOScale(OriginalScale, clickDuration).SetEase(Ease.OutBack);
            });
    }
}
