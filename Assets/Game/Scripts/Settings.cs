using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    public GameObject settingsMenu;
    public Slider sensitivityBar;
    public TextMeshProUGUI sensitivityValue;

    Movement movement;

    public float Sensitivity { get; protected set; }
    public bool MenuActive { get; protected set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        movement = GameObject.Find("Player").GetComponent<Movement>();
        Sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        movement.sensitivityX = Sensitivity;
        movement.sensitivityY = Sensitivity;

        sensitivityValue.text = Sensitivity.ToString("f2");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            MenuActive = !MenuActive;

            if(MenuActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                settingsMenu.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                settingsMenu.SetActive(false);
            }
        }
    }

    public void OnValueChange()
    {
        Sensitivity = sensitivityBar.value;
        movement.sensitivityX = Sensitivity;
        movement.sensitivityY = Sensitivity;

        sensitivityValue.text = Sensitivity.ToString("f2");

        PlayerPrefs.SetFloat("Sensitivity", Sensitivity);
    }
}
