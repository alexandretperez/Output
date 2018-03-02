# Output

A .NET object-object mapper fast, powerful, extensible, easy to learn and to extend!

* No configuration required
* Flattening and unflattening capabilities
* LINQ Projections
* Chained Mapping
* Custom Configurations
* Custom Providers and Resolvers

## Installing

```
PM> Install-Package Output
```

## Getting Started

``` c#
    // Instantiate a provider...
    var provider = new MappingProvider();

    // ... and its respective mapper
    var mapper = new Mapper(provider);

    // Done! Now we are ready to go.
    mapper.Map<DTO>(Model);
```

## Docs

The full documentation is available at [Read the Docs](http://output.readthedocs.io).

## Dependency

.NET Standard 1.0+

## Credits

The Output project is developed by Alexandre T. Perez under [MIT License](LICENSE).

