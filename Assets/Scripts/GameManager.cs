using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject GameOverPannel;
    public Button reTryButton;
    public int curScenenum;

    private void Awake()
    {
        Instance = this;

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameOverPannel.SetActive(false);

        reTryButton.onClick.AddListener(ReStart);

    }

    
    
    void ReStart()
    {
        SceneManager.LoadScene(curScenenum);
    }

    public void Die()
    {
        GameOverPannel.SetActive(true);
        Time.timeScale = 0f;
    }
}
