using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Unit _playerUnit;

    public enum PlayerAction { NONE, ATTACK, DEFEND, SWITCH, DEBUG }
    private PlayerAction _currentAction;

    [SerializeField]
    private LayerMask _selectable;
    private Camera _camera;

    [SerializeField]
    private GeneralEvent ShowNearbySeatsRequested;

    private bool _paused;


    private void Awake()
    {
        _currentAction = PlayerAction.NONE;
        _camera = Camera.main;
        _paused = false;
    }

    private void Update()
    {
        if (_paused)
        {
            return;
        }

        ObservePlayerAction();
        SwitchSeats();
        Attack();
        Defend();
    }

    public void OnPauseGame()
    {
        _paused = !_paused;
    }


    public void ObservePlayerAction()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _currentAction = PlayerAction.ATTACK;
            ShowNearbySeats(true);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _currentAction = PlayerAction.SWITCH;
            ShowNearbySeats(false);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _currentAction = PlayerAction.DEFEND;
        }
    }

    private void ShowNearbySeats(bool show)
    {
        _playerUnit.RaiseMarkNearbySetRequest(show);
    }

    private void SwitchSeats()
    {

        if (_currentAction != PlayerAction.SWITCH)
        {
            return;
        }

        if (!_playerUnit.CanPerformAction())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseInWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit2d = Physics2D.Raycast(mouseInWorldPosition, _camera.transform.forward, Mathf.Infinity, _selectable);

            if (hit2d)
            {
                _playerUnit.RaiseSwitchSeatRequest(hit2d.collider.GetComponent<Seat>());
            }
        }
    }


    private void Attack()
    {

        if (_currentAction != PlayerAction.ATTACK)
        {
            return;
        }

        if (!_playerUnit.CanPerformAction())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseInWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit2d = Physics2D.Raycast(mouseInWorldPosition, _camera.transform.forward, Mathf.Infinity, _selectable);

            if (hit2d)
            {
                _playerUnit.RaiseAttackSeatRequest(hit2d.collider.GetComponent<Seat>());
                ShowNearbySeats(true);
            }
        }
    }

    private void Defend()
    {
        if (_currentAction != PlayerAction.DEFEND)
        {
            return;
        }

        if (!_playerUnit.CanPerformAction())
        {
            return;
        }

        _playerUnit.RaiseDefendSeatRequest();
        _currentAction = PlayerAction.SWITCH;
    }

    public void SetUnit(Unit unit)
    {
        _playerUnit = unit;
        _playerUnit.Moved.AddListener(OnUnitMoved);
    }

    public Unit GetUnit()
    {
        return _playerUnit;
    }

    private void OnUnitMoved(bool moved)
    {
        if (!moved)
        {
            ShowNearbySeats(true);
        }
    }
}


