# "High performance .NET by example"

We have a feature in Adform that identifies unwanted bot traffic. It’s backed by the Aho–Corasick algorithm, a string searching algorithm that matches all strings simultaneously. We will discover the algorithm and its current implementation. We will learn how to write micro benchmarks, profile code and read assembly code. We will improve performance by 20-30 times by different techniques: re-implement BCL data structures, fix CPU cache misses, reduce main memory reads by putting values in CPU registers by force, avoid calls to Method table, evaluate .NET Core (try SIMD?)


This workshop consists of two parts: basics and hardcore.
The first one is a workshop. We will learn how to write micro benchmarks, profile code using different profiles. 
We will learn how to read IL and assembly code for saner understanding how our code works.
We will find main bottlenecks and optimize performance significantly.

The second part will be a demonstration. I'll show you how we can use different tools and improve performance by tens times using different optimization techniques.

Slack: https://adform.slack.com/archives/workshop-highperf-net

### Prerequisites:
- Laptop (or a peer with a laptop) with dev environment 
- Aho-Corasick algorithm: study and try to implement it by yourself
- ILSpy
- WinDBG
- PerfView
- Intel VTune Amplifier

What it isn't about:
* .NET vs JVM vs C++ vs ...
* .NET is awesome!!!
* Business logic
* GC

What it is about:

* Pure performance

