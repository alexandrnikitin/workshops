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



What it isn't about:
* .NET vs JVM vs C++
* .NET is awesome!!!
* GC



### Domain:
unwanted bot traffic



### Algorithm:
https://en.wikipedia.org/wiki/Aho%E2%80%93Corasick_algorithm
http://blog.ivank.net/aho-corasick-algorithm-in-as3.html
TODO: http://www.cs.uku.fi/~kilpelai/BSA05/lectures/slides04.pdf
TODO: https://www.quora.com/What-is-the-most-intuitive-explanation-of-the-Aho-Corasick-string-matching-algorithm
the only .NET implementation: https://www.informit.com/guides/content.aspx?g=dotnet&seqNum=769



### Harness:

#### Microbenchmarks:
https://github.com/dotnet/coreclr/blob/master/Documentation/project-docs/performance-guidelines.md#creating-a-microbenchmark

#### Profiling:
"Profilers Are Lying Hobbits (and we hate them!)" https://www.infoq.com/presentations/profilers-hotspots-bottlenecks
Perfview

#### IL & Assembly code:
ildasm & ILSpy
Windbg - the great and powerful
TODO: https://github.com/snare/voltron



### Optimizations:
#### Know APIs of libraries you use
#### Know BCL collections and data structures
#### Know advanced data structures



### Experiments
#### "All is Fair in Love and War"
#### .NET Core
#### SIMD
