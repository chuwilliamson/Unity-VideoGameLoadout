using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterToGameState : MonoBehaviour {

	// Use this for initialization
	void OnEnable ()
	{
	    if (GetComponent<IUpgradeable>() == null)
	    {
	        print("no iupgradeable");
            return;
	    }


        GameStateBehaviour.Instance.RegisterUpgrade(GetComponent<IUpgradeable>());
	}

    void OnDisable()
    {
        if(GetComponent<IUpgradeable>() == null)
        {
            print("no iupgradeable");
            return;
        }
        GameStateBehaviour.Instance.UnregisterUpgrade(GetComponent<IUpgradeable>());
    }
}
