# Monty Hall LINQ
The purpose of this repository is to showcase a way of expressing the 
[Monty Hall problem](https://en.wikipedia.org/wiki/Monty_Hall_problem) in a 
way that closely resembles a natural language description of the problem. 
That is, we would like to be able to write something like the query 
expression below, which would compute the probability of winning when 
switching doors:

```C#
from doorWithPrize in OneOf(doors)
from doorPickedByContestant in OneOf(doors)
let doorsQuizMasterCouldOpen = doors.Without(doorWithPrize, doorPickedByContestant)
from doorOpenedByQuizMaster in OneOf(doorsQuizMasterCouldOpen)
let doorContestantCouldSwithTo = doors.Without(doorPickedByContestant, doorOpenedByQuizMaster).First()
select doorContestantCouldSwithTo == doorWithPrize
```


## How to read the example query
If one is not familiar with LINQ queries or only has seen it used with 
[IEnumerable\<T\>](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1),
then the query might be hard to read or understand. The example is also missing
things like the definitions of `OneOf(...)` and `.Without(...)`.

The first step to understanding the example query is knowing what each part in
the `from` clause represents. In this example, we interpret each `from` clause
to define an *outcome* of a 
*[random variable](https://en.wikipedia.org/wiki/Random_variable)*.
That is, the clause 

> `... from x in X ...` 

could be read as:

> Let *x* be an outcome of the random variable *X*.


The second step is understanding the `let` clause. The `let` clause allows us 
to store the result of a sub-expression in a variable. In the example query,
`let` is used to give a name to sub-expressions in order to make the query 
easier to read. For example, instead of
```C#
...
let doorsQuizMasterCouldOpen = doors.Without(doorWithPrize, doorPickedByContestant)
from doorOpenedByQuizMaster in OneOf(doorsQuizMasterCouldOpen)
...
```
we could have also written:
```C#
...
from doorOpenedByQuizMaster in OneOf(doors.Without(doorWithPrize, doorPickedByContestant))
...
```

The third and final step is understanding what the `select` clause does. 
The `select` clause allows us to combine and transform *outcomes* from one
or more *random variables* into a *new outcome*. In other words, with the 
`select` clause we are effectively defining a new random variable.
The example query above defines a random variable with two possible outcomes:
`true` if the contestant wins by switching doors 
and `false` if the contestant loses.



## Background information
The key ingredients needed for enabling expressive queries such as the example 
listed above are:
 - a suitable data structure to represent random variables, and
 - suitable definitions for `Select` and `SelectMany` methods that operate on 
   the data structure.

The solution here uses *vectors* to represent random variables, where scalar
components of these vectors represent the probability of occurrence of possible 
outcomes. For example, a fair coin could be represented as follows:
```C#
var fairCoin = 0.5.Of("head") + 0.5.Of("tails");
Console.WriteLine(fairCoin); // prints: {head: 0.5, tails: 0.5}
```

The idea of using vectors as to represent random variables was taken from 
the talk [Commutative Monads, Diagrams and Knots](https://vimeo.com/6590617) 
presented by [Dan Piponi](https://twitter.com/sigfpe) 
at the [International Conference on Functional Programming (ICFP), Edinburgh 2009](http://www.cs.nott.ac.uk/~pszgmh/icfp09.html).
Please see this talk for more information, including suitable definitions for
`Select` (called `fmap` in the talk) and `SelectMany` (which can be defined in
terms of `Select` and a function called `join`).


## Projects in this repository
The repository consists of the following projects:

 - [MontyHallLINQ](MontyHallLINQ): 
   The main program.
 - [VectorSpace](VectorSpace): 
   Library that provides the `Vector<T>` class for representing probabilities 
   as vectors.
 - [VectorSpace.Tests](VectorSpace.Tests): 
   Test suite for [VectorSpace](VectorSpace).