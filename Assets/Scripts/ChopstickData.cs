using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopstickData {
    public List<int> availablePickupChopsticks = new List<int>();
    public List<int> usedPickupChopsticks = new List<int>();
    public List<int> availableDropChopsticks = new List<int>();
    public List<int> usedDropChopsticks = new List<int>();


    public ChopstickData(int leftChopstickId, int rightChopstickId) {
        availablePickupChopsticks.Add(leftChopstickId);
        availablePickupChopsticks.Add(rightChopstickId);
        availableDropChopsticks.Add(leftChopstickId);
        availableDropChopsticks.Add(rightChopstickId);
    }
}