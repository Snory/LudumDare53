using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


public class Table : MonoBehaviour
{
    [SerializeField]
    private GeneralEvent Reorganized;

    [SerializeField]
    private float _minRandomEventTime;

    [SerializeField]
    private float _maxRandomEventTime;

    [SerializeField]
    private bool _instantiate;

    [SerializeField]
    private GameObject _sponsorPrefab;

    [SerializeField]
    private GameObject _unitPrefab;

    [SerializeField]
    private GameObject _seatPrefab;

    [SerializeField]
    private GameObject _twistPrefab;

    [SerializeField]
    private GameObject _enemyPrefab;

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

    [SerializeField]
    private GeneralEvent _gameOver;


    [SerializeField]
    private GeneralEvent _addScoreToRepository;

    [SerializeField]
    private GeneralEvent _seatDestroyed;

    private void Awake()
    {
        if (_instantiate)
        {
            PrepareTable();
        }

        StartCoroutine(StartGenerateRandomEventsRoutine());
    }

    public List<Seat> GetSeats()
    {
        return _seats;
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
    
            //prepare RequestingUnit
            GameObject unitGO = Instantiate(_unitPrefab, this.transform.root);
            unitGO.name = "Unit" + i.ToString();
            Unit unit = unitGO.GetComponent<Unit>();
            unit.GetComponent<SpriteRenderer>().color = _colors[i];

            //prepare Sponsors
            GameObject sponsorGO = Instantiate(_sponsorPrefab, this.transform.position, Quaternion.identity, this.transform.root);
            Sponsor sponsor = sponsorGO.GetComponent<Sponsor>();

            //settle the RequestingUnit into the SeatRequestor
            seat.SetUnit(unit);

            //set Sponsor for Unit
            sponsor.SetUnit(unit);

            if (i == 0)
            {
                _player.SetUnit(unit);
                sponsor.SetPlayerSponsor();
            }
            else
            {
                GameObject enemyObject = Instantiate(_enemyPrefab);
                Enemy enemy = enemyObject.GetComponent<Enemy>();
                enemy.Inicialize(this, unit);
            }

            sponsor.StartSponsoring();


            angle += angleStep;
        }

