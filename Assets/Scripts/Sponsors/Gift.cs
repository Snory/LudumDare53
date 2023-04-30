using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gift : MonoBehaviour
{
    private GiftData _giftData;

    private Seat _seat;

    [SerializeField]
    private float _movementPerSecond;

    [SerializeField]
    private float _movingDistanceTreshold;

    [SerializeField]
    private SpriteRenderer _giftSpriteRenderer;

    public void Inicialize(Seat finalSeat, GiftData giftData)
    {
        _seat = finalSeat;
        _giftData = giftData;
        _giftSpriteRenderer.sprite = _giftData.GiftSprite;
        _seat.SeatEmpty.AddListener(OnEmptySeat);
        StartCoroutine(Move());
    }

    private void OnEmptySeat(Seat s)
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
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
        _seat.GiveGift(_giftData);
        Destroy(this.gameObject);
    }
}
