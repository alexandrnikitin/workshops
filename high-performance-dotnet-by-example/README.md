# "High performance .NET by example"

In progress...

We have a feature in Adform that identifies unwanted bot traffic. It’s backed by the Aho–Corasick algorithm, a string searching algorithm that matches all strings simultaneously. We will discover the algorithm and its current implementation. We will learn how to write micro benchmarks, profile code and read assembly code. We will improve performance by 20-30 times by different techniques: re-implement BCL data structures, fix CPU cache misses, reduce main memory reads by putting values in CPU registers by force, avoid calls to Method table, evaluate .NET Core (try SIMD?)



0. Intro
  * What it's about and what isn't, agenda
  * Domain field
0. About the Aho-Corasick algorithm
  * The algorithm explained
  * Implement the algorithm by ourselves
  * The current implementation
0. Harness
  * How to write micro-benchmarks
  * How to profile code
  * How to get and read assembly code
0. Optimizations:
  * Re-implement BCL
  * Open address hashset
  * Further micro optimizations
0. Experiments
  * Static
  * .NET Core
  * SIMD?
