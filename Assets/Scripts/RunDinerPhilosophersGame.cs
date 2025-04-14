using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
/**
 * This script simulates the Diner Philosophers problem using Unity.
 * It creates a number of philosophers and chopsticks, and allows the philosophers to pick up and drop chopsticks while eating.
 * The simulation runs for a specified amount of time, and detects deadlocks.
 */
public class RunDinerPhilosophersGame : MonoBehaviour
{
    private const int NUM_PHILOSOPHERS = 5;
    public const int SIMULATION_TIME = 5; // seconds
    public static readonly string[] PHILOSOPHER_NAMES = { "Tigress", "Monkey", "Viper", "Crane", "Mantis" };

    private readonly Philosopher[] philosophers = new Philosopher[NUM_PHILOSOPHERS];
    private readonly Chopstick[] chopsticks = new Chopstick[NUM_PHILOSOPHERS];


    // Making a scrollable textArea in Unity is nowhere as straight forward
    // as JS/HTML. See:
    // https://www.youtube.com/watch?v=-4nkI9XnAU0
    public TMP_InputField consoleInputField; // Reference to the console text UI element
    public Button runButton; // Reference to the Run button

    public void Log(string message)
    {
        // Append the message to the console text
        consoleInputField.text += message + "\n";
        Debug.Log(message); // Also log to the console for debugging
    }

    // This method is called to start the simulation (from the Run button)


    public void BeginSimulation()
    {
        BeginSimulationHelper(i => AssignPickupAndDropChopsticksListsFromUserInput(i));
    }
    public void BeginSimulationGlobalOrderSolution()
    {
        BeginSimulationHelper(i => GlobalOrderSolution(i));
    }

    public void BeginSimulationPartialOrderSolution()
    {
        BeginSimulationHelper(i => PartialOrderSolution(i));
    }

    private void BeginSimulationHelper(Func<int, List<int>[]> pickupAndDropStrategy)
    {
        Debug.Log("Starting Diner Philosophers Game");
        DisableUIButtons();
        string currentTime = System.DateTime.Now.ToString("F"); // Get current date and time in human-readable format
        consoleInputField.text = "Started simulation at " + currentTime + "\n"; // Change button text
        // Create chopsticks
        for (int i = 0; i < NUM_PHILOSOPHERS; i++)
        {
            chopsticks[i] = new Chopstick();
        }

        // Initialize philosophers with left and right chopsticks and pass the MonoBehaviour reference
        for (int i = 0; i < NUM_PHILOSOPHERS; i++)
        {

            List<int>[] pickupAndDropLists = pickupAndDropStrategy(i);

            philosophers[i] = new Philosopher(i, pickupAndDropLists[0], pickupAndDropLists[1], this);

            // Start the philosopher's coroutine and add it to the list
            StartCoroutine(philosophers[i].Run());
        }
        StartCoroutine(DetectDeadlock(this, SIMULATION_TIME));
        StartCoroutine(StopSimulationAfterSeconds(this, SIMULATION_TIME));
    }

    // UI Related Methods
    private void DisableUIButtons()
    {
        runButton.interactable = false; // Disable the button to prevent multiple clicks
        foreach (string buttonName in GetChopstickButtonNamesInChopsticksHolder())
        {
            GameManager.Instance.Get(buttonName).GetComponent<Button>().interactable = false;
        }
    }

    private void EnableUIButtons()
    {
        runButton.interactable = true; // Re-enable the button      
        foreach (string buttonName in GetChopstickButtonNamesInChopsticksHolder())
        {
            GameManager.Instance.Get(buttonName).GetComponent<Button>().interactable = true;
        }
    }

    private string[] GetChopstickButtonNamesInChopsticksHolder()
    {
        return new string[] {
            "availableChopstickPickup0", "availableChopstickPickup1",
            "availableChopstickDrop0", "availableChopstickDrop1",
            "orderChopstickPickup0", "orderChopstickPickup1",
            "orderChopstickDrop0", "orderChopstickDrop1"
        };
    }
    // End UI Related Methods


