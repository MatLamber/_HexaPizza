using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Client : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject orderPanel;
    [SerializeField] private GameObject[] orderIcons;
    [SerializeField] private GameObject orderHolder;
    [SerializeField] private AIPath agent;
    [Header("Settings")] 
    [SerializeField] private Toping order;
    [SerializeField] private Transform goal;
    private bool canFollow;
    

    public Toping Order => order;

    public void ShowOrderPanel()
    {
        for (int i = 0; i < orderIcons.Length; i++)
            orderIcons[i].SetActive(i == (int)order);
        orderPanel.transform.localScale = Vector3.zero;
        LeanTween.scale(orderPanel, Vector3.one, 0.3f).setEaseOutSine();


    }


    private void Update()
    {
        if(canFollow)
            agent.destination = goal.position;
    }

    public void HideOrderPanel()
    {
        LeanTween.scale(orderPanel, Vector3.zero, 0.3f).setEaseOutSine();
        canFollow = true;
    }

    public void SetOnOrderHolder(Transform pizza)
    {
        pizza.SetParent(orderHolder.transform);
        Vector3 hexagonLocalPos = Vector3.up * (pizza.GetSiblingIndex() * .2f);
        Vector3 spawnPosition = orderHolder.transform.TransformPoint(hexagonLocalPos);
        pizza.position = spawnPosition;
    }
    
}
