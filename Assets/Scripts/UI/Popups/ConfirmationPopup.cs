using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPopupDataProvider : PopupDataProvider<ConfirmationPopup>
{
    public class ConfirmationPopupParams : PopupParams
    {
        public string Text;
        public string ButtonText;
        public Action OnConfirm;
    }

    public ConfirmationPopupParams Params => Data as ConfirmationPopupParams;

    public ConfirmationPopupDataProvider(string text, string buttonText, Action onConfirm)
    {
        Data = new ConfirmationPopupParams
        {
            Text = text,
            ButtonText = buttonText,
            OnConfirm = onConfirm
        };
    }

    public ConfirmationPopupDataProvider(PopupParams data) : base(data) { }
}

public class ConfirmationPopup : PopupBase<ConfirmationPopup, ConfirmationPopupDataProvider>
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _confirmText;
    [SerializeField] private Button _confirmButton;

    public override void Initialize(ConfirmationPopupDataProvider provider)
    {
        _text.text = DataProvider.Params.Text;
        _confirmText.text = DataProvider.Params.ButtonText;
        _confirmButton.onClick.AddListener(OnConfirm);
    }

    private void OnConfirm()
    {
        Close();
        DataProvider.Params?.OnConfirm?.Invoke();
    }
}

