using TMPro;

using UnityEngine;

public class UIProgressionDropDown : MonoBehaviour
{
    // Use this for initialization
    void OnEnable()
    {
        var dropdown = GetComponent<TMP_Dropdown>();
        
        GameStateBehaviour.Instance.Upgradeables.ForEach(
            u => dropdown.options.Add(new TMP_Dropdown.OptionData(u.GetType().ToString())));
    }

    private void OnDisable()
    {
        var dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
    }

}