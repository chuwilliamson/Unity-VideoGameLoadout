
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
public class EnvironmentBehaviour : MonoBehaviour, IUpgradeable
{

    public string label01;
    public TMP_Text m_textMeshPro;
    int _counter;
    int _visibleCount;
    
    [SerializeField]
    [Range(0, 3)]
    int _level =0;

    void Awake()
    {
        print("awake on EnvironmentBehaviour");
        m_textMeshPro.text = label01;
        m_textMeshPro.enableWordWrapping = true;
        m_textMeshPro.alignment = TextAlignmentOptions.Bottom;
    }

    void OnEnable()
    {
        _counter = 0;
        _visibleCount = 0;
        label01 = "";
        m_textMeshPro.text = label01;
    }

    public void DoText()
    {
        print("do text");
        StopCoroutine("StartUp");
        label01 += GameStateBehaviour.Instance.playerBehaviour.GetComponent<PlayerBehaviour>().Position.ToString() + "\n";
        m_textMeshPro.text = label01;
        StartCoroutine("StartUp");
    }

    IEnumerator StartUp()
    {
        // Force and update of the mesh to get valid information.
        m_textMeshPro.ForceMeshUpdate();
        var totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
        while(_visibleCount < m_textMeshPro.textInfo.characterCount)
        {
            _visibleCount = _counter % (totalVisibleCharacters + 1);
            m_textMeshPro.maxVisibleCharacters = _visibleCount; // How many characters should TextMeshPro display?

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

    public int Level { get { return _level; } }
}

