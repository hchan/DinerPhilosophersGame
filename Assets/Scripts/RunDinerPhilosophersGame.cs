using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        Debug.Log("Starting Diner Philosophers Game");
        runButton.interactable = false; // Disable the button to prevent multiple clicks
        string currentTime = System.DateTime.Now.ToString("F"); // Get current date and time in human-readable format
        consoleInputField.text = "Running at " + currentTime + "\n"; // Change button text
        // Create chopsticks
        for (int i = 0; i < NUM_PHILOSOPHERS; i++)
        {
            chopsticks[i] = new Chopstick();
        }

        // Initialize philosophers with left and right chopsticks and pass the MonoBehaviour reference
        for (int i = 0; i < NUM_PHILOSOPHERS; i++)
        {
            List<int> pickupChopsticks, dropChopsticks;
            PartialOrderSoln(i, out pickupChopsticks, out dropChopsticks);
            //AssignPickupAndDropChopsticksListsFromUserInput(i, out pickupChopsticks, out dropChopsticks);
            //AssignPickupAndDropChopsticksListsFromLocalTesting(i, out pickupChopsticks, out dropChopsticks);
            philosophers[i] = new Philosopher(i, pickupChopsticks, dropChopsticks, this);

            // Start the philosopher's coroutine and add it to the list
            StartCoroutine(philosophers[i].Run());
        }

        StartCoroutine(StopSimulationAfterSeconds(this, SIMULATION_TIME));
    }

    private static void AssignPickupAndDropChopsticksListsFromLocalTesting(int i, out List<int> pickupChopsticks, out List<int> dropChopsticks)
    {
        pickupChopsticks = new List<int>
            {
                i,
                (i + 1) % NUM_PHILOSOPHERS
            };
        dropChopsticks = new List<int>
            {
                (i + 1) % NUM_PHILOSOPHERS,
                i
            };
    }
    private static void AssignPickupAndDropChopsticksListsFromUserInput(int i, out List<int> pickupChopsticks, out List<int> dropChopsticks)
    {
        pickupChopsticks = GameManager.Instance.chopstickData[i].orderPickupChopsticks;
        dropChopsticks = GameManager.Instance.chopstickData[i].orderDropChopsticks;
    }

    // Partial ordering of chopsticks
    // This is a solution to the deadlock problem
    // in the Diner Philosopher's problem.
    // The idea is to have a partial ordering of the chopsticks
    // so that a philosopher will pick up the lower numbered
    // chopstick first and then the higher numbered chopstick.
    // This way, no two philosophers will pick up the same
    // chopstick at the same time and there will be no deadlock.
    // For example, if there are 5 philosophers and 5 chopsticks,
    // the philosophers will pick up the chopsticks in the following order:
    // Philosopher 0: 0, 1
    // Philosopher 1: 1, 2
    // Philosopher 2: 2, 3
    // Philosopher 3: 3, 4
    // Philosopher 4: 4, 0
    // This way, no two philosophers will pick up the same
    // chopstick at the same time and there will be no deadlock.
    // Note that using an even/odd ordering of the chopsticks works too
    // exmple:
    // if (i % 2 == 0)
    // {
    //   left = (i + 1) % NUM_PHILOSOPHERS;
    //   right = i;
    // }
    private static void PartialOrderSoln(int i, out List<int> pickupChopsticks, out List<int> dropChopsticks)
    {
        int left = i;
        int right = (i + 1) % NUM_PHILOSOPHERS;
        int lower = Mathf.Min(left, right); // partial ordering
        int higher = Mathf.Max(left, right); // partial ordering
        pickupChopsticks = new List<int>
            {
                lower,
                higher
            };
        dropChopsticks = new List<int>
            {
                higher,
                lower
            };
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
        runButton.interactable = true; // Re-enable the button      

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
                yield return new WaitForSeconds(Random.Range(0f, 0.0001f));

                // Philosopher is hungry and trying to pick up chopsticks
                runDinerPhilosophersGame.Log($"{PHILOSOPHER_NAMES[id]} is hungry and trying to pick up chopsticks.");
                yield return runDinerPhilosophersGame.PickupChopsticks(this);
                if (!keepRunning) break; // Check if the philosopher is still running

                // Philosopher is eating
                runDinerPhilosophersGame.Log($"{PHILOSOPHER_NAMES[id]} is eating!");
                stirFryEaten++;
                yield return new WaitForSeconds(Random.Range(0f, 0.0001f));

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
        //while (philosopher.keepRunning && (!chopsticks[leftChopstickId].isHeld || !chopsticks[rightChopstickId].isHeld))
        //{
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
