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

    public void Inicialize(Seat finalSeat, SpellData spellData)
    {
        _seat = finalSeat;
        _spellData = spellData;
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

    private void AttackSeat()
    {
        _seat.TakeDamage(_spellData.Power);
        Destroy(this.gameObject);
    }

}

