#include <iostream>    // Input/output stream header file
#include <stdlib.h>    // Standard library header file
#include <omp.h>       // OpenMP header file
#include <time.h>      // Time-related header file
#include <string>      // String header file

using namespace std;  // Use standard namespace

#define NUM_THREADS 8  // Define the number of threads
#define MAX 1000000    // Define the maximum size of the array

int data[MAX];        // Declare an integer array of size MAX
long global_sum = 0;  // Declare a long integer variable to store the final sum

int main(int argc, char *argv[])
{
    int cores = omp_get_num_procs();           // Get the number of processors on the machine
    omp_set_num_threads(NUM_THREADS);          // Set the number of threads to be used in parallel execution
    cout << "The number of cores on this machine = " << cores << endl; // Print the number of cores on the machine

    // Generate random numbers and store them in the data array
    for (int i = 0; i < MAX; i++) {
        data[i] = rand() % 20;                  // Generate a random number between 0 and 19 and store it in the array
    }

    // Parallel region where each thread sums a portion of the data array
    #pragma omp parallel default(none) shared(data) private(i)
    {
        long tid, sum;                          // Declare thread id and local sum variables
        tid = omp_get_thread_num();             // Get the thread id
        sum = 0;                                // Initialize the local sum to zero

        int range = MAX / NUM_THREADS;          // Calculate the portion of the data array to be processed by each thread
        int start = tid * range;                // Calculate the starting index of the portion assigned to this thread
        int end = start + range;                // Calculate the ending index of the portion assigned to this thread

        // Loop through the portion of the data array assigned to this thread and sum its elements
        for (int i = start; i < end; i++) {
            sum += data[i];
        }

        // Use a critical section to update the global sum with the local sum calculated by this thread
        #pragma omp critical
        global_sum += sum;
    }

    // Print the final sum
    cout << "The final sum = " << global_sum << endl;

    return 0;
}
