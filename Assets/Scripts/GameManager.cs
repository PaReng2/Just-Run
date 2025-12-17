using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Data")]
    public bool isHaveKey;

    private void Awake()
    {
        // 싱글톤 설정: 데이터 유지를 위해 파괴되지 않음
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
    }

    // 캐릭터가 죽었을 때 호출하는 함수
    public void OnPlayerDie()
    {
        // 씬에 있는 UIManager를 찾아서 ShowGameOver()를 실행시킵니다.
        UIManager ui = FindFirstObjectByType<UIManager>();
        if (ui != null)
        {
            ui.ShowGameOver();
        }
    }
}