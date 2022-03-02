using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public CanvasRenderer startButtonRenderer;

    public void OnStartButton_Pressed()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OnStartButton_Over()
    {
        startButtonRenderer.SetAlpha(0.5f);
    }

    public void OnStartButton_Out()
    {
        startButtonRenderer.SetAlpha(1.0f);
    }
}
