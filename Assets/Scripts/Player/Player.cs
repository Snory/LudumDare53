using System;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Button _attack, _defend, _switch;

    [SerializeField]
    private Color _pressedColor;

    private ColorBlock _attackColorBlock, _defendColorBlock, _switchColorBlock;

    private void Awake()
    {
        _currentAction = PlayerAction.ATTACK;
        _camera = Camera.main;
        _paused = false;

        _attack.onClick.AddListener(AttackAction);
        _attackColorBlock = _attack.colors;
        _defend.onClick.AddListener(DefendAction);
        _defendColorBlock = _defend.colors;
        _switch.onClick.AddListener(SwitchAction);
        _switchColorBlock = _switch.colors;

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

    public void AttackAction()
    {
        _currentAction = PlayerAction.ATTACK;
        ShowNearbySeats(true);
        ClearButtonColor();
        _attackColorBlock.normalColor = _pressedColor;
        SetButtonColor();

    }

    public void SwitchAction()
    {
        _currentAction = PlayerAction.SWITCH;
        ClearButtonColor();
        _switchColorBlock.normalColor = _pressedColor;
        SetButtonColor();
    }

    public void DefendAction()
    {
        _currentAction = PlayerAction.DEFEND;
        ClearButtonColor();
        _defendColorBlock.normalColor = _pressedColor;
        SetButtonColor();
    }

    private void ClearButtonColor()
    {
        _attackColorBlock.normalColor = Color.white;
        _defendColorBlock.normalColor = Color.white;
        _switchColorBlock.normalColor = Color.white;
    }

    private void SetButtonColor()
    {
        _attack.colors = _attackColorBlock;
        _defend.colors = _defendColorBlock;
        _switch.colors = _switchColorBlock;
    }





    public void ObservePlayerAction()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AttackAction();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SwitchAction();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            DefendAction();
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
        SwitchAction();
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


