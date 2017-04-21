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
        uitext.text = Upgrade.name;
        if(Upgrade == null)
            return;
        SetUpgrade(Upgrade.GetComponent<IUpgradeable>());
    }
    public void SetUpgrade(IUpgradeable upgrade)
    {
        upgradeable = upgrade;
    }
    public void Increment()
    {
        upgradeable.Upgrade();
        slider.value = upgradeable.Level;
    }

    public void Decrement()
    {
        upgradeable.Downgrade();
        slider.value = upgradeable.Level;
    }
}
