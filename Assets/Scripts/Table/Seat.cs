using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Seat : MonoBehaviour
{
    private Unit _unit;

    [SerializeField]
    private SpriteRenderer _defendSpriteRendered;

    [SerializeField]
    private SpriteRenderer _attackSpriteRendered;

    public UnityEvent<Seat> SeatEmpty;

    private void Awake()
    {
        _defendSpriteRendered.enabled = false;
        _attackSpriteRendered.enabled = false;
    }

    public void SetUnit(Unit unit)
    {
        if(_unit != null)
        {
            _unit.Died.RemoveListener(OnUnitDeath);
        }
        _unit = unit;
        _unit.Died.AddListener(OnUnitDeath);
    }

    public void SettleUnit()
    {
        _unit.SetSeat(this);
        _unit.MoveUnitToSeat();
    }

    private void OnUnitDeath()
    {
        SeatEmpty?.Invoke(this);
        Destroy(this.gameObject);
    }
    public Unit GetSeatedUnit()
    {
        return _unit;
    }

    public void HightLightAttack(bool highLight)
    {
        _attackSpriteRendered.enabled = highLight;
    }

    public void HightLightDefend(bool highLight, float hightLightTime)
    {
        _defendSpriteRendered.enabled = highLight;
        StartCoroutine(HighlightDefendRoutine(hightLightTime));
    }

    public IEnumerator HighlightDefendRoutine(float hightLightTime)
    {
        yield return new WaitForSeconds(hightLightTime);
        _defendSpriteRendered.enabled = false;
    }
}
