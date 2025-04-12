using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RunDinerPhilosophersGame : MonoBehaviour
{
    private const int NUM_PHILOSOPHERS = 5;
    private readonly Philosopher[] philosophers = new Philosopher[NUM_PHILOSOPHERS];
    private readonly Chopstick[] chopsticks = new Chopstick[NUM_PHILOSOPHERS];

    public TMP_InputField consoleInputField; // Reference to the console text UI element

    // Start is called before the first frame update
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
    }

    // Philosopher class
    public class Philosopher
    {
        private readonly int id;


        public List<int> pickupChopsticks = new List<int>();
        public List<int> dropChopsticks = new List<int>();
        private readonly string[] philosopherNames = { "Tigress", "Monkey", "Viper", "Crane", "Mantis" };
        private readonly RunDinerPhilosophersGame runDinerPhilosophersGame;

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


        public IEnumerator Run()
        {
            while (true)
            {


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

    // Methods to control the philosopher actions in the Test class
    public IEnumerator PickupChopsticks(Philosopher philosopher)
    {
        for (int i = 0; i < philosopher.pickupChopsticks.Count; i++)
        {
            yield return StartCoroutine(chopsticks[philosopher.pickupChopsticks[i]].PickUp());

        }

    }

    public IEnumerator DropChopsticks(Philosopher philosopher)
    {
        for (int i = 0; i < philosopher.pickupChopsticks.Count; i++)
        {
            yield return StartCoroutine(chopsticks[philosopher.dropChopsticks[i]].Drop());

        }
    }
}
