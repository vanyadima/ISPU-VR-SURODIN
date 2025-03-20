using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendPhoto : MonoBehaviour, IInteractable
{
    public Canvas pauseCanvas; // ������, ������� ������ ������� ��� �����
    public AudioSource pauseMusic; // ������, ������� ����� ���������������� ��� �����

    private bool isPaused = false; // ����, ����������� �� ��, �� ����� �� ����
    void Start()
    {
        // ��������, ��� ������ � ������ ��������� ����������
        if(pauseCanvas != null) pauseCanvas.gameObject.SetActive(false);
        if(pauseMusic != null) pauseMusic.Stop();
    }

    void Update()
    {
        //// �������� ������� ����� ������ �����������
        //if (anyButtonAction.stateDown)
        //{
        //    if (isPaused)
        //        ResumeGame();
        //    //else
        //    //    PauseGame();
        //}
    }

    public void PauseGame()
    {
        isPaused = true;
        if (pauseCanvas != null) pauseCanvas.gameObject.SetActive(true);
        if (pauseMusic != null) pauseMusic.Play();
        //Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isPaused = false;
        //Time.timeScale = 1;
        if(pauseCanvas != null) pauseCanvas.gameObject.SetActive(false);
        if(pauseMusic != null) pauseMusic.Stop();
    }

    public void OnInteractKeyDown()
    {
        PauseGame();
    }

    public void OnInteractKeyUp()
    {
        ResumeGame();
    }

    public void OnAddInteractKeyDown()
    {
        
    }

    public void OnAddInteractKeyUp()
    {
        
    }
}
