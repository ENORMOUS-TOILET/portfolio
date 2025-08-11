using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    private string currentSceneName;

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(currentSceneName);

        // 检查是否按下ESC键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 如果是在编辑器中，则停止播放；如果是构建后的游戏，则退出
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 在编辑器中停止播放
#else
            Application.Quit(); // 在构建后的游戏中退出
#endif
        }
    }
}
