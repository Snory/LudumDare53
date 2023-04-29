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
    private float _movementPerSecond;
    [SerializeField]
    private float _movingDistanceTreshold;

    [SerializeField]
    private bool _moving;


    [Header("Events")]
    [SerializeField]
    private GeneralEvent _switchSeatRequest;
    [SerializeField]
    private GeneralEvent _attackSeatRequest;
    [SerializeField]
    private GeneralEvent _defendSeatRequest;
    [SerializeField]
    private GeneralEvent _showNearbySeatRequest;
    [SerializeField]
    private GeneralEvent _unitDied;

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
    private TextMeshProUGUI _healthText;

    [SerializeField]
    private int _health;
    [SerializeField]
    private float _defendTime;
    public UnityEvent Died;

    [Header("Attack")]
    [SerializeField]
    private int _attackPower;
    [SerializeField]
    private int _attackRange;

    private void Awake()
    {
        SetHealth();
    }

    public void SetSeat(Seat s)
    {
        _seat = s;
    }

    public void MoveUnitToSeat()
    {
        StartCoroutine(ChangePositionRoutine());
    }

    private IEnumerator ChangePositionRoutine()
    {
        _moving = true;
        while (this.transform.position != _seat.transform.position)
        {
            transform.position = Vector3.Lerp(transform.position, _seat.transform.position, _movementPerSecond * Time.deltaTime);
            yield return null;
            if(Vector2.Distance(this.transform.position, _seat.transform.position) < _movingDistanceTreshold)
            {
                _moving = false;
            }
        }
        _moving = false; //if it is on same position
    }

    public Seat GetSeat()
    {
        return _seat;
    }

    public void SetHealth()
    {
        _healthText.text = _health.ToString();
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
        _attackSeatRequest.Raise(new AttackSeatRequestEventArgs(this, attackedSeat, _attackRange, _attackPower, RequestAttackCallback));
    }

    public void RaiseDefendSeatRequest()
    {
        _defendSeatRequest.Raise(new DefendSeatRequestEventArgs(this, _seat, _defendTime , RequestDefendCallback));
    }

    public void RaiseMarkNearbySetRequest(bool show)
    {
        _showNearbySeatRequest.Raise(new ShowNearbySeatsEventArgs(_seat, _attackRange, show));
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
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if(_health <= 0)
        {
            Death();
        }

        SetHealth();
    }

    public void Death()
    {
        Died?.Invoke();
        Destroy(this.gameObject,0.01f);
    }

    public void RequestSeatSwitchCallback(bool switched)
    {
        if (switched)
        {
            CoolDown(true);
        }
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
            yield return null;

            _coolDownTime -= Time.deltaTime;
            _coolDownImage.fillAmount = _coolDownTime / _maxCoolDownTime;

            if (_coolDownTime < 0)
            {
                CoolDown(false);
            }
        }
    }





}
