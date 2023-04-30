using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { TEST }

    private EnemyState _enemyState;

    private Unit _enemyUnit;
    private Table _table;

    [SerializeField]
    private float _decisionCoolDownMin, _decisionCoolDownMax;


    private float _decisionCoolDownCurrentRandom;
    private float _decisionCooldownCurrent;

    private Seat _seatReadyToAttack;

    public void Inicialize(Table table, Unit unit)
    {
        _enemyUnit = unit;
        _enemyUnit.Died.AddListener(OnUnitDied);
        _table = table;
        _enemyState = EnemyState.TEST;
        _decisionCooldownCurrent = Random.Range(_decisionCoolDownMin, _decisionCoolDownMax);
    }


    private void Update()
    {
        TestState();
    }

    public void OnUnitDied()
    {
        Destroy(this.gameObject);
    }

    public void TestState()
    {
        if (_enemyState != EnemyState.TEST)
        {
            return;
        }

        if (_decisionCooldownCurrent > 0)
        {
            _decisionCooldownCurrent -= Time.deltaTime;
            return;
        }


        if (_enemyUnit.CanPerformAction())
        {
            List<Seat> seats = _table.GetSeats();

            if (seats.Count == 1)
            {
                return; //just in case
            }

            bool delay = Behaviour(seats);

            if (delay)
            {
                _decisionCoolDownCurrentRandom = Random.Range(_decisionCoolDownMin, _decisionCoolDownMax);
                _decisionCooldownCurrent = _decisionCoolDownCurrentRandom;
            }
        }
    }


    private bool Behaviour(List<Seat> seats)
    {

        List<Seat> listToAttack = null;
        float randomValue = Random.Range(0, 10);

        if (_seatReadyToAttack != null)
        {
            listToAttack = new List<Seat> { _seatReadyToAttack };
        }
        else if (randomValue > 8)
        {
            listToAttack = _table.GetSeatsWithHighestHealth().Where(s => !s.IsSeatDefended()).ToList();
        }
        else
        {
            listToAttack = _table.GetSeatsWithLowestHealth().Where(s => !s.IsSeatDefended()).ToList();
        }

        Seat seatToAttack = null;
        Seat nearbySafeSeatToAttackedSeat = null;

        if (listToAttack.Count > 0)
        {
            seatToAttack = listToAttack[0];
            nearbySafeSeatToAttackedSeat =
                    _table.GetNearbySeats(seatToAttack, _enemyUnit.GetAttackRange()).Where(s => !s.IsSeatMovedInto() && !s.IsSeatUnderAttack()).ToList()
                          .OrderByDescending(s => _table.GetDistanceBetweenSeats(s, seatToAttack)).FirstOrDefault();

            if (_seatReadyToAttack == null)
            {
                //get throught the rest if there is any
                for (int i = 1; i < listToAttack.Count; i++)
                {
                    Seat tempLowest = listToAttack[i];
                    Seat tempNearby = null;

                    //get lists of seats which are not under attack and can be moved into
                    //if enemy is in the seat and is under attack, it is removed, therefore enemy will not attack if he is under attack
                    List<Seat> nearbySeatsToLowestList = _table.GetNearbySeats(tempLowest, _enemyUnit.GetAttackRange()).Where(s => !s.IsSeatMovedInto() && !s.IsSeatUnderAttack()).ToList();

                    //Find nearby seat which is most far away from the seatToAttack heal unit
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

        /*
            If there is a danger from environment or there is a unit nearby while I have the seatToAttack HP or I am waiting for package
            THEN DEFEND MYSELF!!!
        */

        bool mySeatUnderAttack = _table.GetSeatsUnderAttack().Contains(_enemyUnit.GetSeat());
        bool lowestHp = _table.GetSeatsWithLowestHealth().Contains(_enemyUnit.GetSeat());

        if (mySeatUnderAttack)
        {
            if (seatToAttack != null)
            {
                if (!seatToAttack.IsSeatDefended())
                {
                    _enemyUnit.RaiseSwitchSeatRequest(seatToAttack);
                    _seatReadyToAttack = null;
                    return true;
                }
                else if (_enemyUnit.GetSeat() != nearbySafeSeatToAttackedSeat)
                {
                    _enemyUnit.RaiseSwitchSeatRequest(nearbySafeSeatToAttackedSeat);
                    return true;
                }
                else
                {
                    _enemyUnit.RaiseDefendSeatRequest();
                    return true;
                }
            }
        }

        if (nearbySafeSeatToAttackedSeat != null)
        {
            if (nearbySafeSeatToAttackedSeat == _enemyUnit.GetSeat())
            {
                _enemyUnit.RaiseAttackSeatRequest(seatToAttack);
                _seatReadyToAttack = null;
                return true;
            }
            else
            {
                _enemyUnit.RaiseSwitchSeatRequest(nearbySafeSeatToAttackedSeat);
                return false;
            }
        } else
        {
            _seatReadyToAttack = null;
        }

        return false;

    }
}
