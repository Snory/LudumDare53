using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class Unit : MonoBehaviour
{
    private Seat _seat;

    [Header("Movement")]

    [SerializeField]
    private float _firstMovementSpeedPerSecond;

    [SerializeField]
    private float _movementSpeedPerSecond;

    [SerializeField]
    private bool _moving;

    public UnityEvent<bool> Moved;

    [Header("Events")]
    [SerializeField]
    private GeneralEvent _switchSeatRequest;
    [SerializeField]
    private GeneralEvent _attackSeatRequest;
    [SerializeField]
    private GeneralEvent _defendSeatRequest;
    [SerializeField]
    private GeneralEvent _showNearbySeatRequest;

    [Header("Cooldown")]
    [SerializeField]
    private Image _coolDownImage;
    [SerializeField]
    private float _maxCoolDownTime;

    [SerializeField]
    private bool _coolingDown;
    private float _coolDownTime;

    [Header("Health")]

    [SerializeField]
    private Image _healthImage;

    [SerializeField]
    private int _maxHealth;

    [SerializeField]
    private int _health;
    [SerializeField]
    private float _defendTime;


    [Header("Attack")]

    [SerializeField]
    private GameObject _spellPrefab;

    [SerializeField]
    private Transform _spellPositionTransform;

    [SerializeField]
    private SpellData _basicSpell;
    private Spell _currentSpell;

    private Seat _attackedSeat;

    private bool _firstMove;

    [SerializeField]
    private int _score;

    [SerializeField]
    private GeneralEvent _unitDied;

    public void Stop()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
    public int GetScore()
    {
        return _score;
    }

    public void AddScore(int score)
    {
        _score += score;
    }

    private void Awake()
    {
        SetHealth(_maxHealth);
        SetSpell(_basicSpell);
        _firstMove = true;
    }

    public void SetSpell(SpellData data)
    {
        GameObject weaponGO = Instantiate(_spellPrefab, _spellPositionTransform.position, Quaternion.identity, this.transform);
        if(_currentSpell != null)
        {
            Destroy(_currentSpell.gameObject);
        }
        _currentSpell = weaponGO.GetComponent<Spell>();
        _currentSpell.Initialize(data);
        _currentSpell.Used.AddListener(OnSpellUsed);
    }

    public void SetSeat(Seat s)
    {
        _seat = s;
        MoveUnitToSeat();
    }

    public void MoveUnitToSeat()
    {
        StartCoroutine(ChangePositionRoutine());
    }

    private IEnumerator ChangePositionRoutine()
    {
        Moving(true);
        while (this.transform.position != _seat.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _seat.transform.position, _movementSpeedPerSecond * Time.deltaTime);
    
            yield return new WaitForSeconds(0);
        }
        Moving(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Break")
        {
            if(collision.GetComponentInParent<Seat>() == _seat)
            {
                Moving(false);
            }
        }
    }

    private void Moving(bool moving)
    {
        if(_firstMove == true && moving == false)
        {
            _firstMove = false;
        }

        _moving = moving;
        Moved?.Invoke(_moving);
    }

    public int GetAttackRange()
    {
        return _currentSpell.GetSpellRange();
    }


    public Seat GetSeat()
    {
        return _seat;
    }

    public void SetHealth(int health)
    {
        _health = health;
        _healthImage.fillAmount = _health / (float)_maxHealth;
    }

    public void AddHealth(int health)
    {
        int healthAfterHeal = _health + health;

        if(healthAfterHeal > 10)
        {
            healthAfterHeal = 10;
        }

        SetHealth(healthAfterHeal);
    }

    public int GetHealth()
    {
        return _health;
    }

    public bool CanPerformAction()
    {
        return !_coolingDown && !_moving;
    }

    public void RaiseSwitchSeatRequest(Seat requestedSeat)
    {
        _switchSeatRequest.Raise(new SwitchSeatRequestEventArgs(this, requestedSeat, RequestSeatSwitchCallback));
    }

    public void RaiseAttackSeatRequest(Seat attackedSeat)
    {
        _attackedSeat = attackedSeat;
        _attackSeatRequest.Raise(new AttackSeatRequestEventArgs(this, attackedSeat, GetAttackRange(), RequestAttackCallback));
    }

    public void RaiseDefendSeatRequest()
    {
        _defendSeatRequest.Raise(new DefendSeatRequestEventArgs(this, _seat, _defendTime, RequestDefendCallback));
    }

    public void RaiseMarkNearbySetRequest(bool show)
    {
        _showNearbySeatRequest.Raise(new ShowNearbySeatsEventArgs(_seat, GetAttackRange(), show));
    }

    public void RequestDefendCallback(bool defended)
    {
        if (defended)
        {
            CoolDown(true);
        }
    }

    public void RequestAttackCallback(bool attacked)
    {
        if (attacked)
        {
            CoolDown(true);
            _currentSpell.UseSpell(_attackedSeat, AddScore);
        }
        
        _attackedSeat = null;
    }

    private void OnSpellUsed()
    {
        SetSpell(_basicSpell);
    }

    public void RequestSeatSwitchCallback(bool switched)
    {
        if (switched)
        {
            CoolDown(true);
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Death();
        }

        SetHealth(_health);
    }

    public void Death()
    {
        StopAllCoroutines();
        _unitDied.Raise(new UnitEventArgs(this));
    }


    public void CoolDown(bool start)
    {
        _coolingDown = start;
        _coolDownTime = _maxCoolDownTime;

        if (start)
        {
            StartCoroutine(CoolingDownRoutine());
        }
        else
        {
            _coolDownImage.fillAmount = 1;
        }

    }

    public IEnumerator CoolingDownRoutine()
    {
        while (_coolingDown)
        {
            yield return new WaitForSeconds(0); 

            _coolDownTime -= Time.deltaTime;
            _coolDownImage.fillAmount = _coolDownTime / _maxCoolDownTime;

            if (_coolDownTime < 0)
            {
                CoolDown(false);
            }
        }
    }

    internal void ResetCoolDown()
    {
        CoolDown(false);
    }
}
