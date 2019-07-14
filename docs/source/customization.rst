.. include:: styles.txt

Customizations
==============

Is very simple to customize and improve the :underline:`Output` mapping engine, it's require just a basic knowledgement about *Reflections* and *System.Linq.Expressions*.

Basically we need create our own provider inherited from :underline:`Output.Providers.MappingProvider` and start to override some methods.

Let's get an example:

.. code-block:: C#

    // ENTITY

    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int GetAge()
        {
            var days = (DateTime.Today - BirthDate).TotalDays;
            if (days > 0 && days < int.MaxValue)
                return (int)Math.Floor(days / 365.2524);

            return -1; // invalid
        }
    }

    // DTO

    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
    }


Mapping *Customer* to *CustomerDto* will result in a empty (0 in this case) *Age* value. 
It occurs because the names must match and in the *Customer* entity we have the method named "GetAge()" and not "Age()".

We can change the name "Age" in our DTO to "GetAge" but of course this is not even close a good solution. 

Maybe we can create a custom mapping as described in :doc:`configuration` but this is restricted to a pair of types so every time that a method
*GetSomething()* apper again we need create or update a configuration. Not good either.

The best is tell to :underline:`Output` ignore the "Get" prefix of every :feature:`Method` found in our |input|.

To do so, we just need make a very simple change.

Overriding the GetMember
------------------------

.. code-block:: C#

    public class MyMappingProvider : MappingProvider
    {
        protected override Expression GetMember(Expression input, string name)
        {
            // first we let the base try to find an equivalent 
            // Field, Property or Method with name's parameter
            var result = base.GetMember(input, name);
            if (result == null)
            {
                // As it didn't find anything, 
                // we will look for a method prefixed with "Get"
                var method = input.Type.GetMethod("Get" + name, Type.EmptyTypes);

                // if found, we return the equivalent expression
                if (method != null)
                    return Expression.Call(input, method); 
            }

            return result;
        }	
    }


    // using it

    var mapper = new Mapper( new MyMappingProvider() );

    mapper.Map<CustomerDto>(customer); // Age will be mapped now.

Now every "GetSomething()" methods will be mapped to a property "Something".

Easy right? 

But now, let's make the :underline:`Output` engine even better creating a custom resolver.

Customizing Resolvers
---------------------

Instead of representing our "Email" property as a string in our entity model, it's more interesting create a custom type "Email" where we can put all the validation logic into it.

.. code-block:: C#

    public class Email
    {
        public Email(string value)
        {
            if (IsValid(value))
                Value = value;
        }

        public string Value { get; private set; }

        public static bool IsValid(string value)
        {
            var reg = new Regex("@"); // our naive regular expression to validate the e-mail
            return reg.IsMatch(value);
        }
    }

    // ... Customer changes
    public Email Email { get; set; }
    // ...

To map the *Customer.Email* to *CustomerDto.Email* we have two common ways:

Change the *CustomerDto.Email* to *CustomerDto.EmailValue* leaving the mapping job to the flattening engine or 
override the *Customer.ToString()* implementation returning *Customer.Value* and then the :doc:`AnyToStringResolver </resolvers>` deal with it.

Maybe this is enough in some situations but there is a better way to do that by creating an **EmailResolver**.

To create it, we must inherit the contract :underline:`Output.Resolvers.IResolver` and implement the :underline:`Resolve` method.

.. code-block:: C#
        
    public class EmailResolver : IResolver
    {
        public Expression Resolve(Expression input, Expression output);
        {
            // First we check if the input and output types is of what we expect. (Email and string)
            if (input.Type == typeof(Email) && output.Type == typeof(string))

                // if so, we return the string property Value from our input.
                return Expression.Property(input, "Value");

            return null; // otherwise we return null.
        }
    }

The result above represents this ``input.Email.Value`` but there is some changes to make.

If our input instance has not a valid email, the property will be ``null`` and consequently 
we get a ``NullReferenceException`` when we try to access the Value property. Thus, to avoid that, a better
result should be ``input.Email != null ? input.Email.Value : null``.

Of course you can implement the verifications by yourself but the :underline:`Output` already has a 
built-in *ExpressionVisitor* that do this. Let's apply the changes:

.. code-block:: C#

    using Output.Visitors;

    public class EmailResolver : IResolver
    {
        public Expression Resolve(Expression input, Expression output)
        {
            if (input.Type == typeof(Email) && output.Type == typeof(string))
            {
                var value = Expression.Property(input, "Value");
                
                // Handle null references
                return new NullCheckVisitor(value).Visit();
            }
            return null;
        }
    }

The code above is able to map an :underline:`Email` to a :underline:`string`. But what about from a :underline:`string` to an :underline:`Email`. Let's complete the code.

.. code-block:: C#

    public class EmailResolver : IResolver
    {
        public Expression Resolve(Expression input, Expression output)
        {
            if (input.Type == typeof(Email) && output.Type == typeof(string))
            {
                var value = Expression.Property(input, "Value");

                return new NullCheckVisitor(value).Visit();
            }
            else if (input.Type == typeof(Email) && output.Type == typeof(string))
            {
                var emailCtor = typeof(Email).GetConstructor(new [] { typeof(string) });

                // initializes the Email type passing the input (string) as parameter
                return Expression.New(emailCtor, input);
            }

            return null;
        }
    }

Now our custom resolver is complete, we just need register it into our custom provider.

Overriding the GetResolvers
---------------------------

Member resolvers are retrieved from *.GetResolvers* method. Let's override it.

.. code-block:: C#

    public class MyMappingProvider : MappingProvider
    {
        // ... GetMember(...)

        protected override IEnumerable<IResolver> GetResolvers()
        {   
            yield return new EmailResolver();

            foreach (var resolver in base.GetResolvers())
                yield return resolver;
        }
    }

.. note::

    Is recommended that you return your custom resolvers before the built-in ones, otherwise you may get unwanted results 
    because another resolver was used instead of what you created.