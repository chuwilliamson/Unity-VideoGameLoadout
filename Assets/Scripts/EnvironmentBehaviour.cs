using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironmentBehaviour : MonoBehaviour, IUpgradeable
{
    public string _terminalText;

    public TMP_Text _textMeshPro;

    int _counter;

    int _visibleCount;

    [SerializeField]
    [Range(0, 3)]
    int _level = 0;

    void Awake()
    {
        _textMeshPro.text = _terminalText;
        _textMeshPro.enableWordWrapping = true;
        _textMeshPro.alignment = TextAlignmentOptions.BottomLeft;
    }

    void OnEnable()
    {
        _counter = 0;
        _visibleCount = 0;
        _terminalText = string.Empty;
        _textMeshPro.text = _terminalText;
    }

    public void DoText(string text)
    {
        StopCoroutine("StartUp");
        _terminalText += text;
        _textMeshPro.text = _terminalText;
        StartCoroutine("StartUp");
    }

    IEnumerator StartUp()
    {
        // Force and update of the mesh to get valid information.
        _textMeshPro.ForceMeshUpdate();
        var totalVisibleCharacters = _textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
        while (_visibleCount < _textMeshPro.textInfo.characterCount)
        {
            _visibleCount = _counter % (totalVisibleCharacters + 1);
            _textMeshPro.maxVisibleCharacters = _visibleCount; // How many characters should TextMeshPro display?

            // Once the last character has been revealed, wait 1.0 second and start over.
            _counter += 1;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Upgrade()
    {
        _level = (_level < 3) ? ++_level : 3;
    }

    public void Downgrade()
    {
        _level = (_level > 0) ? --_level : 0;
    }

    public int Level
    {
        get
        {
            return _level;
        }
    }
}