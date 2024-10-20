using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region Singleton
    public static MenuManager instance;
    public static bool isGamePaused = false;
    public Animator transitionFade;
    public Animator transitionItem;
    public Animator transitionPauseMenu;
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion
    // Start is called before the first frame update
    public void PlayGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // StartCoroutine(LoadingLevel(SceneManager.GetActiveScene().buildIndex+1));
        StartCoroutine(LoadingFromHome(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void ContinueGame()
    { Time.timeScale = 1f;
        if (transitionPauseMenu)
            transitionPauseMenu.SetTrigger("Leave");
        Debug.Log("continue");
       
        isGamePaused = false;
    }
    public void PauseGame()
    {
        if (transitionPauseMenu)
            transitionPauseMenu.SetTrigger("Come");
        Debug.Log("pause");
        StartCoroutine(LoadPauseMenu());
       
    }
    public void MainMenu()
    {
        ContinueGame();
        // SceneManager.LoadScene(0);
        StartCoroutine(LoadingHome(0));
    }
    IEnumerator LoadPauseMenu()
    {
        yield return new WaitForSeconds(1f); 
        Time.timeScale = 0f;
        isGamePaused = true;
    }
        IEnumerator LoadingLevel(int levelIndx)
    {
       
        if (transitionItem)
            transitionItem.SetTrigger("Leave");
      //  yield return new WaitForSeconds(1f);
        if(transitionFade)
            transitionFade.SetTrigger("Leave");
       
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(levelIndx);
    }
    IEnumerator LoadingHome(int levelIndx)
    {

        if (transitionItem)
            transitionItem.SetTrigger("Leave");
       
        if (transitionPauseMenu)
            transitionPauseMenu.SetTrigger("Leave");
        if (transitionFade)
            transitionFade.SetTrigger("Leave");
       
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(levelIndx);
    }
    IEnumerator LoadingFromHome(int levelIndx)
    {

        if (transitionItem)
            transitionItem.SetTrigger("Leave");
        //  yield return new WaitForSeconds(1f);
        if (transitionFade)
            transitionFade.SetTrigger("Leave");
       
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(levelIndx);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGamePaused)
            {

                PauseGame();
            }
            else
            {

                ContinueGame();
            }
        }
    }

}
