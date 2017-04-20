
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
public class EnvironmentBehaviour : MonoBehaviour, IUpgradeable
{
   
    public string label01;
    public TMP_Text m_textMeshPro;
    int counter;
    int visibleCount;

    void Awake()
    {
        print("awake on EnvironmentBehaviour");
        m_textMeshPro.text = label01;
        m_textMeshPro.enableWordWrapping = true;
        m_textMeshPro.alignment = TextAlignmentOptions.Bottom;
    }
    
    void OnEnable()
    {
        counter = 0;
        visibleCount = 0;
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
        int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
        while(visibleCount < m_textMeshPro.textInfo.characterCount)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);
            m_textMeshPro.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            // Once the last character has been revealed, wait 1.0 second and start over.

            counter += 1;

            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Upgrade()
    {
        m_level++;
    }

    public void Downgrade()
    {
        m_level--;
    }

    public int Level { get { return m_level; } }
}

