# Longest Path in a Graph – Project Documentation

## Overview

The project was created as part of a university assignment. The primary goal was to **find the longest path in a graph** from the first node to the last.

Beyond the assignment requirements, the problem was also used as an opportunity to get familiar with writing benchmarks and tests in C# using:

- Writing **benchmarks** using [BenchmarkDotNet](https://benchmarkdotnet.org/)
- Writing **unit tests** using [NUnit](https://nunit.org/)

---

## Project Structure

The main logic responsible for computing the longest path can be found in:

**./src/GraphProject**

The project also includes appropriate references to an external parser project:
**DotGraphFormatParser**

In **./tests** and **./benchmarks** there are project that was creating to train how to use NUNIT and BenchmarkDotNet

---

## How to Run

The compiled application must be run with **one** of the following parameters:

- An **absolute path** to a DOT file describing the graph.
- `-h` or `-help` to display usage instructions.

---

## DOT File Format

A **custom class** was implemented to parse DOT files into graph structures.  
The expected format is similar to the following:

<pre>
digraph [optionalName] {
nodes;
direction -> direction;
direction -> direction -> direction;
node { direction direction direction }
}
</pre>

## Example DOT File that will be parsed correctly

<pre>
digraph G {
0;
1;
2;
3;
4;
5;
0 -> {1 2};
1 -> 2 -> 3;
3 -> {4 1};
4 -> {5 2};
5 -> 3;
}
</pre>

This parser supports multiple edge declarations per line using both `->` syntax and curly braces `{}`.

---
