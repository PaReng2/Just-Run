using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float jumpInterval = 2.0f; // 점프 간격 (초)
    [SerializeField] private float jumpPower = 5.0f;    // 위로 뛰는 힘
    [SerializeField] private float moveSpeed = 3.0f;    // 앞으로 나가는 힘

    [Header("References")]
    [SerializeField] private Transform targetPlayer;    // 쫓아갈 플레이어

    private Rigidbody rb;
    private float timer;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 플레이어가 할당되지 않았다면 태그로 자동 검색
        if (targetPlayer == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                targetPlayer = playerObj.transform;
        }
    }

    void Update()
    {
        if (targetPlayer == null) return;

        // 1. 몬스터가 플레이어를 바라보게 함 (y축 회전만 적용)
        Vector3 lookPos = targetPlayer.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        // 2. 타이머 계산
        timer += Time.deltaTime;

        // 3. 2초가 지났고, 땅에 있다면 점프 실행
        if (timer >= jumpInterval && isGrounded)
        {
            JumpTowardsPlayer();
            timer = 0f; // 타이머 초기화
        }
    }

    void JumpTowardsPlayer()
    {
        // 플레이어 방향 벡터 계산 (높이 차이는 무시)
        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        direction.y = 0; // 수평 방향만 고려

        // 위쪽 힘(Jump) + 플레이어 방향 힘(Move) 적용
        // ForceMode.Impulse는 순간적인 힘(점프)에 적합합니다.
        Vector3 jumpForce = (Vector3.up * jumpPower) + (direction * moveSpeed);

        rb.AddForce(jumpForce, ForceMode.Impulse);

        isGrounded = false; // 공중 상태로 변경
    }

    // 바닥에 착지했는지 확인
    void OnCollisionEnter(Collision collision)
    {
        // "Ground" 태그가 달린 바닥이나, 단순히 y축 아래쪽 충돌일 경우
        // 여기서는 간단하게 모든 충돌에 대해 착지로 간주합니다.
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}
