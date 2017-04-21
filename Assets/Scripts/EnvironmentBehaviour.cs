using UnityEngine;
using System.Collections;
using TMPro;

public class EnvironmentBehaviour : MonoBehaviour, IUpgradeable
{
    public string label01 = "Hello world";
    public TMP_Text terminalText;
    public TMP_InputField inputField;
    int _counter;
    int _visibleCount;
    public string inputText;
    

    [SerializeField] [Range(0, 3)] int _level = 0;

    void Awake()
    {
        terminalText.SetText(label01);
        terminalText.enableWordWrapping = true;
        terminalText.alignment = TextAlignmentOptions.Bottom;
    }

    void UpdateText(string val)
    {
        inputText = val;
        GameStateBehaviour.Instance.playerBehaviour.MovePlayer(inputText);
    }
    private void Start()
    {
        inputText = inputField.text;
        inputField.onSubmit.AddListener(UpdateText);
    }
    void OnEnable()
    {
        _counter = 0;
        _visibleCount = 0;
        label01 = "";
        terminalText.SetText(label01);
    }

    public void DoText()
    {
        StopCoroutine("StartUp");
        label01 += GameStateBehaviour.Instance.playerBehaviour.Position + "\n";
        terminalText.SetText(label01);
        StartCoroutine("StartUp");
    }

    IEnumerator StartUp()
    {
        // Force and update of the mesh to get valid information.
        terminalText.ForceMeshUpdate();
        var totalVisibleCharacters = terminalText.textInfo.characterCount; // Get # of Visible Character in text object
        while (_visibleCount < terminalText.textInfo.characterCount)
        {
            _visibleCount = _counter % (totalVisibleCharacters + 1);
            terminalText.maxVisibleCharacters = _visibleCount; // How many characters should TextMeshPro display?

            // Once the last character has been revealed, wait 1.0 second and start over.

            _counter += 1;

            yield return new WaitForSeconds(0.05f);
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
        get { return _level; }
    }
}