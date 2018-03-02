.. include:: styles.txt

Output
======

:underline:`Output` is a .NET object-object mapper designed to be fast, powerful, extensible, easy to learn and to extend!

Some of features of the :underline:`Output` library includes:

* No configuration required
* Flattening and unflattening capabilities
* LINQ Projections
* Chained Mapping
* Custom Configurations
* Custom Providers and Resolvers

Getting Started
---------------

.. code-block:: c#

    // Instantiate a provider...
    var provider = new MappingProvider();

    // ... and its respective mapper
    var mapper = new Mapper(provider);

    // Done! Now we are ready to go.
    mapper.Map<DTO>(Model);

.. Check out the :doc:`getting-started` page to more concrete examples.

Dependency
----------

.NET Standard 1.0+

License
-------

MIT License

Repository
----------

This project is available at |github|

.. toctree::
   :maxdepth: 2
   :hidden:

   getting-started
   providers
   resolvers
   flattening
   configuration
   projections
   chain
   customization
   dependency-injection
   performance