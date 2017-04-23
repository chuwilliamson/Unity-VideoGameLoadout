using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class UIProgressionDropDown : MonoBehaviour
{
    // Use this for initialization
    void OnEnable()
    {
        var opts = GetComponent<TMP_Dropdown>().options;
        opts.Clear();
        GameStateBehaviour.Instance.Upgradeables.ForEach(
            u => opts.Add(new TMP_Dropdown.OptionData(u.GetType().ToString())));
    }
}