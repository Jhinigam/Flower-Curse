using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sett : MonoBehaviour
{
    public float ambientIntensity;
    public CharacterController moveinput;

    void Awake()
    { 
        
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        ambientIntensity = RenderSettings.ambientIntensity;
        moveinput = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        

    }
    private void OnEnable()
    {
        Time.timeScale = 1;
        moveinput.enabled = false;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ambientIntensity += Time.deltaTime * 1f; 
        }

            



        if (Input.GetKey(KeyCode.DownArrow))
        {
            ambientIntensity -= Time.deltaTime * 1f;
        }
            
        
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow))
            {
                ambientIntensity = Mathf.Clamp(ambientIntensity, 0f, 4f);
            RenderSettings.ambientIntensity = ambientIntensity; 
            }
        


    }
}
