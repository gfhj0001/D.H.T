
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    public string sceneName; // ��ȯ�� ���� �̸�

    void Start()
    {
        // ��ư�� Ŭ�� �̺�Ʈ �߰�
        GetComponent<Button>().onClick.AddListener(SwitchScene);
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    void SwitchScene()
    {
        // sceneName�� ������ ������ ��ȯ
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
