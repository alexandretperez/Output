.. include:: styles.txt

Dependency Injection
====================

Nowadays is difficult to think in not use dependency injection in our applications, everything is much easier with DI.

We can easily inject the mapper instance passing the contract :underline:`IMapper` where we need it.

Here is an example using the *Microsoft.Extensions.DependencyInjection* but we can use it with any DI container.

.. code-block:: C#

    public void ConfigureServices(IServiceCollection services)
    {
        // ...

        // Mapping Engine
        var provider = new MappingProvider();

        MappingConfigurationSetup.ConfigCustomMappings(provider);

        services.AddSingleton<IMapper, Mapper>(s => new Mapper(provider));

        // ...
    }

    // this is just for instance

    public class MappingConfigurationSetup 
    {
        public static void ConfigCustomMappings(IMappingProvider provider)
        {
            // ... custom configurations
        }
    }
    

Now is just inject it where we need.

.. code-block:: C#

    public abstract class ApplicationService 
    {
        private readonly IMapper _mapper;

        public ApplicationService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TOutput Map<TOutput>(object input)
        {
            return _mapper.Map<TOutput>(input);
        }
    }


    public class CustomerAppService : ApplicationService
    {
        private readonly ICustomerService _customerService;

        public CustomerAppService(ICustomerService customerService, IMapper mapper) : base(mapper)
        {
            _customerService = service;
        }

        public List<CustomerDto> GetCustomers()
        {
            return Map<List<CustomerDto>>(_customerService.GetCustomers());
        }

        public CustomerDto FindById(int id)
        {
            return Map<CustomerDto>(_customerService.FindById(id));
        }

        // ...
    }