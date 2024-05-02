using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeArea : MonoBehaviour
{
    public float damage = 10; // 데미지 값
    public float damageInterval = 0.5f; // 데미지 간격 (초)
    public float duration = 5f; // 용암이 유지되는 시간

    private float elapsedTime = 0f; // 용암 안에 머무른 시간 추적을 위한 변수
    private Coroutine damageCoroutine; // 데미지 코루틴

    private void Start()
    {
        // 용암 유지 시간이 종료되면 용암 오브젝트를 비활성화하기 위한 코루틴 실행
        StartCoroutine(DisableLavaAfterDuration());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // 용암 안에 들어온 적이라면
        {
            

            // 적의 체력을 가져오기
            Enemy enemyScript = other.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                float enemyHealth = enemyScript.health;

                // 데미지 적용
                enemyScript.TakeDamageOverTime(damage, damageInterval);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // 용암에서 나간 적이라면
        {
           
            if (damageCoroutine != null) // 데미지 코루틴이 실행 중이라면
            {
                StopCoroutine(damageCoroutine); // 데미지 적용 코루틴 종료
            }
        }
    }

    private IEnumerator DisableLavaAfterDuration()
    {
        yield return new WaitForSeconds(duration); // 용암 유지 시간만큼 대기

        // 용암 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}
