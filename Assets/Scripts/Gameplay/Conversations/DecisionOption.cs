using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecisionOption
{
    public string Text;
    public DecisionAction Action;
}

public enum DecisionAction
{
    Cancel,
    GoToRabbitArea,
    GoToLionArea,
    GoToMonkeyArea,
    GoToElephantArea,
    GoToZooDirectorRoom,
    GoToZooGate,
    GoToAquariumInside,
    GoToAquariumOutside,
    GoToGuestRoom,
    GoToWhaleArea,
    Escape,
    TearMap,
    FeedMapCorner,
    GoLeft,
    GoRight,
    // Add other actions as needed
}
