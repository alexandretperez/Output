.. include:: styles.txt

Getting Started
===============

Syntax to perform mapping:

.. code-block:: c#

    mapper.Map<TOutput>(TInput input);

``TOutput`` is our destination type and ``TInput`` is our source one.


Initialization
--------------

A :underline:`mapper` requires a :underline:`provider` to be instantiated. 

.. code-block:: C#

    IMappingProvider provider = new Output.Providers.MappingProvider();

    IMapper mapper = new Mapper(provider);


Entities to respectives DTOs
----------------------------

Given our entities:

.. code-block:: c#

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ProductCategory Category { get;set; }
    }

    public class ProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

And our DTOs:
 
.. code-block:: c#

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ProductCategoryDto Category { get;set; }
    }

    public class ProductCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

Sample:

.. code-block:: c#

    var product = new Product()
    {
        Id = Guid.NewGuid(),
        Name = "Coffee",
        IsActive = true,
        Category = new ProductCategory() 
        {
            Id = Guid.NewGuid(),
            Name = "Heaven's Goods"
        }
    }

    var mapper = new Mapper(new MappingProvider());

    // performing the mapping
    mapper.Map<ProductDto>(product);

Transforming to a flat DTO
--------------------------

You can also transform the |input| nested objects to a flatten |output| one.

.. code-block:: c#

    // ... previous code above

    public class ProductFlatDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }        
    }

    mapper.Map<ProductFlatDto>(product);

Read more about :doc:`flattening`.


Existing TOutput object
-----------------------

You can also pass a instance of the |output| as the second parameter of the Map's method.

.. code-block:: C#

    var dto = new ProductDto();

    mapper.Map(product, dto);

Internally it uses this already instantiated object to perform the mappings.

See also :doc:`chain`.