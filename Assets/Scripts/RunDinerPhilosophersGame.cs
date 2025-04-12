using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RunDinerPhilosophersGame : MonoBehaviour
{
    private const int NUM_PHILOSOPHERS = 5;
    private readonly Philosopher[] philosophers = new Philosopher[NUM_PHILOSOPHERS];
    private readonly Chopstick[] chopsticks = new Chopstick[NUM_PHILOSOPHERS];

    // Making a scrollable textArea in Unity is nowhere as straight forward
    // as JS/HTML. See:
    // https://www.youtube.com/watch?v=-4nkI9XnAU0
    public TMP_InputField consoleInputField; // Reference to the console text UI element

    // This method is called to start the simulation (from the Run button)
    public void BeginSimulation()
    {
        Debug.Log("Starting Diner Philosophers Game");
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
            philosophers[i] = new Philosopher(i, pickupChopsticks, dropChopsticks, this);
            StartCoroutine(philosophers[i].Run());
        }
        StartCoroutine(StopSimulationAfterSeconds(5f)); // Run for 5 seconds
    }

    private IEnumerator StopSimulationAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        for (int i = 0; i < NUM_PHILOSOPHERS; i++)
        {
            philosophers[i].Stop(); // Tell each philosopher to stop
        }
        Debug.Log("Simulation ended after 5 seconds.");
    }

    // Philosopher class
    public class Philosopher
    {
        private readonly int id;
        // The below lists will have 0, 1, or 2 chopstickIds (0,1,2,3,4) in them
        // the order of the list IS important
        public List<int> pickupChopsticks = new List<int>();
        public List<int> dropChopsticks = new List<int>();
        private readonly string[] philosopherNames = { "Tigress", "Monkey", "Viper", "Crane", "Mantis" };
        private readonly RunDinerPhilosophersGame runDinerPhilosophersGame;
        private bool keepRunning = true;

        // Pass MonoBehaviour reference (Test) to be able to call StartCoroutine()
        public Philosopher(int id, List<int> pickupChopsticks, List<int> dropChopsticks, RunDinerPhilosophersGame runDinerPhilosophersGame)
        {
            this.id = id;
            this.pickupChopsticks = pickupChopsticks;
            this.dropChopsticks = dropChopsticks;
            this.runDinerPhilosophersGame = runDinerPhilosophersGame;
        }

        public void Log(string message)
        {
            // Append the message to the console text
            runDinerPhilosophersGame.consoleInputField.text += message + "\n";
            Debug.Log(message); // Also log to the console for debugging
        }

        public void Stop()
        {
            keepRunning = false;
        }

        public IEnumerator Run()
        {
            while (keepRunning)
            {
                // Philosopher is thinking
                Log($"{philosopherNames[id]} is thinking.");
                // Simulate thinking time ... the philosopher thinks
                // twice as long as s/he eats
                yield return new WaitForSeconds(Random.Range(0.002f, 0.2f));
                // Philosopher is hungry and trying to pick up chopsticks
                Log($"{philosopherNames[id]} is hungry and trying to pick up chopsticks.");
                yield return runDinerPhilosophersGame.PickupChopsticks(this);
                // Philosopher is eating
                Log($"{philosopherNames[id]} is eating!");
                yield return new WaitForSeconds(Random.Range(0.001f, 0.1f));
                // Drop chopsticks
                yield return runDinerPhilosophersGame.DropChopsticks(this);
                Log($"{philosopherNames[id]} finished eating and is thinking again.");
            }
        }
    }


    // Chopstick class to simulate lock behavior
    public class Chopstick
    {
        private bool isHeld = false;
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
        for (int i = 0; i < philosopher.pickupChopsticks.Count; i++)
        {
            yield return StartCoroutine(chopsticks[philosopher.pickupChopsticks[i]].PickUp());

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
