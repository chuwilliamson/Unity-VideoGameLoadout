using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIUpgradeableMenu : MonoBehaviour
{
    IUpgradeable upgradeable;
    Slider slider;
    TMPro.TextMeshProUGUI uitext;
    public GameObject Upgrade;
    private void Start()
    {
        
        uitext = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        slider = GetComponentInChildren<Slider>();
        
        if(Upgrade == null)
               return;
        uitext.text = Upgrade.name;
        SetUpgrade(Upgrade.GetComponent<IUpgradeable>());
    }
    public void SetUpgrade(IUpgradeable upgrade)
    {
        uitext.text = Upgrade.name;
        upgradeable = upgrade;
    }
    public void Increment()
    {
        SetUpgrade(Upgrade.GetComponent<IUpgradeable>());
        upgradeable.Upgrade();
        slider.value = upgradeable.Level;
    }

    public void Decrement()
    {
        SetUpgrade(Upgrade.GetComponent<IUpgradeable>());
        upgradeable.Downgrade();
        slider.value = upgradeable.Level;
    }
}
