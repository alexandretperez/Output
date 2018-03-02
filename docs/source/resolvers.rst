.. include:: styles.txt

.. _resolvers:

Resolvers
=========

Resolvers are used to solve the mapping between two members based on their types.

The :doc:`providers` determines a sequence that the resolvers are executed and each one is only triggered if the previous one was not able solve the mapping.

You don't need to worry about resolvers, this documentation is just to explain how each one works.

Bellow, we are listing all built-in :underline:`Output's` resolvers and what they can do.

.. _typesAssignableResolver:

TypesAssignableResolver
-----------------------

Can map the members with the same type or when it's underlying type matches.

+------------+-------------+-----------------+
| Input Type | Output Type | Is able to map? |
+------------+-------------+-----------------+
| int        | int         | |yes|           |
+------------+-------------+-----------------+
| int        | int?        | |yes|           |
+------------+-------------+-----------------+
| int?       | int         | |yes|           |
+------------+-------------+-----------------+
| byte       | int         | |no|            |
+------------+-------------+-----------------+
| decimal    | string      | |no|            |
+------------+-------------+-----------------+
| etc.       |             |                 |
+------------+-------------+-----------------+


.. _primitiveResolver:

PrimitiveResolver
-----------------

When the type is primitive or is decimal type, this resolver tries to map by conversion.

+------------+-------------+-----------------+
| Input Type | Output Type | Is able to map? |
+------------+-------------+-----------------+
| byte       | int         | |yes|           |
+------------+-------------+-----------------+
| long       | decimal     | |yes|           |
+------------+-------------+-----------------+
| int?       | int         | |no|            |
+------------+-------------+-----------------+

GuidResolver
------------

Can convert a :underline:`System.Guid` to string or to a byte array and vice-versa.

+------------------+------------------+-----------------+
| Input Type       | Output Type      | Is able to map? |
+------------------+------------------+-----------------+
| Enum             | string or byte[] | |yes|           |
+------------------+------------------+-----------------+
| string or byte[] | Enum             | |yes|           |
+------------------+------------------+-----------------+

Mapping from a :underline:`string` or :underline:`byte array` to a :underline:`Guid`, requires of course, a valid data format otherwise an :underline:`Exception` will occur.

.. _enumResolver:

EnumResolver
------------

Can convert an Enum to all it's approved types (
:underline:`byte`, 
:underline:`sbyte`, 
:underline:`short`, 
:underline:`ushort`, 
:underline:`int`, 
:underline:`uint`, 
:underline:`long` or 
:underline:`ulong`), to a :underline:`string` or to an equivalent :underline:`Enum`.

The reverse map also works.

+--------------------------+-------------------------+-----------------+
| Input Type               | Output Type             | Is able to map? |
+--------------------------+-------------------------+-----------------+
| Approved Types / String  | Enum                    | |yes|           |
+--------------------------+-------------------------+-----------------+
| Enum                     | Approved Types / String | |yes|           |
+--------------------------+-------------------------+-----------------+
| Enum                     | Enum (Equivalent)       | |yes|           |
+--------------------------+-------------------------+-----------------+

e.g:

.. code-block:: C#

    public enum Level
    {
        None = 0,
        Gold = 1,
        Silver = 2,
        Bronze = 3
    }

    public class Source
    {
        public Level LevelA { get; set; }
        public Level LevelB { get; set; }
        public Level LevelC { get; set; }
    }

    public enum Another
    {
        None = 0,
        Gold = 1,
        Silver = 2,
        Bronze = 3
    }

    public class Target
    {
        public Another LevelA { get; set; }
        public string LevelB { get; set; }
        public int LevelC { get; set; }
    }

    // Mapping to the Target a source like:
    new Source
    {
        LevelA = Level.Gold,
        LevelB = Level.Silver,
        LevelC = Level.Bronze
    });

    // Will result in:
    -> Target {
        LevelA == Another.Gold
        LevelB == "LevelB"
        LevelC == 3
    }


DictionaryResolver
------------------

It can map a :underline:`Dictionary<TKeyType, TValueType>` to another :underline:`Dictionary<TAnotherKeyType, TAnotherValueType>`.

The types of **Key** and **Value** are resolved by the :ref:`resolvers` described on this page.

In the example bellow, the dictionary **Key** is solved by :ref:`typesAssignableResolver` and the dictionary **Value** by :ref:`enumResolver`.

+-------------------------+-------------------------+-----------------+
| Input Type              | Output Type             | Is able to map? |
+-------------------------+-------------------------+-----------------+
| Dictionary<int, MyEnum> | Dictionary<int, string> | |yes|           |
+-------------------------+-------------------------+-----------------+
| etc.                                                                |
+---------------------------------------------------------------------+


CollectionResolver
------------------

This resolver can convert many types derived from generics :underline:`ICollection<T>` or :underline:`IReadOnlyCollection<T>` to another type derived from the same.

