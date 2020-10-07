using DG.Tweening;
using TMPro;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    [SerializeField] private GameObject hintObject;
    [SerializeField] private string hintText;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private float fadeInOutTime = 0.8f;
    [SerializeField] private bool showOnlyOnce = false;

    private TextMeshProUGUI _hintTmPro;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = hintObject.GetComponent<CanvasGroup>();
        _hintTmPro = hintObject.GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _hintTmPro.text = $"<mark=#00000066>{hintText}<mark>";
        DOVirtual.Float(_canvasGroup.alpha, 1, fadeInOutTime, x => _canvasGroup.alpha = x);
    }

    private void OnTriggerExit(Collider other)
    {
        DOVirtual.Float(_canvasGroup.alpha, 0, fadeInOutTime, x => _canvasGroup.alpha = x);
        if (showOnlyOnce) boxCollider.enabled = false;
    }
}
