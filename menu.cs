using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    public GameObject Settingsmenu;
    public GameObject Initmenu;
    public bool paused = false;
    public GameObject Menu;
    public CharacterController moveinput;

    // Start is called before the first frame update
    void Start()
    {
        moveinput = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == false)
            {
                Pause();
            }
            else
            {
                Resume();
            }
          }
        
    }   


    private void Pause()
    {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                paused = true;
                Debug.Log("gioco in pausa");
                Menu.SetActive(true);

    }


    public void Resume()
    {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                paused = false; 
                Menu.SetActive(false);
                Settingsmenu.SetActive(false);
                moveinput.enabled = true;

    }


    public void Quit()
    {
        Application.Quit();
        Debug.Log("applicazione chiusa");

    }


    public void Settings()
    {
        Initmenu.SetActive(false);
        Settingsmenu.SetActive(true);

    }

    
    public void Return()
    {
        Initmenu.SetActive(true);
        Settingsmenu.SetActive(false);
        Time.timeScale = 0;

    }
}
