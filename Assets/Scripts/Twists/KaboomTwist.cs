using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaboomTwist : MonoBehaviour
{


    [SerializeField]
    private float _anticipationTimeMax;
    private float _anticipationTimeCurrent;
    private bool _anticipated;
    [SerializeField]
    private SpriteRenderer _anticipationSpriteRenderer;
    private Color _anticipatedColor;

    [SerializeField]
    private float _kaboomScaleMax;

    [SerializeField]
    private float _kaboomTimeMax;

    private bool _kaboombed;

    private float _kaboomTimeCurrent;
    [SerializeField]
    private GameObject _kaboomGameObject;

    private void Awake()
    {
        _anticipated = false;
        _kaboombed = false;
        _anticipatedColor = _anticipationSpriteRenderer.color;
        _anticipationSpriteRenderer.color = new Color(_anticipatedColor.r, _anticipatedColor.g, _anticipatedColor.b, 0);
        _kaboomGameObject.transform.localScale = Vector3.zero;
        StartAnticipation();
    }

    [ContextMenu("RunShadow")]
    private void StartAnticipation()
    {
        StartCoroutine(AnticipationRoutine());
    }

    private IEnumerator AnticipationRoutine()
    {
        float currentAlpha = 0;
        float alphaStep = (1 / _anticipationTimeMax);
        _anticipationSpriteRenderer.color = new Color(_anticipatedColor.r, _anticipatedColor.g, _anticipatedColor.b, currentAlpha);

        while (!_anticipated)
        {
            currentAlpha += alphaStep * Time.deltaTime;
            _anticipationSpriteRenderer.color = new Color(_anticipatedColor.r, _anticipatedColor.g, _anticipatedColor.b, currentAlpha);
            yield return null;

            _anticipationTimeCurrent += Time.deltaTime;

            if (_anticipationTimeCurrent > _anticipationTimeMax)
            {
                _anticipated = true;
                _anticipationSpriteRenderer.enabled = false;
                StartKaboomRoutine();
            }
        }
    }

    private void StartKaboomRoutine()
    {
        StartCoroutine(KaboomRoutine());

    }

    private IEnumerator KaboomRoutine()
    {
        float currentScale = 0;
        float scaleStep = (_kaboomScaleMax / _kaboomTimeMax);
        Debug.Log("Scale step is: " + scaleStep);


        while (!_kaboombed)
        {
            currentScale += scaleStep * Time.deltaTime;
            _kaboomGameObject.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            Debug.Log(currentScale);

            yield return null;

            _kaboomTimeCurrent += Time.deltaTime;

            if (_kaboomTimeCurrent > _kaboomTimeMax)
            {
                _kaboombed = true;
                Destroy(this.gameObject);
                
            }
        }
    }


}
