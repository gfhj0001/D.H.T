
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    public string sceneName; // 전환할 씬의 이름

    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        GetComponent<Button>().onClick.AddListener(SwitchScene);
    }

    // 버튼 클릭 시 호출되는 메서드
    void SwitchScene()
    {
        // sceneName에 지정된 씬으로 전환
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
