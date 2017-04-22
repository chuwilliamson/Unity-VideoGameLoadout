using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIProgressionDropDown : MonoBehaviour
{

    // Use this for initialization
    private void Start()
    {
        Set();
    }

    [ContextMenu("set")]
    void Set()
    {
        var opts = GetComponent<TMP_Dropdown>().options;
        opts.Clear();
        GameStateBehaviour.Instance.Upgradeables.ForEach(u =>
            opts.Add(new TMP_Dropdown.OptionData(u.GetType().ToString())));
        GetComponent<TMP_Dropdown>().RefreshShownValue();
    }
}
