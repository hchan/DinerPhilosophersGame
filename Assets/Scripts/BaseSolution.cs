using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
    This script provides two solutions to the Dining Philosophers problem:
    1. Global Order Solution: A global ordering of chopsticks to prevent deadlock.
    2. Partial Order Solution: A partial ordering of chopsticks to prevent deadlock.
    
    The script also includes methods to execute these solutions and update the game state accordingly.
*/
public class BaseSolution : MonoBehaviour
{
    public GameObject baseSolution;
    
    /** 
        Global ordering of chopsticks to solve the deadlock problem
        in the Dining Philosophers problem.
        Philosophers pick up the lower-numbered chopstick first, then the higher-numbered one.
        This global ordering ensures that no two philosophers will pick up the same chopstick
        simultaneously, preventing deadlock.
        
        Philosopher 0: picks chopsticks 0, then 1
        Philosopher 1: picks chopsticks 1, then 2
        Philosopher 2: picks chopsticks 2, then 3
        Philosopher 3: picks chopsticks 3, then 4
        Philosopher 4: picks chopsticks 4, then 0
    */
    public List<int>[] GlobalOrderSolution(int philosopherId)
    {
        int left = philosopherId;
        int right = (philosopherId + 1) % RunDinerPhilosophersGame.NUM_PHILOSOPHERS;
        int lower = Mathf.Min(left, right); // partial ordering
        int higher = Mathf.Max(left, right); // partial ordering
        List<int> pickupChopsticks = new List<int>
            {
                lower,
                higher
            };
        // any order would work for dropping the chopsticks
        // but LIFO (Last In First Out) is the best practice
        List<int> dropChopsticks = new List<int>
            {
                higher,
                lower
            };
        return new List<int>[] { pickupChopsticks, dropChopsticks };
    }

    /** 
        Partial ordering of chopsticks to solve the deadlock problem
        in the Dining Philosophers problem.
        Philosophers pick up chopsticks in a specific order, but the order is not global.
        Even-numbered philosophers pick chopsticks in a different order than odd-numbered ones.
        This approach creates a local partial ordering, preventing deadlock by ensuring that no two philosophers
        will pick up the same chopstick at the same time.
        
        Philosopher 0: picks chopsticks 1, then 0
        Philosopher 1: picks chopsticks 1, then 2
        Philosopher 2: picks chopsticks 3, then 2
        Philosopher 3: picks chopsticks 3, then 4
        Philosopher 4: picks chopsticks 0, then 4
    */
    public List<int>[] PartialOrderSolution(int philosopherId)
    {
        int first;
        int second;
        if (philosopherId % 2 == 0) // odd
        {
            // even philosopher
            first = (philosopherId + 1) % RunDinerPhilosophersGame.NUM_PHILOSOPHERS;
            second = philosopherId;
        }
        else
        {
            // odd philosopher
            first = philosopherId;
            second = (philosopherId + 1) % RunDinerPhilosophersGame.NUM_PHILOSOPHERS;
        }
        List<int> pickupChopsticks = new List<int>
            {
                first,
                second
            };
        // any order would work for dropping the chopsticks
        // but LIFO (Last In First Out) is the best practice
        List<int> dropChopsticks = new List<int>
            {
                second,
                first
            };
        return new List<int>[] { pickupChopsticks, dropChopsticks };

    }


    public void GlobalOrderSolution()
    {
        ExecuteSolution((runDinerPhilosophersGame, i) => GlobalOrderSolution(i));
    }

    public void PartialOrderSolution()
    {
        ExecuteSolution((runDinerPhilosophersGame, i) => PartialOrderSolution(i));
    }

    /**
        Execute the solution strategy for each philosopher.
        This method sets up the game state by updating the available and ordered chopsticks for each philosopher.
        It then invokes the run button click event to start the game with the specified solution strategy.
    */
    private void ExecuteSolution(System.Func<RunDinerPhilosophersGame, int, List<int>[]> solutionStrategy)
    {
        GameManager.Instance.Get("TabManager").GetComponent<TabManager>().ShowTab("diningTable");
        RunDinerPhilosophersGame runDinerPhilosophersGame = GameManager.Instance.Get("runDinerPhilosophersGame").GetComponent<RunDinerPhilosophersGame>();
        for (int i = 0; i < RunDinerPhilosophersGame.NUM_PHILOSOPHERS; i++)
        {
            GameManager.Instance.chopstickData[i].availablePickupChopsticks.Clear();
            GameManager.Instance.chopstickData[i].orderPickupChopsticks.Clear();
            GameManager.Instance.chopstickData[i].availableDropChopsticks.Clear();
            GameManager.Instance.chopstickData[i].orderDropChopsticks.Clear();

            List<int>[] pickupAndDropLists = solutionStrategy(runDinerPhilosophersGame, i);
            GameManager.Instance.chopstickData[i].orderPickupChopsticks.AddRange(pickupAndDropLists[0]);
            GameManager.Instance.chopstickData[i].orderDropChopsticks.AddRange(pickupAndDropLists[1]);            
        }
        GameManager.Instance.Get("runButton").GetComponent<Button>().onClick.Invoke(); // Invoke the click event of the run button ... this includes ALL listeners
    }
}
