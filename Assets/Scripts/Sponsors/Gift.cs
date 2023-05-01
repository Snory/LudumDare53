using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gift : MonoBehaviour
{
    private Unit _unitWhichShouldBeSponsored;

    private GiftData _giftData;

    private Seat _seat;

    [SerializeField]
    private float _movementPerSecond;

    [SerializeField]
    private float _movingDistanceTreshold;

    public void Inicialize(Seat finalSeat, GiftData giftData, Unit sponsoredUnit)
    {
        _seat = finalSeat;
        _giftData = giftData;
        _unitWhichShouldBeSponsored = sponsoredUnit;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        if (_seat != null)
        {
            while (this.transform.position != _seat.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, _seat.transform.position, _movementPerSecond * Time.deltaTime);
                yield return null;

                if (Vector2.Distance(this.transform.position, _seat.transform.position) < _movingDistanceTreshold)
                {
                    break;
                }
            }

            GiftGive();
        }
    }

    private void GiftGive()
    {
        bool correctRecipient = _unitWhichShouldBeSponsored == _seat.GetSeatedUnit();  
        _seat.GiveGift(_giftData, correctRecipient);
        Destroy(this.gameObject);
    }

    public void OnUnitDied(EventArgs args)
    {
        UnitEventArgs unitEventArgs = args as UnitEventArgs;

        if (unitEventArgs.Unit == _unitWhichShouldBeSponsored)
        {
            Destroy(this.gameObject);
        }
    }
}
