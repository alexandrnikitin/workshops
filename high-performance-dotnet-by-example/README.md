# "High performance .NET by example"

In progress...

We have a feature in Adform that identifies unwanted bot traffic. It’s backed by the Aho–Corasick algorithm, a string searching algorithm that matches all strings simultaneously. We will discover the algorithm and its current implementation. We will learn how to write micro benchmarks, profile code and read assembly code. We will improve performance by 20-30 times by different techniques: re-implement BCL data structures, fix CPU cache misses, reduce main memory reads by putting values in CPU registers by force, avoid calls to Method table, evaluate .NET Core (try SIMD?)



0. Intro, what it's about and what isn't, agenda
0. Domain field
0. Intro into Aho-Corasick algorithm
0. Implement the algorithm by ourselves and the current one
0. How to write micro-benchmarks
0. How to profile code
0. How to get and read assembly code
0. Re-implement BCL
0. Open address hashset
0. Experiment: static
0. Experiment: .NET Core
0. Experiment: SIMD
