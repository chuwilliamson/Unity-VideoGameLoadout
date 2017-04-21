using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputInstructionText : MonoBehaviour, IUpgradeable
{
    int _level;

    //String values for instructions
    string L1 =
            "Level 1\n\nInstructions: Type the letter that corresponds with the direction that you want to move into the text box, and then and hit enter to confirm your move\n\nControls: \nW - Walk North\nS - Walk South\nA - Walk West\nD - Walk East"
        ;

    string L2 =
            "Level 2\n\nInstructions: Hit the letter that corresponds with the direction that you want to move in\n\nControls: \nW - Walk North\nS - Walk South\nA - Walk West\nD - Walk East"
        ;

    string L3 =
            "Level 3\n\nInstructions: Press or hold the letter that corresponds with the direction you want to move\n\nControls: \nW - Walk North\nS - Walk South\nA - Walk West\nD - Walk East"
        ;

    public int Level
    {
        get { return _level; }
    }

    public void Upgrade()
    {
        _level++;
        if (_level >= 2)
        {
            _level = 2;
        }
    }

    public void Downgrade()
    {
        _level--;
        if (_level <= 0)
        {
            _level = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Changes the instruction text based on Player Level
        switch (_level)
        {
            default:
                gameObject.GetComponent<Text>().text = L1;
                break;
            case 1:
                gameObject.GetComponent<Text>().text = L2;
                break;
            case 2:
                gameObject.GetComponent<Text>().text = L3;
                break;
        }
    }
}