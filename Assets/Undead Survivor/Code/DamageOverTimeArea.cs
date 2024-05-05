// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DamageOverTimeArea : MonoBehaviour
// {
//     public float damage = 0.1f; // ������ ��
//     public float damageInterval = 0.5f; // ������ ���� (��)
//     public float duration = 5f; // ����� �����Ǵ� �ð�

//     private Coroutine damageCoroutine; // ������ �ڷ�ƾ

//     private void Start()
//     {

//     }

//     private void OnTriggerStay2D(Collider2D other)
//     {
//         if (other.CompareTag("Enemy")) // ��� �ȿ� ���� ���̶��
//         {
            

//             // ���� ü���� ��������
//             Enemy enemyScript = other.GetComponent<Enemy>();
//             if (enemyScript != null)
//             {
//                 float enemyHealth = enemyScript.health;

//                 // ������ ����
//                 enemyScript.TakeDamageOverTime(damage, damageInterval);
//             }
//         }
//     }


//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Enemy")) // ��Ͽ��� ���� ���̶��
//         {
           
//             if (damageCoroutine != null) // ������ �ڷ�ƾ�� ���� ���̶��
//             {
//                 StopCoroutine(damageCoroutine); // ������ ���� �ڷ�ƾ ����
//             }
//         }
//     }
// }
