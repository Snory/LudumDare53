using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
    private SpellData _spellData;
    private int _usageCounterMax;
    private int _usageCounter;

    [SerializeField]
    private Image _spellImage;

    public UnityEvent Used;

    [SerializeField]
    private Image _usageImage;

    [SerializeField]
    private GameObject _spellProjectilePrefab;


    public void Initialize(SpellData data)
    {
        _usageCounter = data.UsageCounter == 0 ? int.MaxValue : data.UsageCounter;
        _usageCounterMax = _usageCounter;
        _spellImage.sprite = data.SpellSprite;
        _spellData = data;

    }

    public void UseSpell(Seat attackedSeat, Action<int> AddScoreCallback)
    {
        _usageCounter--;

        GameObject spellProjectileGO = Instantiate(_spellProjectilePrefab, this.transform.position, Quaternion.identity, this.transform.root);
        SpellProjectile spellProjectile = spellProjectileGO.GetComponent<SpellProjectile>();
        spellProjectile.Inicialize(attackedSeat, _spellData, AddScoreCallback);

        if(_usageCounter == 0)
        {
            Used?.Invoke();
            Destroy(this.gameObject);
        }

        _usageImage.fillAmount = _usageCounter / (float) _usageCounterMax;
    }

    public int GetSpellRange()
    {
        return _spellData.Range;
    }


}
