.. include:: styles.txt

Providers
=========

Providers are responsable to create the *mapping function*.

The implementation of the provider determines the support to property mappings, flattening, unflattening, collections, projections, etc.

The built-in provider is **MappingProvider** which deliveries every feature in this documentation.

MappingProvider
---------------

.. code-block:: C#

    IMappingProvider provider = new Output.Providers.MappingProvider();

    IMapper mapper = new Mapper(provider); // register the provider to a new mapper.
    

Read and Write capabilities
^^^^^^^^^^^^^^^^^^^^^^^^^^^

The **MappingProvider** can **read** :feature:`Fields`, :feature:`Properties` and *parameterless* :feature:`Methods` and can **write** to a destination :feature:`Properties`.

In general, the target property type must be equal to the source member type. This restrictions can vary based on the current resolver. Read more about :doc:`resolvers`.

.. code-block:: C#

    public class Sample
    {
        public Sample()
        {
            One = 1;
            Two = true;
        }

        public int One;
        public bool Two { get; set; }
        public string Three()
        {
            return "Hello";
        }
    }

    public class SampleDto
    {
        public int One { get; set; }
        public bool Two { get; set; }
        public string Three { get; set; }
    }

    var mapper = new Mapper( new MappingProvider() );

    var dto = mapper.Map<SampleDto>(new Sample());
    // -> dto.One == 1
    // -> dto.Two == true
    // -> dto.Three == "Hello"

Mapping Collections
^^^^^^^^^^^^^^^^^^^

There's no secret to mapping collections, the syntax is the same, you just need pass the right |output| and |input| parameters.

Let's say that we have a service that gets a list of customers and we want to convert that list to an array of our DTO. It's easy:

.. code-block:: C#

    var customers = customerService.GetCustomersByCountry("Brazil");

    mapper.Map<CustomerDto[]>(customers);

To know all collections support read about :doc:`CollectionResolver </resolvers>`.


Flattening and Unflattening
^^^^^^^^^^^^^^^^^^^^^^^^^^^

You can transform a nested object in a single flat object and vice-versa. Read more in :doc:`flattening`.


Mapping Configuration
^^^^^^^^^^^^^^^^^^^^^

You can customize the mapping between a |input| and |output|. Read more in :doc:`configuration`.


Create your own provider
------------------------

You can also create your own provider.

Read more in :doc:`customization`.