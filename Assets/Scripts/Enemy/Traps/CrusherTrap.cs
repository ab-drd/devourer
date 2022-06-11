using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherTrap : MonoBehaviour
{
    [Header("Crusher Components")]
    [SerializeField] GameObject crusherBase;
    [SerializeField] Transform crusherHead;
    [Header("Crusher Parameters")]
    [SerializeField] private float crushSpeed;
    [SerializeField] private float startDelay;
    [SerializeField] private float waitTime;
    [Header("Transform Points")]
    [SerializeField] Transform retractPoint;
    [SerializeField] Transform outPoint;

    private bool movingOut;
    private bool isWaiting;
    private float startTime;
    private float offsetTime;
    private Vector2 retractP;
    private Vector2 outP;

    private void Awake()
    {
        retractP = new Vector2(retractPoint.position.x, retractPoint.position.y);
        outP = new Vector2(outPoint.position.x, outPoint.position.y);
    }

    private void Start()
    {
        StartCoroutine(WaitTime(startDelay));
    }

    private void Update()
    {
        if (isWaiting) return;
        else
        {
            if (movingOut)
                MoveTowards();
            else
                MoveBack();
        }
    }

    private void MoveTowards()
    {
        if (new Vector2(crusherHead.position.x, crusherHead.position.y) != outP)
        {
            crusherHead.position = Vector2.MoveTowards(crusherHead.position, new Vector2(outP.x, outP.y), crushSpeed * Time.deltaTime);
        }
        else
        {
            movingOut = false;
        }
    }

    private void MoveBack()
    {
        if (new Vector2(crusherHead.position.x, crusherHead.position.y) != retractP)
        {
            crusherHead.position = Vector2.MoveTowards(crusherHead.position, new Vector2(retractP.x, retractP.y), crushSpeed * Time.deltaTime);
        }
        else
        {
            movingOut = true;
            StartCoroutine(WaitTime(waitTime));
        }
    }

    IEnumerator WaitTime(float wait)
    {
        isWaiting = true;
        yield return new WaitForSeconds(wait);
        isWaiting = false;
    }
}