    // This method is used to assign the pickup and drop chopsticks lists from the UI
    private List<int>[] AssignPickupAndDropChopsticksListsFromUserInput(int philosopherId)
    {
        return new List<int>[] { GameManager.Instance.chopstickData[philosopherId].orderPickupChopsticks, GameManager.Instance.chopstickData[philosopherId].orderDropChopsticks };
    }

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
        int right = (philosopherId + 1) % NUM_PHILOSOPHERS;
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
            first = (philosopherId + 1) % NUM_PHILOSOPHERS;
            second = philosopherId;
        }
        else
        {
            // odd philosopher
            first = philosopherId;
            second = (philosopherId + 1) % NUM_PHILOSOPHERS;
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



    private IEnumerator DetectDeadlock(RunDinerPhilosophersGame runDinerPhilosophersGame, float seconds)
    {
        while (IsSimulationComplete() == false)
        {
            // get all the heldPhilsopherIds in a Set
            // this is a set of the philosophers that are holding chopsticks
            // if the size of the set is equal to the number of philosophers
            // then there is a deadlock
            HashSet<int> heldPhilosopherIds = new HashSet<int>();
            for (int i = 0; i < NUM_PHILOSOPHERS; i++)
            {
                if (chopsticks[i].isHeldByPhilosopherId != -1)
                {
                    heldPhilosopherIds.Add(chopsticks[i].isHeldByPhilosopherId);
                }
            }
            if (heldPhilosopherIds.Count == NUM_PHILOSOPHERS)
            {
                runDinerPhilosophersGame.Log("Deadlock detected!  All philosophers are holding one chopstick");
                for (int i = 0; i < NUM_PHILOSOPHERS; i++)
                {
                    runDinerPhilosophersGame.Log($"Deadlock diagnostic: {PHILOSOPHER_NAMES[i]} is holding chopstick{i}");
                }
                break; // Exit the loop if deadlock is detected
            }
            yield return null;
        }
    }

    private IEnumerator StopSimulationAfterSeconds(RunDinerPhilosophersGame runDinerPhilosophersGame, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        for (int i = 0; i < NUM_PHILOSOPHERS; i++)
        {
            philosophers[i].Stop(); // Tell each philosopher to stop
            chopsticks[i].Stop(); // Release the chopstick
        }

        // Wait until all philosopher coroutines have finished
        yield return new WaitUntil(() => IsSimulationComplete());
        Summary(runDinerPhilosophersGame);
        runDinerPhilosophersGame.EnableUIButtons();

        ScrollRect scrollRect = GameManager.Instance.Get("console").GetComponent<ScrollRect>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        scrollRect.verticalNormalizedPosition = 0f; // Scroll to the bottom of the console
    }

    private void Summary(RunDinerPhilosophersGame runDinerPhilosophersGame)
    {

        runDinerPhilosophersGame.Log("--------------------------------------------------------------------");
        runDinerPhilosophersGame.Log($"Simulation ended after {SIMULATION_TIME} seconds.");
        runDinerPhilosophersGame.Log("--------------------------------------------------------------------");
        runDinerPhilosophersGame.Log("Summary of stir fry eaten:");
        bool allPhilsophersHaveEatenAtLeastTenBowls = true;
        for (int i = 0; i < NUM_PHILOSOPHERS; i++)
        {
            runDinerPhilosophersGame.Log($"{PHILOSOPHER_NAMES[i]} ate {philosophers[i].stirFryEaten} bowls of stir fry.");
            if (philosophers[i].stirFryEaten < 10)
            {
                allPhilsophersHaveEatenAtLeastTenBowls = false;
            }
        }
        if (allPhilsophersHaveEatenAtLeastTenBowls)
        {
            runDinerPhilosophersGame.Log("All philosophers have eaten at least 10 bowls of stir fry.  Simulation successful!  You win!");
        }
        else
        {
            runDinerPhilosophersGame.Log("Not all philosophers have eaten at least 10 bowls of stir fry.");
        }
    }

    // Check if all philosopher coroutines have completed

    private bool IsSimulationComplete()
    {
        foreach (Philosopher philosopher in philosophers)
        {
            if (!philosopher.IsDone) return false; // If any philosopher is still running, the simulation is not complete
        }
        return true;
    }

    // Philosopher class
    public class Philosopher
    {
        public readonly int id;
        // The below lists will have 0, 1, or 2 chopstickIds (0,1,2,3,4) in them
        // the order of the list IS important
        public List<int> pickupChopsticks = new List<int>();
        public List<int> dropChopsticks = new List<int>();
        private readonly RunDinerPhilosophersGame runDinerPhilosophersGame;
        public bool keepRunning = true;

        public int stirFryEaten = 0; // Number of stir fry eaten - let's assume the unit is a bowl
        private bool isDone = false;

        // Pass MonoBehaviour reference (Test) to be able to call StartCoroutine()
        public Philosopher(int id, List<int> pickupChopsticks, List<int> dropChopsticks, RunDinerPhilosophersGame runDinerPhilosophersGame)
        {
            this.id = id;
            this.pickupChopsticks = pickupChopsticks;
            this.dropChopsticks = dropChopsticks;
            this.runDinerPhilosophersGame = runDinerPhilosophersGame;
        }

        public void Stop()
        {
            keepRunning = false;
        }

        public bool IsDone => isDone; // Property to check if the philosopher is done

        public IEnumerator Run()
        {
            while (keepRunning)
            {
                // Philosopher is thinking
                runDinerPhilosophersGame.Log($"{PHILOSOPHER_NAMES[id]} is thinking.");
                yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.0001f));

                // Philosopher is hungry and trying to pick up chopsticks
                runDinerPhilosophersGame.Log($"{PHILOSOPHER_NAMES[id]} is hungry and trying to pick up chopsticks.");
                yield return runDinerPhilosophersGame.PickupChopsticks(this);
                if (!keepRunning) break; // Check if the philosopher is still running

                // Philosopher is eating
                runDinerPhilosophersGame.Log($"{PHILOSOPHER_NAMES[id]} is eating!");
                stirFryEaten++;
                yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.0001f));

                // Drop chopsticks
                yield return runDinerPhilosophersGame.DropChopsticks(this);
                runDinerPhilosophersGame.Log($"{PHILOSOPHER_NAMES[id]} has dropped his/her chopsticks.");

            }
            isDone = true; // Mark the philosopher as done when the loop finishes
        }
    }

    // Chopstick class to simulate lock behavior
    public class Chopstick
    {
        public bool keepRunning = true;

        // this can be used to simulate atomic-like behavior
        // also is reentrant
        public int isHeldByPhilosopherId = -1; // -1 means no philosopher is holding the chopstick


        // Try to pick up the chopstick (atomic-like behavior)
        public IEnumerator PickUp(int philosopherId)

        {
            // Wait until the chopstick is available
            // the isHeldByPhilosopherId != philosopherId implies reentrant
            while (isHeldByPhilosopherId != -1 && isHeldByPhilosopherId != philosopherId && keepRunning)
            {
                yield return null; // Wait for the next frame
            }
            // Mark the chopstick as picked up
            isHeldByPhilosopherId = philosopherId; // Set the philosopher ID who picked it up
        }

        // Drop the chopstick
        public IEnumerator Drop()
        {
            // Simulate dropping the chopstick
            isHeldByPhilosopherId = -1; // Reset the philosopher ID
            yield return null; // Done dropping
        }

        public void Stop()
        {
            keepRunning = false;
        }
    }

    public IEnumerator PickupChopsticks(Philosopher philosopher)
    {

        if (philosopher.pickupChopsticks.Count < 2)
        {
            Log($"Hint: Philosopher {PHILOSOPHER_NAMES[philosopher.id]} has {philosopher.pickupChopsticks.Count} chopstick(s) to pick up.");
            Log("In order to eat, a philosopher needs to pick up 2 chopsticks.");
        }

        int lockCount = 0; // you need 2 chopsticks to eat
        while (lockCount != 2 && philosopher.keepRunning)
        {
            for (int i = 0; i < philosopher.pickupChopsticks.Count && philosopher.keepRunning; i++)
            {
                yield return StartCoroutine(chopsticks[philosopher.pickupChopsticks[i]].PickUp(philosopher.id));
                lockCount++;
            }
            yield return null; // Wait for the next frame
        }
    }

    public IEnumerator DropChopsticks(Philosopher philosopher)
    {
        // Loop through each chopstick in the dropChopsticks list
        // note that this list is ordered containing the chopstickIds
        // and this list will have 0, 1, or 2 chopstickIds in it
        for (int i = 0; i < philosopher.dropChopsticks.Count && philosopher.keepRunning; i++)
        {
            yield return StartCoroutine(chopsticks[philosopher.dropChopsticks[i]].Drop());
        }
    }
}
