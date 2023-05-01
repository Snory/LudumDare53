using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { TEST }

    private EnemyState _enemyState;

    private Unit _enemyUnit;
    private Table _table;

    [SerializeField]
    private float _decisionCoolDownMin, _decisionCoolDownMax, _attackCoolDownTime, _defendCoolDownTime, _switchCoolDownTime;

    [SerializeField]
    private float _targetLockTimeMax, _targetLockTimeCurrent;

    private Seat _seatReadyToAttack, _nearbySafeSeatToAttackedSeat;

    private bool _switchCooldown, _attackCooldown, _defendCooldown;

    public void Inicialize(Table table, Unit unit)
    {
        _enemyUnit = unit;
        _table = table;
        _enemyState = EnemyState.TEST;
        CoolDownAll();
    }

    public void CoolDownAll()
    {
        AttackCooldown();
        Defendooldown();
        SwitchCooldown();
    }


    private void Update()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.GameState == GameState.PAUSED)
            {
                return;
            }
        }

        TestState();
    }

    public void OnUnitDied(EventArgs args)
    {
        UnitEventArgs unitEventArgs = args as UnitEventArgs;

        if (unitEventArgs.Unit == _enemyUnit)
        {
            Destroy(this.gameObject);
        }
    }

    public void TestState()
    {
        if (_enemyState != EnemyState.TEST)
        {
            return;
        }

        if (_enemyUnit.CanPerformAction())
        {
            List<Seat> seats = _table.GetSeats();

            if (seats.Count == 1)
            {
                return; //just in case
            }

            LockTarget(seats);

            Behaviour2(seats);
        }
    }

    private void Behaviour2(List<Seat> seats)
    {

        bool mySeatUnderAttack = _table.GetSeatsUnderAttack().Contains(_enemyUnit.GetSeat());

        if (mySeatUnderAttack)
        {
            if (_seatReadyToAttack != null)
            {
                if (!_seatReadyToAttack.IsSeatDefended() && !_switchCooldown) //switchnuse s ohroženým místem
                {
                    _enemyUnit.RaiseSwitchSeatRequest(_seatReadyToAttack);
                    SwitchCooldown();
                    _seatReadyToAttack = null; // asi jsem daleko, tak si radši najdu novej cíl

                }
                else if (_enemyUnit.GetSeat() != _nearbySafeSeatToAttackedSeat && !_switchCooldown && _nearbySafeSeatToAttackedSeat != null) //jdu blíž k cíli, stejnak jsem tam chtìl
                {
                    _enemyUnit.RaiseSwitchSeatRequest(_nearbySafeSeatToAttackedSeat);
                    SwitchCooldown();

                }
                else if (!_defendCooldown)
                {
                    _enemyUnit.RaiseDefendSeatRequest();
                    Defendooldown();
                }
            }
        }
        else if (_nearbySafeSeatToAttackedSeat != null) //jsem v bezpeèí nebo jsem stejnak v prdeli, ale mám na koho útoèit, tak zkusim zaútoèit
        {
            if (_nearbySafeSeatToAttackedSeat == _enemyUnit.GetSeat())
            {
                if (_seatReadyToAttack != null && !_attackCooldown && _seatReadyToAttack != null)
                {
                    _enemyUnit.RaiseAttackSeatRequest(_seatReadyToAttack);
                    AttackCooldown();
                }
            }
            else if (!_switchCooldown)
            {
                _enemyUnit.RaiseSwitchSeatRequest(_nearbySafeSeatToAttackedSeat);
                SwitchCooldown();
            }
        }
    }


    public void OnSeatRemoved(EventArgs args)
    {
        SeatEventArgs seatEventArgs = args as SeatEventArgs;

        if (_seatReadyToAttack != null)
        {
            if (seatEventArgs.Seat == _seatReadyToAttack)
            {
                _seatReadyToAttack = null;
                _targetLockTimeCurrent = _targetLockTimeMax;
            }
        }
    }

    private void LockTarget(List<Seat> seats)
    {

        if (_seatReadyToAttack != null)
        {
            if (Time.timeSinceLevelLoad - _targetLockTimeCurrent > _targetLockTimeMax)
            {
                _targetLockTimeCurrent = Time.timeSinceLevelLoad;
                _seatReadyToAttack = null;
                _nearbySafeSeatToAttackedSeat = null;
            }
        }

        List<Seat> listToAttack = null;
        float randomValue = Random.Range(0, 10);

        if (_seatReadyToAttack != null)
        {
            listToAttack = new List<Seat> { _seatReadyToAttack };
        }
        else if (randomValue > 8)
        {
            listToAttack = _table.GetSeatsWithHighestHealth().ToList();
        }
        else if (randomValue <= 8 && randomValue > 2)
        {
            listToAttack = _table.GetSeatsWithLowestHealth().ToList();
        }
        else
        {
            listToAttack = _table.GetSeatsWithHighestScore().ToList();
        }

        Seat seatToAttack = null;
        Seat nearbySafeSeatToAttackedSeat = null;

        if (listToAttack.Count > 0)
        {
            seatToAttack = listToAttack[0];
            nearbySafeSeatToAttackedSeat =
                    _table.GetNearbySeats(seatToAttack, _enemyUnit.GetAttackRange()).Where(s => !s.IsSeatMovedInto() && !s.IsSeatUnderAttack() && !s.IsSeatDefended()).ToList()
                          .OrderByDescending(s => _table.GetDistanceBetweenSeats(s, seatToAttack)).FirstOrDefault();

            if (_seatReadyToAttack == null)
            {
                //get throught the rest if there is any
                for (int i = 1; i < listToAttack.Count; i++)
                {
                    Seat tempLowest = listToAttack[i];
                    Seat tempNearby = null;

                    //get lists of seats which are not under attack and can be moved into
                    //if enemy is in the Seat and is under attack, it is removed, therefore enemy will not attack if he is under attack
                    List<Seat> nearbySeatsToLowestList = _table.GetNearbySeats(tempLowest, _enemyUnit.GetAttackRange()).Where(s => !s.IsSeatMovedInto() && !s.IsSeatUnderAttack() && !s.IsSeatDefended()).ToList();

                    //Find nearby Seat which is most far away from the seatToAttack 
                    if (nearbySeatsToLowestList.Count > 0)
                    {
                        tempNearby = nearbySeatsToLowestList.OrderByDescending(s => _table.GetDistanceBetweenSeats(s, tempLowest)).FirstOrDefault();
                    }

                    if (_table.GetDistanceBetweenSeats(tempNearby, _enemyUnit.GetSeat()) < _table.GetDistanceBetweenSeats(nearbySafeSeatToAttackedSeat, _enemyUnit.GetSeat()))
                    {
                        nearbySafeSeatToAttackedSeat = tempNearby;
                        seatToAttack = tempLowest;
                    }
                }

            }
        }


        _seatReadyToAttack = seatToAttack;
        _nearbySafeSeatToAttackedSeat = nearbySafeSeatToAttackedSeat;
    }

    private void SwitchCooldown()
    {
        StartCoroutine(SwitchCooldownRoutine());
    }

    private IEnumerator SwitchCooldownRoutine()
    {
        _switchCooldown = true;
        yield return new WaitForSeconds(_switchCoolDownTime);
        _switchCooldown = false;
    }

    private void AttackCooldown()
    {
        StartCoroutine(AttackCooldownRoutine());
    }

    private IEnumerator AttackCooldownRoutine()
    {
        _attackCooldown = true;
        yield return new WaitForSeconds(_attackCoolDownTime);
        _attackCooldown = false;
    }


    private void Defendooldown()
    {
        StartCoroutine(DefendCooldownRoutine());
    }

    private IEnumerator DefendCooldownRoutine()
    {
        _defendCooldown = true;
        yield return new WaitForSeconds(_defendCoolDownTime);
        _defendCooldown = false;
    }
}
