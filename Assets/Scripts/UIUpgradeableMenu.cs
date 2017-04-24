using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIUpgradeableMenu : MonoBehaviour
{
    IUpgradeable _upgradeable;

    Slider _slider;

    TextMeshProUGUI _headerTextMeshProUgui;

    public GameObject UpgradeableReferenceGameObject;

    void Awake()
    {
        _headerTextMeshProUgui = GetComponentInChildren<TextMeshProUGUI>();
        _headerTextMeshProUgui.SetText(string.Empty);
    }

    private void OnEnable()
    {
        var canvas = transform.GetComponentInParent<Canvas>();
        if (canvas == null) transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        _headerTextMeshProUgui = GetComponentInChildren<TextMeshProUGUI>();
        _headerTextMeshProUgui.SetText(string.Empty);
    }

    void Start()
    {
        _slider = GetComponentInChildren<Slider>();

        if (UpgradeableReferenceGameObject == null) return;
        SetUpgrade(UpgradeableReferenceGameObject.GetComponent<IUpgradeable>());
    }

    public void UISetUpgrade(TMP_Dropdown tmp)
    {
        int index = tmp.value;
        string val = tmp.options[index].text;
        _upgradeable = GameStateBehaviour.Instance.UpgradeDict[val];
        RefreshShownValues();
    }

    public void SetUpgrade(IUpgradeable upgrade)
    {
        _upgradeable = upgrade;
        RefreshShownValues();
    }

    private void RefreshShownValues()
    {
        _headerTextMeshProUgui.SetText(
            string.Format(
                " <#ffa000><size=125%>{0}</size></color> <sprite={1}>",
                _upgradeable.GetType().ToString(),
                _upgradeable.Level));
        _slider.value = _upgradeable.Level;
    }

    public void Increment()
    {
        if (_upgradeable == null) return;
        _upgradeable.Upgrade();
        RefreshShownValues();
    }

    public void Decrement()
    {
        if (_upgradeable == null) return;
        _upgradeable.Downgrade();
        RefreshShownValues();
    }
}