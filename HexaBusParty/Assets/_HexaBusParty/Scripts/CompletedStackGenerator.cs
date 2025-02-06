using System;
using System.Collections.Generic;
using UnityEngine;

public class CompletedStackGenerator : MonoBehaviour
{
    public static CompletedStackGenerator Instance { get; private set; }

    [Header("Elements")] 
    [SerializeField] private List<Transform> positions;
    [SerializeField] private Hexagon hexagonPrefab;
    private  Transform freeStackPosition;
    private int stackSize;
    private List<Toping> availableTopings = new List<Toping>();
    [Header("Actions")] public static Action<List<Toping>, List<Transform>> onStackCompleted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void GenerateStack(Hexagon hexagon)
    {
        if(freeStackPosition == null) return;
        Vector3 hexagonLocalPos = Vector3.up * (stackSize * .2f);
        Vector3 spawnPosition = freeStackPosition.transform.TransformPoint(hexagonLocalPos);
        Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPosition, Quaternion.identity, freeStackPosition.transform);
        Vector3 originalScale = hexagonInstance.transform.localScale;
        hexagonInstance.transform.localScale = Vector3.zero;
        LeanTween.scale(hexagonInstance.gameObject, originalScale, .3f).setDelay(stackSize * .1f).setEase(LeanTweenType.easeOutBack);
        hexagonInstance.ToppingTexture = hexagon.ToppingTexture;
        stackSize++;
    }

    public void PickFreeStackPosition(Hexagon similarHexaong)
    {
        foreach (var position in positions)
        {
            if (position.childCount == 0)
            {
                freeStackPosition = position;
                SetAvailableTopings(similarHexaong.Toping);
                break;
            }
        }
        
    }


    public void ResetFreeStackPositions()
    {
        onStackCompleted?.Invoke(availableTopings,  positions);
        freeStackPosition = null;
        stackSize = 0;

    }

    public void SetAvailableTopings(Toping toping)
    {
        availableTopings.Add(toping);
    }
    
    public void RemoveAvailableTopings(Toping toping)
    {
        availableTopings.Remove(toping);
    }
}