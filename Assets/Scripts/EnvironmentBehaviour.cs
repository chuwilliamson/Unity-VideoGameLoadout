
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
public class EnvironmentBehaviour : MonoBehaviour
{
    public string label01;
    public TMP_Text m_textMeshPro;
     
    void Awake()
    {
        m_textMeshPro.text = label01;
        m_textMeshPro.enableWordWrapping = true;
        m_textMeshPro.alignment = TextAlignmentOptions.Bottom;
    }
    
    public void DoText()
    {
        StopCoroutine("StartUp");        
        label01 += GameStateBehaviour.Instance.playerBehaviour.GetComponent<PlayerBehaviour>().Position.ToString() + "\n";
        m_textMeshPro.text = label01;
        StartCoroutine("StartUp");
    }
    int counter = 0;
    int visibleCount = 0;
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

}

