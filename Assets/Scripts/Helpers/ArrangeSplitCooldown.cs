using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ArrangeSplitCooldown : MonoBehaviour
{
    public Sprite Sprite;

    public int Count;

    public int Angle;

    public GameObject CoolDownSplitPrefab;

    public float DistanceFromCenter;

    public float StartAngle;

    private List<GameObject> shards;

    [Range(-1,1)]
    public int Direction;


    [ContextMenu("Empty")]
    public void EmptyShards()
    {
        shards = null;
    }

    [ContextMenu("Arrange")]
    public void Arrange()
    {
        if(shards != null)
        {
            foreach(var shard in shards)
            {
                DestroyImmediate(shard.gameObject);
            }
        }

        shards = new List<GameObject>();

        float angleStep = Angle / Count;

        float angle = StartAngle;

        for (int i = 0; i < Count; i++)
        {
            float x = this.transform.position.x + Mathf.Sin(angle * Mathf.Deg2Rad) * DistanceFromCenter;
            float y = this.transform.position.y + Mathf.Cos(angle * Mathf.Deg2Rad) * DistanceFromCenter;

            Vector2 position = new Vector2(x, y);

            Quaternion rotation = Quaternion.Euler(0, 0, -angle);

            GameObject o = Instantiate(CoolDownSplitPrefab, position, rotation, this.transform);
            o.GetComponent<SpriteRenderer>().sprite = Sprite;
            shards.Add(o);

            angle += (Direction) * angleStep;
        }
    }

}
