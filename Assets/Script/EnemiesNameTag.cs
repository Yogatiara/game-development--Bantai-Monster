using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesNameTag : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform enemyTransform;
    // Update is called once per frame
    void Update()
    {

        transform.localRotation = enemyTransform.rotation;
    }
}
