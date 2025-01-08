using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private BoxCollider _boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnableCollider());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
    }

    private IEnumerator EnableCollider()
    {
        float time = 0.5f;
        while (time > 0)
        {
            if (_boxCollider != null)
            {
                _boxCollider.enabled = false;
            }
            else
            {
                _boxCollider= GetComponent<BoxCollider>();
            }
            time -= Time.deltaTime;
            yield return null;
        }

        _boxCollider.enabled=true;
    }
}
