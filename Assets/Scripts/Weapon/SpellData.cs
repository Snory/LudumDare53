using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "NewWeapon", menuName = "Spell/NewWeapon")]
public class SpellData : ScriptableObject
{
    public Sprite SpellSprite;
    public int UsageCounter;
    public int Power;
    public int Range;

}
