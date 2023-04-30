using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SetUnit(Unit sponseredUnit)
    {
        _sponsoredUnit = sponseredUnit;
        _sponsoredUnitColor = sponseredUnit.GetComponent<SpriteRenderer>().color;
        _sponsoredUnit.Died.AddListener(OnUnitDied);
        StartSponsoring();
    }

    private void OnUnitDied()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    public void StartSponsoring()
    {
        StartCoroutine(SponsoringRoutine());
    }

    private IEnumerator SponsoringRoutine()
    {
        yield return new WaitForSeconds(Random.Range(_sponsorSpeedMin, _sponsorSpeedMax));
        GameObject giftGO = Instantiate(_giftPrefab, this.transform.position, Quaternion.identity);
        Gift gift = giftGO.GetComponent<Gift>();
        gift.GetComponentInChildren<SpriteRenderer>().color = _sponsoredUnitColor;

        gift.Inicialize(_sponsoredUnit.GetSeat(), _giftsData[Random.Range(0, _giftsData.Count)]);
        StartCoroutine(SponsoringRoutine());
    }



}
