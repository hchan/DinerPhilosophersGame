using System.Collections;
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
            philosophers[i] = new Philosopher(i, chopsticks[lower], chopsticks[higher], this);
            StartCoroutine(philosophers[i].Run());
        }
    }

    // Philosopher class
    public class Philosopher
    {
        private readonly int id;
        private readonly Chopstick leftChopstick;
        private readonly Chopstick rightChopstick;
        private readonly string[] philosopherNames = { "Tigress", "Monkey", "Viper", "Crane", "Mantis" };
        private readonly Test test;  // Reference to Test class to call StartCoroutine

        // Pass MonoBehaviour reference (Test) to be able to call StartCoroutine()
        public Philosopher(int id, Chopstick left, Chopstick right, Test test)
        {
            this.id = id;
            this.leftChopstick = left;
            this.rightChopstick = right;
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

        public Chopstick GetLeftChopstick() => leftChopstick;
        public Chopstick GetRightChopstick() => rightChopstick;
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
        // Try to pick up the left chopstick
        yield return StartCoroutine(philosopher.GetLeftChopstick().PickUp());

        // Try to pick up the right chopstick
        yield return StartCoroutine(philosopher.GetRightChopstick().PickUp());
    }

    public IEnumerator DropChopsticks(Philosopher philosopher)
    {
        // Drop the right chopstick
        yield return StartCoroutine(philosopher.GetRightChopstick().Drop());

        // Drop the left chopstick
        yield return StartCoroutine(philosopher.GetLeftChopstick().Drop());
    }
}
