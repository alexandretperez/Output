.. include:: styles.txt

Mapping Configurations
======================

Mapping configurations allow us to create custom mappings between |input| and |output| objects.

There are three methods that we can use.


**.Map**: Determines how a property in |output| should be mapped.

.. code-block:: C#

    .Map<TProperty>(
        Expression<Func<TOutput, TProperty>> output, 
        Expression<Func<TInput, TProperty>> input
    );

**.Ignore**: Removes a property from mapping operation.

.. code-block:: C#

    .Ignore<TProperty>(
        Expression<Func<TOutput, TProperty>> output
    );

**.Instance**: Determines how the |output| should be instantiate.

.. code-block:: C#

    .Instance<TProperty>(
        Expression<Func<TOutput, TProperty>> output
    );




Let's look into it.

.. code-block:: C#

    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

In your DTO we do not have the "FirstName" and "LastName" but we have the "FullName". Let's configure that.

.. code-block:: C#

    var customersConfig = new MappingConfiguration<Customer, CustomerDto>();
    
    customersConfig.Map(p => p.FullName, p => p.FirstName + " " + p.LastName);

Now we just need to register this to the provider before instantiate the mapper.

.. code-block:: C#

    var provider = new MappingProvider();

    provider.AddConfig(customersConfig); // registers the configuration

    var mapper = new Mapper(provider);

You can add as many configuration as you want.

.. code-block:: C#

    provider.AddConfig(customersConfig);

    var employeesConfig = new MappingConfiguration<Employee, EmployeeDto>()
        .Ignore(p => p.BirthDate)
        .Ignore(p => p.HireDate);

    var productsConfig = new MappingConfiguration<Product, ProductDto>()
        .Map(p => p.FullName, p => p.Category.Name + " " + p.Name)
        .Instantiate(p => new Product(p.Id, p.Name));

    provider.AddConfig(employeesConfig);
    provider.AddConfig(productsConfig);

We can use an alternative syntax to register the configuration as well.

.. code-block:: C#

    using Output.Extensions;

    ...

    provider.AddConfig<Customer, CustomerDto>(config => 

        // config is a instance of MappingConfiguration<Customer, CustomerDto>
        config.Map(...)
        config.Ignore(...);
        // ...etc
    );

    provider.AddConfig<Employee, EmployeeDto>(config => ...);
    provider.AddConfig<Product, ProductDto>(config => ...);