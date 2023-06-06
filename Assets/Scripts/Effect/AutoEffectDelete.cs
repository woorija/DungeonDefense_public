using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEffectDelete : MonoBehaviour
{
    [SerializeField] float deletetime;
    void Start()
    {
        StartCoroutine(Delete());
    }

    IEnumerator Delete()
    {
        yield return YieldCache.WaitForSeconds(deletetime);
        Destroy(gameObject);
    }
}
