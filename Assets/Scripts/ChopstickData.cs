using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopstickData {
    public List<int> availablePickupChopsticks = new List<int>();
    public List<int> orderPickupChopsticks = new List<int>();
    public List<int> availableDropChopsticks = new List<int>();
    public List<int> orderDropChopsticks = new List<int>();


    public ChopstickData(int leftChopstickId, int rightChopstickId) {
        availablePickupChopsticks.Add(leftChopstickId);
        availablePickupChopsticks.Add(rightChopstickId);
        availableDropChopsticks.Add(leftChopstickId);
        availableDropChopsticks.Add(rightChopstickId);
    }

    public void ConsumeAvailablePickupChopstick(int chopstickId)
    {
        availablePickupChopsticks.Remove(chopstickId);
        orderPickupChopsticks.Add(chopstickId);
    }

    public void ConsumeAvailableDropChopstick(int chopstickId)
    {
        availableDropChopsticks.Remove(chopstickId);
        orderDropChopsticks.Add(chopstickId);
    }

    public void ConsumeOrderPickupChopstick(int chopstickId)
    {
        orderPickupChopsticks.Remove(chopstickId);
        availablePickupChopsticks.Add(chopstickId);
    }

    public void ConsumeOrderDropChopstick(int chopstickId)
    {
        orderDropChopsticks.Remove(chopstickId);
        availableDropChopsticks.Add(chopstickId);
    }
}