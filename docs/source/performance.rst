.. include:: styles.txt

.. |benchmark| raw:: html

    <a href="http://benchmarkdotnet.org" target="_blank">BenchmarkDotNet</a>

Performance
===========

The benchmarking is performed using the powerful library |benchmark|.

The tests was runned against a handwritten implementation of the object-object mappings and against the libraries:

- AutoMapper 6.2.2 *(probably the most popular one)*
- Mapster 3.1.8 *(one of the fastest mappers)*
- A handwritten implementation which is used as the base-line of all tests.

The benchmark test project is also available at |github|.

Benchmarking Results
--------------------

Here is just a short resume of all summary results. You can see the full log running the benchmark project.

.. code::

    BenchmarkDotNet=v0.10.12, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.248)
    Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical cores and 4 physical cores

    Frequency=3515641 Hz, Resolution=284.4431 ns, Timer=TSC


Basic Test
^^^^^^^^^^

.. code::

    ShortRun : .NET Core 2.0.5 (Framework 4.6.26020.03), 64bit RyuJIT | .NET Core SDK=2.1.4

+----------------+----------+--------+------+-----------+
|     Method     |   Mean   | Scaled | Rank | Allocated |
+================+==========+========+======+===========+
| HandwrittenMap | 237.5 ns | 1.00   | I    | 512 B     |
+----------------+----------+--------+------+-----------+
| MapsterMap     | 309.8 ns | 1.30   | II   | 528 B     |
+----------------+----------+--------+------+-----------+
| AutoMapperMap  | 429.1 ns | 1.81   | III  | 544 B     |
+----------------+----------+--------+------+-----------+
| OutputMap      | 609.5 ns | 2.57   | IV   | 624 B     |
+----------------+----------+--------+------+-----------+


.. code::

    ShortRun : .NET Framework 4.6.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2633.0

+----------------+------------+--------+------+-----------+
|     Method     |    Mean    | Scaled | Rank | Allocated |
+================+============+========+======+===========+
| MapsterMap     | 684.9 ns   | 0.83   | I    | 528 B     |
+----------------+------------+--------+------+-----------+
| HandwrittenMap | 829.9 ns   | 1.00   | II   | 608 B     |
+----------------+------------+--------+------+-----------+
| OutputMap      | 1,056.9 ns | 1.27   | III  | 624 B     |
+----------------+------------+--------+------+-----------+
| AutoMapperMap  | 1,418.3 ns | 1.71   | IV   | 640 B     |
+----------------+------------+--------+------+-----------+

Complex Test
^^^^^^^^^^^^

.. code::

    ShortRun : .NET Core 2.0.5 (Framework 4.6.26020.03), 64bit RyuJIT | .NET Core SDK=2.1.4

+----------------+-------------+--------+------+-----------+
|     Method     |    Mean     | Scaled | Rank | Allocated |
+================+=============+========+======+===========+
| HandwrittenMap | 620.5 ns    | 1.00   | I    | 1.36 KB   |
+----------------+-------------+--------+------+-----------+
| MapsterMap     | 1,194.9 ns  | 1.93   | II   | 1.86 KB   |
+----------------+-------------+--------+------+-----------+
| OutputMap      | 2,640.6 ns  | 4.26   | III  | 1.9 KB    |
+----------------+-------------+--------+------+-----------+
| AutoMapperMap  | 48,040.1 ns | 77.43  | IV   | 16.53 KB  |
+----------------+-------------+--------+------+-----------+


.. code::

    ShortRun : .NET Framework 4.6.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2633.0
    
+----------------+-------------+--------+------+-----------+
|     Method     |    Mean     | Scaled | Rank | Allocated |
+================+=============+========+======+===========+
| HandwrittenMap | 771.8 ns    | 1.00   | I    | 1.59 KB   |
+----------------+-------------+--------+------+-----------+
| MapsterMap     | 1,996.4 ns  | 2.59   | II   | 1.86 KB   |
+----------------+-------------+--------+------+-----------+
| OutputMap      | 3,468.6 ns  | 4.49   | III  | 1.9 KB    |
+----------------+-------------+--------+------+-----------+
| AutoMapperMap  | 44,083.8 ns | 57.12  | IV   | 17.15 KB  |
+----------------+-------------+--------+------+-----------+

Intense Test
^^^^^^^^^^^^

.. code::

    ShortRun : .NET Core 2.0.5 (Framework 4.6.26020.03), 64bit RyuJIT | .NET Core SDK=2.1.4
    
+----------------+------------+--------+------+-----------+
|     Method     |    Mean    | Scaled | Rank | Allocated |
+================+============+========+======+===========+
| HandwrittenMap | 2.910 us   | 1.00   | I    | 4.98 KB   |
+----------------+------------+--------+------+-----------+
| MapsterMap     | 6.419 us   | 2.21   | II   | 6.32 KB   |
+----------------+------------+--------+------+-----------+
| OutputMap      | 9.410 us   | 3.23   | III  | 6.68 KB   |
+----------------+------------+--------+------+-----------+
| AutoMapperMap  | 141.356 us | 48.59  | IV   | 47.54 KB  |
+----------------+------------+--------+------+-----------+

.. code::

    ShortRun : .NET Framework 4.6.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2633.0

+----------------+------------+--------+------+-----------+
|     Method     |    Mean    | Scaled | Rank | Allocated |
+================+============+========+======+===========+
| HandwrittenMap | 4.243 us   | 1.00   | I    | 6.05 KB   |
+----------------+------------+--------+------+-----------+
| MapsterMap     | 9.133 us   | 2.15   | II   | 6.34 KB   |
+----------------+------------+--------+------+-----------+
| OutputMap      | 13.505 us  | 3.18   | III  | 6.68 KB   |
+----------------+------------+--------+------+-----------+
| AutoMapperMap  | 133.597 us | 31.49  | IV   | 49.43 KB  |
+----------------+------------+--------+------+-----------+


Legends:

- Mean      : Arithmetic mean of all measurements
- Scaled    : Mean(CurrentBenchmark) / Mean(BaselineBenchmark)
- Rank      : Relative position of current benchmark mean among all benchmarks (Roman style)
- Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
- 1 ns      : 1 Nanosecond (0.000000001 sec)