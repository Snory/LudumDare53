using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class SpellProjectile : MonoBehaviour
{
    private SpellData _spellData;

    private Seat _seat;

    [SerializeField]
    private float _movementPerSecond;

    [SerializeField]
    private float _movingDistanceTreshold;

    private Action<int> _addScoreCallback;


    public void Inicialize(Seat finalSeat, SpellData spellData, Action<int> addScore)
    {
        _seat = finalSeat;
        _spellData = spellData;
        _addScoreCallback = addScore;
        StartCoroutine(Move());
    }


    private void OnEmptySeat(Seat s)
    {
        Stop();
    }


    private IEnumerator Move()
    {
        if (_seat != null)
        {
            while (this.transform.position != _seat.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, _seat.transform.position, _movementPerSecond * Time.deltaTime);
                yield return new WaitForSeconds(0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Seat")
        {
            if (collision.GetComponent<Seat>() == _seat)
            {
                AttackSeat();
            }
        }
    }

    public void OnSeatRemoved(EventArgs args)
    {
        SeatEventArgs seatEventArgs = args as SeatEventArgs;

        if (seatEventArgs.Seat == _seat)
        {
            Destroy(this.gameObject);
        }
    }

    private void AttackSeat()
    {
        _seat.TakeDamage(_spellData.Power);

        _addScoreCallback.Invoke(10);

        Destroy(this.gameObject);
    }

    public void Stop()
    {
        StopAllCoroutines();
        Destroy(this.gameObject );
    }

}

