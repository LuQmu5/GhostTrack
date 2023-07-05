﻿using UnityEngine;
using UnityEngine.Events;

public class EnemyDetectionSystem : MonoBehaviour
{
    private LayerMask _playerMask;
    private LayerMask _obstacleMask;
    private float _viewAngle;
    private float _viewRange;

    private bool _isPlayerDetected;
    private CircleCollider2D _hearingArea;

    public event UnityAction PlayerDetected;

    public void Init(LayerMask playerMask, LayerMask obstacleMask, float viewAngle, float viewRange, float hearingRange)
    {
        _playerMask = playerMask;
        _obstacleMask = obstacleMask;
        _viewAngle = viewAngle;
        _viewRange = viewRange;

        _hearingArea = gameObject.AddComponent<CircleCollider2D>();
        _hearingArea.isTrigger = true;
        _hearingArea.radius = hearingRange;
    }

    private void OnBecameVisible()
    {
        if (_isPlayerDetected)
            return;

        enabled = true;
        PlayerCombat.Shooted += OnPlayerShooted;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
        PlayerCombat.Shooted -= OnPlayerShooted;
    }

    private void OnDisable()
    {
        PlayerCombat.Shooted -= OnPlayerShooted;
    }

    private void Update()
    {
        DetectPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            enabled = false;
            Destroy(_hearingArea);
            PlayerDetected?.Invoke();
        }
    }

    private void DetectPlayer()
    {
        var targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, _viewRange, _playerMask);

        foreach (var target in targetsInViewRadius)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.right, directionToTarget) < _viewAngle / 2)
            {
                float distantionToTarget = Vector2.Distance(transform.position, target.transform.position);

                if (Physics2D.Raycast(transform.position, directionToTarget, distantionToTarget, _obstacleMask) == false)
                {
                    PlayerDetected?.Invoke();
                    _isPlayerDetected = true;
                    enabled = false;
                }
            }
        }
    }

    private void OnPlayerShooted()
    {
        PlayerDetected?.Invoke();
    }
}