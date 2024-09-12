using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceLevelMenu : MonoBehaviour
{
    public void PlayLevel1()
    {
        SceneManager.LoadScene(2);
    }
    public void PlayLevel2()
    {
        SceneManager.LoadScene(3);
    }
    public void PlayLevel3()
    {
        SceneManager.LoadScene(4);
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
