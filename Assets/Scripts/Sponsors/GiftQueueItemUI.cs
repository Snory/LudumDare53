using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GiftQueueItemUI : MonoBehaviour
{
    [SerializeField]
    private Image _queueImage;

    private float _sponsorTime;

    private float _sponsorTimeMax;

    private bool _dequeuing;

    public void Prepare(GiftData data)
    {
        _queueImage.sprite = data.GetUnwrapedGiftSprite();
    }

    public void Dequeued(float waitTime)
    {
        _sponsorTimeMax = waitTime;
        _sponsorTime = _sponsorTimeMax;

        StartCoroutine(Dequing());
    }

    private IEnumerator Dequing()
    {
        _dequeuing = true;
        while (_dequeuing)
        {
            yield return new WaitForSeconds(0);

            _sponsorTime -= Time.deltaTime;

            _queueImage.fillAmount = _sponsorTime / _sponsorTimeMax;

            if (_sponsorTime < 0)
            {
                _dequeuing = false;
                Destroy(this.gameObject);
            }
        }
    }

}
