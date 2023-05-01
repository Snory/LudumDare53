using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Seat : MonoBehaviour
{
    [SerializeField]
    private bool _movedInto;

    [SerializeField]
    private bool _defended;

    [SerializeField]
    private bool _underAttack;

    private Unit _unit;

    [SerializeField]
    private SpriteRenderer _defendSpriteRendered;

    [SerializeField]
    private SpriteRenderer _attackSpriteRendered;

    [SerializeField]
    private float _seatTime;

    private void Update()
    {
        _seatTime += Time.deltaTime;
    }

    public float GetSeatTime()
    {
        return _seatTime;
    }

    private void Awake()
    {
        _defendSpriteRendered.enabled = false;
        _attackSpriteRendered.enabled = false;
    }

    public void SetUnderAttack(bool underAttack)
    {
        _underAttack = underAttack;
    }

    public bool IsSeatUnderAttack()
    {
        return _underAttack;
    }

    public bool IsSeatMovedInto()
    {
        return _movedInto;
    }

    public bool IsSeatDefended()
    {
        return _defended;
    }

    public bool CanBeSwitched()
    {
        return !_movedInto && !_defended;
    }

    public void TakeDamage(int powerAttack)
    {
        if (_defended)
        {
            SetDefend(false);
        } else
        {
            _unit.TakeDamage(powerAttack);
        }
    }

    public void SetUnit(Unit unit)
    {
        if(_unit != null)
        {
            _unit.Moved.RemoveListener(OnUnitMoved);
        }
        _unit = unit;
        _unit.Moved.AddListener(OnUnitMoved);
    }

    public void AttackSeat(SpellData spellData)
    {
        _unit.TakeDamage(spellData.Power);
    }

    public void GiveGift(GiftData giftData, bool correctRecipient)
    {
        giftData.ApplyGift(_unit, correctRecipient);
    }

    public void OnUnitMoved(bool moved)
    {
        _movedInto = moved;
    }

    public void SettleUnit()
    {
        _unit.SetSeat(this);
        _seatTime = 0;
    }

    public Unit GetSeatedUnit()
    {
        return _unit;
    }

    public void HightLightAttack(bool highLight)
    {
        _attackSpriteRendered.enabled = highLight;
    }

    private void SetDefend(bool defend)
    {
        _defendSpriteRendered.enabled = defend;
        _defended = defend;
    }

    public void Defend(float defendTime)
    {
        SetDefend(true);
        StartCoroutine(HighlightDefendRoutine(defendTime));
    }

    public IEnumerator HighlightDefendRoutine(float hightLightTime)
    {
        yield return new WaitForSeconds(hightLightTime);
        _defendSpriteRendered.enabled = false;
        _defended = false;
    }
}
