using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    bool onClick;
    TextMeshProUGUI targetGraphic;
    [SerializeField] Color normalColor;
    [SerializeField] Color highlightedColor;
    [SerializeField] Color pressedColor;

    void Start()
    {
        targetGraphic = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        ButtonColor("normal");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonColor("pressed");
        onClick = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        ButtonColor("normal");
        onClick = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!onClick)
        {
            ButtonColor("highlighted");
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!onClick)
        {
            ButtonColor("normal");
        }
    }
    void ButtonColor(string state)
    {
        switch (state)
        {
            case "normal":
                targetGraphic.color = normalColor;
                break;
            case "highlighted":
                targetGraphic.color = highlightedColor;
                break;
            case "pressed":
                targetGraphic.color = pressedColor;
                break;
        }
    }
}
