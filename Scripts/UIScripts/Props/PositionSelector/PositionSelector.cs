﻿using UI.Widget;
using UnityEngine;

public class PositionSelector : MonoBehaviour
{
    public GameObject Panel;
    public CustomInputField InputK;
    public CustomInputField InputX;
    public CustomInputField InputY;
    public GUIInteractableIcon AcceptButton;
    public GUIInteractableIcon CloseButton;
    public CameraController CameraCtr;

    private Vector3Int Position;
    private CustomInputField focusInputField;

    private void Awake()
    {
        InputK.OnSelected += delegate { SetFocusInputField(InputK); };
        InputX.OnSelected += delegate { SetFocusInputField(InputX); };
        InputY.OnSelected += delegate { SetFocusInputField(InputY); };

        InputX.OnValueChanged += OnPositionInputChanged;
        InputY.OnValueChanged += OnPositionInputChanged;

        AcceptButton.OnClickEvents += OnAcceptButton;
        CloseButton.OnClickEvents += Close;
    }

    private void OnAcceptButton()
    {
        int.TryParse(InputX.Text, out int x);
        int.TryParse(InputY.Text, out int y);

        Position.x = Mathf.Clamp(x, 0, 512);
        Position.y = Mathf.Clamp(y, 0, 512);

        CameraCtr.Set(Position.ToClientPosition());
        Close();
    }

    public void Close()
    {
        Panel.SetActive(false);
        focusInputField?.Keyboard?.Close();
    }

    public void Open()
    {
        if (Panel.activeInHierarchy)
        {
            Panel.SetActive(false);
        }
        else
            Panel.SetActive(true);
    }

    private void SetFocusInputField(CustomInputField inputField)
    {
        focusInputField = inputField;
    }

    private void OnPositionInputChanged(string value)
    {
        int.TryParse(value, out int v);
        v = Mathf.Clamp(v, 0, 512);

        focusInputField.SetContent(v.ToString());
    }
}
