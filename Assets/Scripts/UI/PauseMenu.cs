using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private MenuManager menuManager;
    [SerializeField]private GameObject pauseMenuUI;
    private void Start()
    {
        
        menuManager = MenuManager.instance;
    }
  /*  void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!MenuManager.isGamePaused)
            {
               
               PauseGame();
            }
            else
            {
               
               ContinueGame();
            }
        }
    }*/
    public void PauseGame() { 
        //pauseMenuUI.SetActive(true);
        menuManager.PauseGame(); 
    }
    public void ContinueGame()
    {
      //  pauseMenuUI.SetActive(false);
        menuManager.ContinueGame();
    }
}
