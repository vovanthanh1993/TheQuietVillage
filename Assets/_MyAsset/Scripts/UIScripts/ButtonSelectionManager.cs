using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonSelectionManager : MonoBehaviour
{
    [SerializeField] private List<Button> menuButtons;
    private Button currentSelectedButton;

    void OnEnable()
    {
        foreach (Button button in menuButtons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
            ApplyNormalState(button);
        }

        if (menuButtons.Count > 0)
        {
            OnButtonClick(menuButtons[0]);
        }
    }

    public void OnButtonClick(Button clickedButton)
    {
        if (currentSelectedButton != null)
        {
            ApplyNormalState(currentSelectedButton);
        }

        currentSelectedButton = clickedButton;
        ApplySelectedState(currentSelectedButton);
    }

    private void ApplyNormalState(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        button.targetGraphic.canvasRenderer.SetColor(button.colors.normalColor);
    }

    private void ApplySelectedState(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        button.Select();
    }
}