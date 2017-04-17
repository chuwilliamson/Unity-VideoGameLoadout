using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void LoadLevel(int index)
    {
        GameStateBehaviour.Instance.LoadScene(index);
    }
}
