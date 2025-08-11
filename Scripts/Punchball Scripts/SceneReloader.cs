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

        // ����Ƿ���ESC��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ������ڱ༭���У���ֹͣ���ţ�����ǹ��������Ϸ�����˳�
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // �ڱ༭����ֹͣ����
#else
            Application.Quit(); // �ڹ��������Ϸ���˳�
#endif
        }
    }
}
