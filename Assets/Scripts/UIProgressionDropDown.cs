using TMPro;
using UnityEngine;

public class UIProgressionDropDown : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    // Use this for initialization
    void OnEnable()
    {
        GameStateBehaviour.Instance.onUpgradeChange.AddListener(Populate);
    }

    void OnDisable()
    {

        GameStateBehaviour.Instance.onUpgradeChange.RemoveListener(Populate);
        
    }


    private void Populate()
    {
        dropdown.ClearOptions();
        GameStateBehaviour.Instance.Upgradeables.ForEach(
            u => dropdown.options.Add(new TMP_Dropdown.OptionData(u.GetType().ToString())));
    }
}