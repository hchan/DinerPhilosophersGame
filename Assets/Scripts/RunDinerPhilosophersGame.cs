using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RunDinerPhilosophersGame : MonoBehaviour
{
    private const int NUM_PHILOSOPHERS = 5;
    private const int SIMULATION_TIME = 1; // seconds
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

        // Create chopsticks
        for (int i = 0; i < NUM_PHILOSOPHERS; i++)
        {
            chopsticks[i] = new Chopstick();
        }

        // Initialize philosophers with left and right chopsticks and pass the MonoBehaviour reference
        for (int i = 0; i < NUM_PHILOSOPHERS; i++)
        {
            
            int left = i;
            int right = (i + 1) % NUM_PHILOSOPHERS;
            int lower = Mathf.Min(left, right); // partial ordering
            int higher = Mathf.Max(left, right); // partial ordering
            List<int> pickupChopsticks = new List<int>
            {
                lower,
                higher
            };
            List<int> dropChopsticks = new List<int>
            {
                higher,
                lower
            };
            //List<int> pickupChopsticks = GameManager.Instance.chopstickData[i].orderPickupChopsticks;
            //List<int> dropChopsticks = GameManager.Instance.chopstickData[i].orderDropChopsticks;
            philosophers[i] = new Philosopher(i, pickupChopsticks, dropChopsticks, this);

            // Start the philosopher's coroutine and add it to the list
            StartCoroutine(philosophers[i].Run());
        }

        StartCoroutine(StopSimulationAfterSeconds(this, SIMULATION_TIME));
    }

    private IEnumerator StopSimulationAfterSeconds(RunDinerPhilosophersGame runDinerPhilosophersGame, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        for (int i = 0; i < NUM_PHILOSOPHERS; i++)
        {
            philosophers[i].Stop(); // Tell each philosopher to stop
        }

        // Wait until all philosopher coroutines have finished
        yield return new WaitUntil(() => IsSimulationComplete());

        runDinerPhilosophersGame.Log($"Simulation ended after {SIMULATION_TIME} seconds.");
        runButton.interactable = true; // Re-enable the button      
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
        private readonly string[] philosopherNames = { "Tigress", "Monkey", "Viper", "Crane", "Mantis" };
        private readonly RunDinerPhilosophersGame runDinerPhilosophersGame;
        private bool keepRunning = true;
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
                runDinerPhilosophersGame.Log($"{philosopherNames[id]} is thinking.");
                yield return new WaitForSeconds(Random.Range(0.002f, 0.2f));

                // Philosopher is hungry and trying to pick up chopsticks
                runDinerPhilosophersGame.Log($"{philosopherNames[id]} is hungry and trying to pick up chopsticks.");
                yield return runDinerPhilosophersGame.PickupChopsticks(this);

                // Philosopher is eating
                runDinerPhilosophersGame.Log($"{philosopherNames[id]} is eating!");
                yield return new WaitForSeconds(Random.Range(0.001f, 0.1f));

                // Drop chopsticks
                yield return runDinerPhilosophersGame.DropChopsticks(this);
                runDinerPhilosophersGame.Log($"{philosopherNames[id]} finished eating and is thinking again.");
            }
            isDone = true; // Mark the philosopher as done when the loop finishes
        }
    }

    // Chopstick class to simulate lock behavior
    public class Chopstick
    {
        public bool isHeld = false;

        // Try to pick up the chopstick (atomic-like behavior)
        public IEnumerator PickUp()
        {
            // Wait until the chopstick is available
            while (isHeld)
            {
                yield return null; // Wait for the next frame
            }
            // Mark the chopstick as picked up
            isHeld = true;
        }

        // Drop the chopstick
        public IEnumerator Drop()
        {
            // Simulate dropping the chopstick
            isHeld = false;
            yield return null; // Done dropping
        }
    }

    public IEnumerator PickupChopsticks(Philosopher philosopher)
    {
        // Loop through each chopstick in the pickupChopsticks list
        // note that this list is ordered containing the chopstickIds
        // and this list will have 0, 1, or 2 chopstickIds in it
        int leftChopstickId = GameManager.Instance.GetLeftChopstickId(philosopher.id);
        int rightChopstickId = GameManager.Instance.GetRightChopstickId(philosopher.id);
        while (!chopsticks[leftChopstickId].isHeld && !chopsticks[rightChopstickId].isHeld)
        {
            for (int i = 0; i < philosopher.pickupChopsticks.Count; i++)
            {
                yield return StartCoroutine(chopsticks[philosopher.pickupChopsticks[i]].PickUp());
            }
            yield return null; // Wait for the next frame
        }
    }

    public IEnumerator DropChopsticks(Philosopher philosopher)
    {
        // Loop through each chopstick in the dropChopsticks list
        // note that this list is ordered containing the chopstickIds
        // and this list will have 0, 1, or 2 chopstickIds in it
        for (int i = 0; i < philosopher.dropChopsticks.Count; i++)
        {
            yield return StartCoroutine(chopsticks[philosopher.dropChopsticks[i]].Drop());
        }        
    }
}
