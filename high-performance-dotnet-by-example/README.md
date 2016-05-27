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
  * Branchless code
  * Further micro optimizations
0. Experiments
  * Static
  * .NET Core
  * SIMD?


### Prerequisites:
- Study the Aho-Corasick algorithm and try to implement it by yourself
- ildasm, ILSpy
- WinDBG[optional]
- PerfView
- Intel VTune Amplifier [optional]

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

is hard!

Pipeline:
A task -> C#
C# + Compiler -> IL
IL + BCL + 3rdParty -> App
App + CLR -> ASM
ASM + CPU -> Result

Infrastructure:
OS: Windows, Linux, OS X
CLR: CLR2, CLR4, CoreCLR, Mono
GC: MS, Boehm, Sgen
JIT: Legacy x86 & x64, RyuJIT
Compilation: JIT, NGen, MPGO, .NET Native

DOs & DON'Ts from Microsoft:

- **DO** use a microbenchmark when you have an isolated piece of code whose performance you want to analyze.
- **DO NOT** use a microbenchmark for code that has non-deterministic dependences (e.g. network calls, file I/O etc.)
- **DO** run all performance testing against retail optimized builds.
- **DO** run many iterations of the code in question to filter out noise.
- **DO** minimize the effects of other applications on the performance of the microbenchmark by closing as many unnecessary applications as possible.

https://github.com/dotnet/coreclr/blob/master/Documentation/project-docs/performance-guidelines.md#creating-a-microbenchmark

##### BenchmarkDotNet:
- A sample benchmark
- Config
- Diagnosers

How does it work: https://github.com/PerfDotNet/BenchmarkDotNet#how-it-works

- Isolated project based on templates
- MethodInvoker: Pilot, Idle, Warmup, Target, Clocks
- Generated project
- Results: R plot

Tasks:
x86 vs x64
RuyJIT vs LegacyJit

#### Profiling:
"Profilers Are Lying Hobbits (and we hate them!)" https://www.infoq.com/presentations/profilers-hotspots-bottlenecks

Sandbox console app


##### Perfview
Swiss army knife
Tutorial
Videos https://channel9.msdn.com/Series/PerfView-Tutorial
Time based
Memory
ETW events
CMD args: https://github.com/lowleveldesign/debug-recipes/blob/master/perfview/perfview-cmdline.txt

#### IL & Assembly code:
ildasm & ILSpy

Visual Studio

Windbg - the great and powerful
SOS Sun of Strike
SOSex
HOWTO: Debugging .NET with WinDbg https://docs.google.com/document/d/1yMQ8NAQZEBtsfVp7AsFLSA_MkIKlYNuSowG72_nU0ek

WinDbgCs https://github.com/southpolenator/WinDbgCs
CLRMD https://github.com/Microsoft/clrmd/blob/master/Documentation/MachineCode.md

TODO: https://github.com/snare/voltron



### Optimizations:
#### Lesson 1: Know APIs of libraries you use!


#### Lesson 2: Know BCL collections and data structures
GC modes: Server vs Workstation (BenchmarkDotNet?) CPU groups?
Try Server GC: less GCs

Profilers are lying hobbits!!!
Everybody lies!!

box struct task?


#### Lesson 3: Know basic data structures
BCL is too generic and isn't suitable for high performance


#### Lesson 4: Know overheads

Basic hotspots
Memory access

Memory writes:
By Ref

Memory reads
```
if (pointer.Results.Count > 0)
```
```
mov rax, qword ptr [rsp+0x28]
mov rax, qword ptr [rax+0x10]
cmp dword ptr [rax+0x18], 0x0
jle 0x7ffcbc4238a1
```


#### Lesson 4: Know how CPU works

CPU: Front-End & Back-End
CPU cache

Intel i7-4770 (Haswell), 3.4 GHz

Sizes:
L1 Data cache = 32 KB
L1 Instruction cache = 32 KB
L2 cache = 256 KB
L3 cache = 8 MB

Latency:
L1 Data Cache Latency = 4 cycles for simple access via pointer
L1 Data Cache Latency = 5 cycles for access with complex address calculation
L2 Cache Latency = 12 cycles
L3 Cache Latency = 36 cycles
RAM Latency = 36 cycles + 57 ns

Source: http://www.7-cpu.com/cpu/Haswell.html


FLOPs per cycle: http://stackoverflow.com/questions/8389648/how-do-i-achieve-the-theoretical-maximum-of-4-flops-per-cycle
Branch prediction


#### Lesson 5: Know advanced data structures
Intel VTune Amplifier - heavy metal of profilers


Classic hashset -> open address hashset

Memory exploration



#### Lesson 6: Know hacks

MOD is expensive


#### Lesson 7: Loop unrolling


#### Lesson: Going unsafe


### Experiments
#### "All is Fair in Love and War"
#### .NET Core
#### SIMD



### Further reads
Pro .NET Performance by Sasha Goldshtein , Dima Zurbalev , Ido Flatow
Writing High-Performance .NET Code by Ben Watson
A fundamental introduction to x86 assembly programming https://www.nayuki.io/page/a-fundamental-introduction-to-x86-assembly-programming
