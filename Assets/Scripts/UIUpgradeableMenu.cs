using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIUpgradeableMenu : MonoBehaviour
{
    IUpgradeable upgradeable;
    Slider slider;
    TMPro.TextMeshProUGUI uitext;

    public GameObject UpgradeableReferenceGameObject;

    private void Awake()
    {
        uitext = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        uitext.SetText("");
    }
    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        
        if(UpgradeableReferenceGameObject == null)
               return;
        SetUpgrade(UpgradeableReferenceGameObject.GetComponent<IUpgradeable>());
    }

    public void UISetUpgrade(TMP_Dropdown tmp)
    {
        upgradeable = GameStateBehaviour.Instance.upgradeDict[tmp.itemText.text];
        uitext.SetText(string.Format(" <#ffa000>{0}</color> <sprite={1}>", UpgradeableReferenceGameObject.name, upgradeable.Level));
    }
    public void SetUpgrade(IUpgradeable upgrade)
    {
        upgradeable = upgrade;
        uitext.SetText(string.Format(" <#ffa000>{0}</color> <sprite={1}>", UpgradeableReferenceGameObject.name, upgradeable.Level));
        
    } 
    public void Increment()
    {
        SetUpgrade(UpgradeableReferenceGameObject.GetComponent<IUpgradeable>());
        upgradeable.Upgrade();
        slider.value = upgradeable.Level;
    }

    public void Decrement()
    {
        SetUpgrade(UpgradeableReferenceGameObject.GetComponent<IUpgradeable>());
        upgradeable.Downgrade();
        slider.value = upgradeable.Level;
    }

}
