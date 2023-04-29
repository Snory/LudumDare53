using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Table : MonoBehaviour
{

    [SerializeField]
    private bool _instantiate;

    [SerializeField]
    private GameObject _unitPrefab;

    [SerializeField]
    private GameObject _seatPrefab;

    [SerializeField]
    private GameObject _twistPrefab;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private int _playerCount;

    [SerializeField]
    private float _distanceFromCenter;

    [SerializeField]
    private List<Color> _colors;

    [SerializeField]
    private List<Seat> _seats;
        
    private void Awake()
    {
        if (_instantiate)
        {
            PrepareTable();
        }
    }

    [ContextMenu("Prepare table")]
    public void PrepareTable()
    {
        float angleStep = 360 / _playerCount;
        float angle = 0;

        _seats = new List<Seat>();

        for (int i = 0; i < _playerCount; i++)
        {
            // prepare SeatRequestor
            GameObject seatGo = Instantiate(_seatPrefab, this.transform);
            seatGo.name = "Seat" + i.ToString();
            Seat seat = seatGo.GetComponent<Seat>();
            _seats.Add(seat);
            seat.SeatEmpty.AddListener(OnSeatEmpty);

            //prepare RequestingUnit
            GameObject unitGO = Instantiate(_unitPrefab);
            unitGO.name = "RequestingUnit" + i.ToString();
            Unit unit = unitGO.GetComponent<Unit>();
            unit.GetComponent<SpriteRenderer>().color = _colors[i];

            if (i == 0)
            {
                _player.SetPlayerUnit(unit);
            }

            //settle the RequestingUnit into the SeatRequestor
            seat.SetUnit(unit);

            angle += angleStep;
        }

        OrganizeSeats();
    }

    [ContextMenu("Reorganize seats")]
    public void OrganizeSeats()
    {

        float angleStep = 360 / _seats.Count;
        float angle = 0;

        for (int i = 0; i < _seats.Count; i++)
        {
            float x = this.transform.position.x + Mathf.Sin(angle * Mathf.Deg2Rad) * _distanceFromCenter;
            float y = this.transform.position.y + Mathf.Cos(angle * Mathf.Deg2Rad) * _distanceFromCenter;

            Vector2 position = new Vector2(x, y);

            _seats[i].transform.position = position;
            _seats[i].SettleUnit();

            angle += angleStep;
        }
    }

    [ContextMenu("Generate random events")]
    public void GenerateRandomEvents()
    {
        int numberOfEvents = UnityEngine.Random.Range(0,_seats.Count/2);

        int[] usedSeats = new int[numberOfEvents];
        

        for(int i = 0; i < numberOfEvents; i++)
        {
            int randomSeat = UnityEngine.Random.Range(0, _seats.Count);
            while(usedSeats.Contains(randomSeat))
            {
                randomSeat = UnityEngine.Random.Range(0, _seats.Count);
            }
            
            usedSeats[i] = randomSeat;
            Instantiate(_twistPrefab, _seats[randomSeat].transform.position, Quaternion.identity);
        }
    }


    public void OnShowNearbySeatsRequest(EventArgs args)
    {
        foreach(var seat in _seats)
        {
            seat.HightLightAttack(false);
        }

        ShowNearbySeatsEventArgs showNearbySeatsEventArgs = args as ShowNearbySeatsEventArgs;

        if (!showNearbySeatsEventArgs.Show)
        {
            return;
        }

        foreach(var seat in GetNearbySeats(showNearbySeatsEventArgs.SeatRequestor, showNearbySeatsEventArgs.Range))
        {
            seat.HightLightAttack(true);
        }
    }

    private List<Seat> GetNearbySeats(Seat requestor, int range)
    {
        List<Seat > seats = new List<Seat>();

        int requestorIndex = _seats.IndexOf(requestor);

        int currentIndex = requestorIndex;

        //first positive
        for(int i = 0; i < range; i++)
        {
            bool searching = true;
            while (searching)
            {
                if(requestorIndex + 1 > _seats.Count - 1){
                    currentIndex = 0;
                } else
                {
                    currentIndex++;
                }

                if (requestorIndex == currentIndex) //cant find any
                {
                    break;
                }   else if (_seats[currentIndex].GetSeatedUnit() != null) //found
                {
                    seats.Add(_seats[currentIndex]);
                    break;
                } 
            }           
        }

        currentIndex = requestorIndex;
        //and negative
        for (int i = range; i > 0; i--)
        {
            bool searching = true;
            while (searching)
            {
                if (requestorIndex - 1 < 0)
                {
                    currentIndex = _seats.Count - 1;
                }
                else
                {
                    currentIndex--;
                }

                if (requestorIndex == currentIndex) //cant find any
                {
                    break;
                }
                else if (_seats[currentIndex].GetSeatedUnit() != null) //found
                {
                    seats.Add(_seats[currentIndex]);
                    break;
                }
            }
        }

        return seats;

    }

    public void OnSeatEmpty(Seat s)
    {
        _seats.Remove(s);
        OrganizeSeats();
    }

    public void OnSwitchSeatRequest(EventArgs args)
    {
        SwitchSeatRequestEventArgs seatSwitchRequestEventArgs = args as SwitchSeatRequestEventArgs;
        Seat requestorSeat = _seats.Where(s => s.GetSeatedUnit()  == seatSwitchRequestEventArgs.RequestingUnit).FirstOrDefault();        
        SwapPositionAtTable(requestorSeat, seatSwitchRequestEventArgs.RequestedSeat);
        seatSwitchRequestEventArgs.SeatSwitchedCallback(true);
    }

    public void OnAttackSeatRequest(EventArgs args)
    {
        AttackSeatRequestEventArgs attackRequestEventArgs = args as AttackSeatRequestEventArgs;
        Seat requestorSeat = _seats.Where(s => s.GetSeatedUnit() == attackRequestEventArgs.RequestingUnit).FirstOrDefault();

        List<Seat> seatsNearBy = GetNearbySeats(requestorSeat, attackRequestEventArgs.AttackRange);
        bool attackSuccesfull = seatsNearBy.Contains(attackRequestEventArgs.AttackedSeat);

        if (attackSuccesfull)
        {
            attackRequestEventArgs.AttackedSeat.GetSeatedUnit().TakeDamage(attackRequestEventArgs.AttackPower);
        }

        attackRequestEventArgs.RequestAttackCallback(attackSuccesfull);    
    }

    public void OnDefendSeatRequest(EventArgs args)
    {
        DefendSeatRequestEventArgs defendSeatRequestEventArgs = args as DefendSeatRequestEventArgs;
        defendSeatRequestEventArgs.RequestedSeat.HightLightDefend(true, defendSeatRequestEventArgs.DefendTime);
        defendSeatRequestEventArgs.SeatDefendCallback(true);
    }

    private void SwapPositionAtTable(Seat requestor, Seat requested)
    {

        Unit requestorUnit = requestor.GetSeatedUnit();
        Unit requestedUnit = requested.GetSeatedUnit();

        if(requestorUnit != null)
        {
            requested.SetUnit(requestorUnit);
            requested.SettleUnit();

        }

        if (requestorUnit != null)
        {
            requestor.SetUnit(requestedUnit);
            requestor.SettleUnit();
        }
    }
}
