using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<Client> clients;

    private Toping currentOrder;
    
    private List<Toping> ordersAvailable = new List<Toping>();
    private List<Transform> orderContainers = new List<Transform>();

    private void OnEnable()
    {
        CompletedStackGenerator.onStackCompleted += CheckAvailableOrders;
    }

    private void OnDisable()
    {
        CompletedStackGenerator.onStackCompleted -= CheckAvailableOrders;
    }

    private void Start()
    {
        MakeClientOrder();
    }

    public void MakeClientOrder()
    {
        if (clients.Count == 0) return;
        clients[0].ShowOrderPanel();
        currentOrder = clients[0].Order;
        StartCoroutine(CheckForAvailableOrders());

    }

    IEnumerator CheckForAvailableOrders()
    {
        yield return new WaitForSeconds(0.7f);
        CheckAvailableOrders(ordersAvailable, orderContainers);
    }

    public void CheckAvailableOrders(List<Toping> orders, List<Transform> orderContainer)
    {
        if(orders.Count == 0) return;
        ordersAvailable = orders;
        orderContainers = orderContainer;
        int matchingIndex = orders.IndexOf(currentOrder);
        if (matchingIndex != -1)
        {
            clients[0].HideOrderPanel();
            Transform clientTransform = clients[0].transform;
            List<Transform> pizzas = new List<Transform>();
            for (int i = 0; i < orderContainer.Count; i++)
            {
                if (i == matchingIndex)
                {
                    for (int j = 0; j < orderContainer[i].childCount; j++)
                    {
                        pizzas.Add(orderContainer[i].GetChild(j));
                    }
                }
                    
            }
            foreach (Transform pizza in pizzas)
            {
                clients[0].SetOnOrderHolder(pizza);
            }
            CompletedStackGenerator.Instance.RemoveAvailableTopings(currentOrder);
            ordersAvailable.Remove(currentOrder);
            clients[0].GetComponent<ClientAnimationManager>().Walk();
            clients[0].GetComponent<ClientAnimationManager>().EnableUpperBodyLayer();
            clients.RemoveAt(0);
            Invoke(nameof(AdvanceClientLine),0.5f);
            Debug.Log($"Order matched at index: {matchingIndex}");
        }
        else
        {
            Debug.Log("No matching order found.");
        }

    }

    public void AdvanceClientLine()
    {
        foreach (Client client in clients)
        {
            LeanTween.moveZ(client.gameObject, client.transform.position.z - 2, 0.3f);
            client.GetComponent<ClientAnimationManager>().WalkAndStop();
        }
        
        MakeClientOrder();
    }
}
