Map Chain
=========

Map chain is just a consecutive mapping in a shared ``TOutput`` object.

We use the syntax bellow multiple times using the same *shared object* as the second parameter.

.. code-block:: C#

    mapper.Map<TOutput>(TInput input, TOuput output);

Sometimes all information that we need must be obtained from different sources.

Suppose that we have three services, the customer service:

.. code-block:: C#

    customerService.getInfoByCustomerId(customerId);

    // returns 
    CustomerInfo {
        Guid CustomerId;
        string Email;
        string Name;
        DateTime Registration;
    }

The financial service:

.. code-block:: C#

    financialService.getUserAccountInfo(userId);

    // returns 
    UserAccessInfo {
        Guid UserId;
        string AccountNumber;
        string BankNumber;
        string BankName;
    }

And the access service:

.. code-block:: C#

    accessService.getAccessInfoByUserId(userId);

    // returns 
    UserAccessInfo {
        Guid UserId;
        string UserName;
        bool IsAuthorized;
        DateTime LastAccess;
    }

Using a shared object
---------------------

Now we want put all this information together in a single DTO.

.. code-block:: C#

    public class FullCustomerInfoDto
    {
        // data from customer service
        public Guid CustomerId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime Registration { get; set; }

        // data from financial service except UserId
        public string AccountNumber { get; set; }
        public string BankNumber { get; set; }
        public string BankName { get; set; }

        // data from access service except UserId
        public string UserName { get; set; }
        public bool IsAuthorized { get; set; }
        public DateTime LastAccess { get; set; }
    }

We just need to perform the mapping three times over the same DTO instance.

.. code-block:: C#

    var sharedObj = new FullCustomerInfoDto();

    mapper.Map(customerService.getInfoByCustomerId(userId), sharedObj);
    mapper.Map(financialService.getUserAccountInfo(userId), sharedObj);
    mapper.Map(accessService.getAccessInfoByUserId(userId), sharedObj);

There's a easier way to do that through an extension method.

.. code-block:: C#

    using Output.Extensions;
    
    // ...

    var sharedObj = new FullCustomerInfoDto();

    mapper.Map(sharedObj, 
        customerService.getInfoByCustomerId(userId),
        financialService.getUserAccountInfo(userId),
        accessService.getAccessInfoByUserId(userId)
    );