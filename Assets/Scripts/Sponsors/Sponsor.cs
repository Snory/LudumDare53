using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Sponsor : MonoBehaviour
{
    [SerializeField]
    private List<GiftData> _giftsData;

    private Unit _sponsoredUnit;
    [SerializeField]
    private float _sponsorSpeedMin;
    [SerializeField]
    private float _sponsorSpeedMax;

    [SerializeField]
    private GameObject _giftPrefab;

    [SerializeField]
    private Color _sponsoredUnitColor;

    [SerializeField]
    private int _queueInAdvanceCount;

    [SerializeField]
    private Queue<GiftQueued> _giftQueue;

    [SerializeField]
    private GameObject _giftQueueItemUI;

    private Transform _contentQueueItemTransform;


    [SerializeField]
    private bool _playerSponsor;

    public void SetPlayerSponsor()
    {
        _playerSponsor = true;
        _contentQueueItemTransform = GameObject.FindWithTag("Queue").transform;
    }

    public void SetUnit(Unit sponseredUnit)
    {
        _sponsoredUnit = sponseredUnit;
        _sponsoredUnitColor = sponseredUnit.GetComponent<SpriteRenderer>().color;
    }
 
    public void OnUnitDied(EventArgs args)
    {
        UnitEventArgs unitEventArgs = args as UnitEventArgs;

        if(unitEventArgs.Unit == _sponsoredUnit)
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }

    public void StartSponsoring()
    {
        _giftQueue = new Queue<GiftQueued>();
        for(int i = 0; i < _queueInAdvanceCount;i++)
        {
            AddToQueue();
        }

        StartCoroutine(SponsoringRoutine());
    }

    private void AddToQueue()
    {
        GiftData data = _giftsData[Random.Range(0, _giftsData.Count)];

        if (!_playerSponsor)
        {
            _giftQueue.Enqueue(new GiftQueued(data));
            return;
        }

        GameObject queueItemUi = Instantiate(_giftQueueItemUI, _contentQueueItemTransform.position, Quaternion.identity, _contentQueueItemTransform);
        GiftQueueItemUI giftQueueItemUI = queueItemUi.GetComponent<GiftQueueItemUI>();

        giftQueueItemUI.Prepare(data);
        _giftQueue.Enqueue(new GiftQueued(data, giftQueueItemUI));
    }

    private IEnumerator SponsoringRoutine()
    {
        GiftQueued preparedGift = _giftQueue.Dequeue();
        float waitTime = Random.Range(_sponsorSpeedMin, _sponsorSpeedMax);

        if (preparedGift.GetPreparedQueueItemUI() != null)
        {
            preparedGift.GetPreparedQueueItemUI().Dequeued(waitTime);
        }

        yield return new WaitForSeconds(waitTime);
        GameObject giftGO = Instantiate(_giftPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
        Gift gift = giftGO.GetComponent<Gift>();
        gift.GetComponentInChildren<SpriteRenderer>().color = _sponsoredUnitColor;
        gift.Inicialize(_sponsoredUnit.GetSeat(), preparedGift.GetPreparedGiftData(), _sponsoredUnit);
        AddToQueue();

        StartCoroutine(SponsoringRoutine());

    }





}
