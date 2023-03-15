using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private RoomsCanvases _roomCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }
    public void DisplayPlayOnline()
    {
        _roomCanvases.CreateOrJoinRoom.Show();
        _roomCanvases.MainMenuManager.Hide();
    }
    public void QuitGame()
    {
        Debug.Log("Game left successfully");
        Application.Quit();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void OnClick_PlayArcade()
    {
        SceneManager.LoadScene(2);
    }
}
