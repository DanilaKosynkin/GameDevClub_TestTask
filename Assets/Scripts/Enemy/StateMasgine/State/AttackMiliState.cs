﻿using System.Threading.Tasks;
using UnityEngine;

public class AttackMiliState : EnemyState
{
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _rangeAttack = 1f;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _attackRate = 0.5f;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _movement;
    private bool _isReadiAttack = true;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _player.OnPlayerDead += Exit;
    }

    private void OnDisable()
    {
        _player.OnPlayerDead -= Exit;
    }

    private void LateUpdate()
    {
        if(Vector3.Distance(_player.transform.position, transform.position) >= _rangeAttack)
        {
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            _movement = new Vector2(direction.x, direction.y) * _moveSpeed;
        }
        else _movement = Vector2.zero;



        if (Vector3.Distance(_player.transform.position, transform.position) <= _rangeAttack && _isReadiAttack)
        {
            _player.TakeDamage(_damage);
            AttackTimer();
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _movement;
    }

    public override void Exit()
    {
        _stateMashine.TransitToPrevious();
    }

    private async void AttackTimer()
    {
        _isReadiAttack = false;
        await Task.Delay((int)(_attackRate * 1000));
        _isReadiAttack = true;
    }
}