using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private const int NUM_PHILOSOPHERS = 5;
    private readonly Philosopher[] philosophers = new Philosopher[NUM_PHILOSOPHERS];
    private readonly Chopstick[] chopsticks = new Chopstick[NUM_PHILOSOPHERS];

    // Start is called before the first frame update
    void Start()
    {
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
        private readonly Test test;  // Reference to Test class to call StartCoroutine

        // Pass MonoBehaviour reference (Test) to be able to call StartCoroutine()
        public Philosopher(int id, List<int> pickupChopsticks, List<int> dropChopsticks, Test test)
        {
            this.id = id;
            this.pickupChopsticks = pickupChopsticks;
            this.dropChopsticks = dropChopsticks;
            this.test = test;  // Store the reference to the 'Test' class
        }

        public IEnumerator Run()
        {
            while (true)
            {
                

                // Philosopher is hungry and trying to pick up chopsticks
                Debug.Log($"{philosopherNames[id]} is hungry and trying to pick up chopsticks.");
                yield return test.PickupChopsticks(this);

                // Philosopher is eating
                Debug.Log($"{philosopherNames[id]} is eating!");
                yield return new WaitForSeconds(Random.Range(0.001f, 0.1f));

                // Drop chopsticks
                yield return test.DropChopsticks(this);
                Debug.Log($"{philosopherNames[id]} finished eating and is thinking again.");
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