        OrganizeSeats();
    }


    [ContextMenu("Reorganize seats")]
    public void OrganizeSeats()
    {
        if (_seats.Count == 0)
        {
            return;
        }

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

        Reorganized.Raise();
    }



    public IEnumerator StartGenerateRandomEventsRoutine()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(_minRandomEventTime, _maxRandomEventTime));
        GenerateRandomEvents();
        StartCoroutine(StartGenerateRandomEventsRoutine());
    }

    [ContextMenu("Generate random events")]
    public void GenerateRandomEvents()
    {
        int numberOfEvents = UnityEngine.Random.Range(0, _seats.Count / 2);

        int[] usedSeats = new int[numberOfEvents];

        int searchCount = 0;
        int maxSearchCOunt = _seats.Count;

        for (int i = 0; i < numberOfEvents; i++)
        {
            int randomSeat = UnityEngine.Random.Range(0, _seats.Count);
            bool unusable = _seats[randomSeat].IsSeatDefended() || _seats[randomSeat].IsSeatUnderAttack();

            while (usedSeats.Contains(randomSeat) || unusable)
            {
                randomSeat = UnityEngine.Random.Range(0, _seats.Count);
                unusable = _seats[randomSeat].IsSeatDefended() || _seats[randomSeat].IsSeatUnderAttack();
                searchCount++;

                if (searchCount > maxSearchCOunt)
                {
                    break;
                }
            }

            searchCount = 0;

            usedSeats[i] = randomSeat;

            GameObject twist = Instantiate(_twistPrefab, _seats[randomSeat].transform.position, Quaternion.identity);
            Twist t = twist.GetComponent<Twist>();
            t.SetSeat(_seats[randomSeat]);
        }
    }


    public void OnShowNearbySeatsRequest(EventArgs args)
    {
        foreach (var seat in _seats)
        {
            seat.HightLightAttack(false);
        }

        ShowNearbySeatsEventArgs showNearbySeatsEventArgs = args as ShowNearbySeatsEventArgs;

        if (!showNearbySeatsEventArgs.Show)
        {
            return;
        }

        foreach (var seat in GetNearbySeats(showNearbySeatsEventArgs.SeatRequestor, showNearbySeatsEventArgs.Range))
        {
            seat.HightLightAttack(true);
        }
    }

    public int GetDistanceBetweenSeats(Seat fromSeat, Seat toSeat)
    {
        int fromIndex = _seats.IndexOf(fromSeat);
        int toIndex = _seats.IndexOf(toSeat);


        int toRightDistance = 0;
        int iterative = fromIndex;
        //to the right
        while (iterative != toIndex)
        {
            toRightDistance++;
            iterative++;

            if (iterative > _seats.Count)
            {
                iterative = 0;
            }
        }


        int toLeftDistance = 0;
        //to the left
        while (iterative != toIndex)
        {
            toRightDistance++;
            iterative--;

            if (iterative < 0)
            {
                iterative = _seats.Count;
            }
        }

        return Math.Min(toLeftDistance, toRightDistance);
    }

    public List<Seat> GetNearbySeats(Seat fromSeat, int range)
    {
        List<Seat> seats = new List<Seat>();

        int fromSeatIndex = _seats.IndexOf(fromSeat);

        int currentIndex = fromSeatIndex;

        //first positive
        for (int i = 0; i < range; i++)
        {
            bool searching = true;
            while (searching)
            {
                if (currentIndex + 1 >= _seats.Count)
                {
                    currentIndex = 0;
                }
                else
                {
                    currentIndex++;
                }

                if (currentIndex >= _seats.Count || currentIndex < 0)
                {
                    Debug.LogError("TOP - CurrentIndex: " + currentIndex.ToString() + " range: " + range.ToString());
                }

                if (fromSeatIndex == currentIndex) //cant find any
                {
                    break;
                }
                else if (_seats[currentIndex].GetSeatedUnit() != null) //found
                {
                    if (fromSeat != _seats[currentIndex])
                    {
                        seats.Add(_seats[currentIndex]);
                    }
                    break;
                }
            }
        }

        currentIndex = fromSeatIndex;
        //and negative
        for (int i = range; i > 0; i--)
        {
            bool searching = true;
            while (searching)
            {
                if (currentIndex - 1 < 0)
                {
                    currentIndex = _seats.Count - 1;
                }
                else
                {
                    currentIndex--;
                }

                if (currentIndex >= _seats.Count() || currentIndex < 0)
                {
                    Debug.LogError("BOTTOM - CurrentIndex: " + currentIndex.ToString() + " range: " + range.ToString());
                }

                if (fromSeatIndex == currentIndex) //cant find any
                {
                    break;
                }
                else if (_seats[currentIndex].GetSeatedUnit() != null) //found
                {
                    if (fromSeat != _seats[currentIndex])
                    {
                        seats.Add(_seats[currentIndex]);
                    }
                    break;
                }
            }
        }

        return seats;

    }

    public List<Seat> GetSeatsWithLowestHealth()
    {
        //check for Seat with lowest HP
        int lowestHealth = int.MaxValue;

        for (int i = 1; i < _seats.Count; i++)
        {
            Seat inspectedSeat = _seats[i];
            if (lowestHealth > inspectedSeat.GetSeatedUnit().GetHealth())
            {
                lowestHealth = inspectedSeat.GetSeatedUnit().GetHealth();
            }
        }

        List<Seat> listOfSeatsWithLowestHealth = _seats.Where(s => s.GetSeatedUnit().GetHealth() == lowestHealth).ToList();

        return listOfSeatsWithLowestHealth;
    }

    public List<Seat> GetSeatsWithHighestScore()
    {
        //check for Seat with lowest HP
        int highestCore = 0;

        for (int i = 1; i < _seats.Count; i++)
        {
            Seat inspectedSeat = _seats[i];
            if (highestCore < inspectedSeat.GetSeatedUnit().GetScore())
            {
                highestCore = inspectedSeat.GetSeatedUnit().GetScore();
            }
        }

        List<Seat> listOfSeatsWithHighestHealth = _seats.Where(s => s.GetSeatedUnit().GetScore() == highestCore).ToList();

        return listOfSeatsWithHighestHealth;
    }

    public List<Seat> GetSeatsWithHighestHealth()
    {
        //check for Seat with lowest HP
        int highestHealth = 0;

        for (int i = 1; i < _seats.Count; i++)
        {
            Seat inspectedSeat = _seats[i];
            if (highestHealth < inspectedSeat.GetSeatedUnit().GetHealth())
            {
                highestHealth = inspectedSeat.GetSeatedUnit().GetHealth();
            }
        }

        List<Seat> listOfSeatsWithHighestHealth = _seats.Where(s => s.GetSeatedUnit().GetHealth() == highestHealth).ToList();

        return listOfSeatsWithHighestHealth;
    }

    public List<Seat> GetSeatsUnderAttack()
    {
        List<Seat> listOfSeatsWithLowestHealth = _seats.Where(s => s.IsSeatUnderAttack()).ToList();

        return listOfSeatsWithLowestHealth;
    }

    public void OnUnitDied(EventArgs args)
    {
        UnitEventArgs unitEventArgs = args as UnitEventArgs;

        //remove Seat of the unit and destroy it
        _seatDestroyed.Raise(new SeatEventArgs(unitEventArgs.Unit.GetSeat()));
        Destroy(unitEventArgs.Unit.gameObject);
        _seats.Remove(unitEventArgs.Unit.GetSeat());
        Destroy(unitEventArgs.Unit.GetSeat().gameObject);

        OrganizeSeats();

        //check for game over conditions
        if (unitEventArgs.Unit == _player.GetUnit() || _seats.Count == 1)
        {
            Debug.Log("End");
            StartCoroutine(WaitWhileMoving());
        }
    }

    private IEnumerator WaitWhileMoving()
    {
        bool moving = true;
        while (moving)
        {
            moving = false;
            foreach (var seat in _seats)
            {

                moving = seat.IsSeatMovedInto();

                if (moving)
                {
                    break;
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(1);

        if(FindObjectOfType<LeaderBoard>() != null)
        {
            _addScoreToRepository.Raise(new ScoreEventData(new ScoreData(_player.GetUnit().GetScore()), OnScoreSaved));
        } else
        {
            OnScoreSaved();
        }
    }

    private void OnScoreSaved()
    {
        _gameOver.Raise(new GameOverEventArgs(_player.GetUnit().GetScore()));
    }

    public void OnSwitchSeatRequest(EventArgs args)
    {
        SwitchSeatRequestEventArgs seatSwitchRequestEventArgs = args as SwitchSeatRequestEventArgs;
        Seat requestorSeat = _seats.Where(s => s.GetSeatedUnit() == seatSwitchRequestEventArgs.RequestingUnit).FirstOrDefault();
        bool canSwitch = seatSwitchRequestEventArgs.RequestedSeat.CanBeSwitched();

        if (canSwitch)
        {
            SwapPositionAtTable(requestorSeat, seatSwitchRequestEventArgs.RequestedSeat);
        }

        seatSwitchRequestEventArgs.SeatSwitchedCallback(canSwitch);
    }

    public void OnAttackSeatRequest(EventArgs args)
    {
        AttackSeatRequestEventArgs attackRequestEventArgs = args as AttackSeatRequestEventArgs;
        Seat requestorSeat = _seats.Where(s => s.GetSeatedUnit() == attackRequestEventArgs.RequestingUnit).FirstOrDefault();

        List<Seat> seatsNearBy = GetNearbySeats(requestorSeat, attackRequestEventArgs.AttackRange);
        bool attackSuccesfull = seatsNearBy.Where(s => !s.IsSeatDefended()).Contains(attackRequestEventArgs.AttackedSeat);

        attackRequestEventArgs.RequestAttackCallback(attackSuccesfull);
    }

    public void OnDefendSeatRequest(EventArgs args)
    {
        DefendSeatRequestEventArgs defendSeatRequestEventArgs = args as DefendSeatRequestEventArgs;
        defendSeatRequestEventArgs.RequestedSeat.Defend(defendSeatRequestEventArgs.DefendTime);
        defendSeatRequestEventArgs.SeatDefendCallback(true);
    }

    private void SwapPositionAtTable(Seat requestor, Seat requested)
    {

        Unit requestorUnit = requestor.GetSeatedUnit();
        Unit requestedUnit = requested.GetSeatedUnit();

        if (requestorUnit != null)
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