There are many possibilities here, so, to simplify I will show just a few possibilities.

+------------+-------------+-----------------+
| Input Type | Output Type | Is able to map? |
+------------+-------------+-----------------+
| List<T>    | T[]         | |yes|           |
+------------+-------------+-----------------+
| T[]        | HashSet<T>  | |yes|           |
+------------+-------------+-----------------+
| Stack<T>   | Queue<T>    | |yes|           |
+------------+-------------+-----------------+
| etc.                                       |
+--------------------------------------------+

ClassResolver
-------------

This resolver is used when the input and output members represents a class.

Internally, it just uses the current mapper to do a new mapping. 


AnyToStringResolver
-------------------

Converts any type to string calling the implementation of ``.ToString`` from the |input|.

---------------------------------------------------------------------------

ConstructorResolver
-------------------

The constructor resolver is a special kind of resolver used to determine how the destination object must be instantiated.

It *prioritizes* the *parameterless constructor* but when the |output| object doesn't have one, it tries to determine one based on |input| properties.

e.g:

.. code-block:: C#

    public class Sample
    {
        public string Title { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class SampleDto
    {
        public SampleDto(string title, bool isActive)
        {
            Title = title;
            IsActive = isActive;
        }

        public string Title { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
    }

As we can see, *SampleDto* has only one constructor with *title* and *isActive* as parameters.

Doing a mapping from *Sample* to *SampleDto*, the *ConstructorResolver* will try find the public :feature:`Properties` "Title" and "IsActive" 
in our *Sample* instance to populate the constructor parameters.

It operates in case insensitive, so does not matter if we have "isActive", "ISACTIVE" or "isactive". All these are considered as long they are public :feature:`Properties`.

An :feature:`AmbiguosMatchException` can occur of course, if we have two public properties with the same name declared with different text cases.

In case of multiple constructors, the execution order is from the one with highest parameters amount to the one with lowest.

+-------------------------------------------------------------------+-----------------+
| Constructor                                                       | Execution order |
+-------------------------------------------------------------------+-----------------+
| SampleDto(string title, DateTime RegistrationDate, bool IsActive) | 1               |
+-------------------------------------------------------------------+-----------------+
| SampleDto(string, bool IsActive)                                  | 2               |
+-------------------------------------------------------------------+-----------------+
| SampleDto(string title)                                           | 3               |
+-------------------------------------------------------------------+-----------------+
| etc.                                                              |                 |
+-------------------------------------------------------------------+-----------------+

**Remember**, the **priority** is the *parameterless* constructor, only if it doesn't exists that the others are going to be used.

Complex Scenarios
^^^^^^^^^^^^^^^^^

Prefixed properties
'''''''''''''''''''

.. code-block:: C#

    public class FooDto
    {
        public string Name { get; set; }
        public int CreatedDateYear { get; set; }
        public int CreatedDateMonth { get; set; }
        public int CreatedDateDay { get; set; }
        public string BarName { get; set; }
    }

    public class Foo
    {
        public Foo(string name, DateTime createdDate, Bar bar)
        {
            Name = name;
            CreatedDate = createdDate;
            Bar = bar;
        }
        public string Name { get; }
        public DateTime CreatedDate { get; }
        public Bar Bar { get; }
    }

    public class Bar 
    {
        public Bar(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

Mapping *FooDto* to *Foo* also works.

As *FooDto* doesn't have a *CreatedDate* property of *System.Date* type, the *ConstructorResolver* look for all properties prefixed by *CreatedDate*.

.. |created_date| raw:: html

   <span class="light">CreatedDate</span><strong>Year</strong>, 
   <span class="light">CreatedDate</span><strong>Month</strong> and 
   <span class="light">CreatedDate</span><strong>Day</strong>

In this case, |created_date|.

With Year, Month and Day the *ConstructorResolver* is able to build the *CreatedDate* of *Foo* using the constructor *DateTime(year, month, day)*.

The same method is used to solve the *Bar* constructor.


Custom map properties
'''''''''''''''''''''

.. code-block:: C#

    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CustomerDto
    {
        public CustomerDto(string fullName)
        {
            FullName = fullName;
        }

        public string FullName { get; set; }
    }

To map the property *CustomerDto.FullName* with the values of *Customer.FirstName* and *Customer.LastName* we need write a custom configuration. See more in :doc:`configuration`.

.. code-block:: C#

    var provider = new MappingProvider();

    provider.AddConfig<Customer, CustomerDto>(config =>
        config.Map(
            output => output.FullName, 
            input => input.FirstName + " " + input.LastName
        )
    );

    var mapper = new Mapper(provider);

    mapper.Map<CustomerDto>(customer);

Doing this, the *ConstructorResolver* will also use this configuration to determine how to resolve the *fullName* parameter of the constructor.


Create your own resolver
------------------------

You can also create your own resolver.

Read more in :doc:`customization`.