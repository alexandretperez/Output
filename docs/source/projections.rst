.. include:: styles.txt

LINQ Projections
================

For query projections we must use the following syntax:

.. code-block:: C#

    mapper.Project<TOutput>(IQueryable<TInput> input);

Let's see how this works and what is the difference between this syntax and the ``mapper.Map``.

Suppose we have this entity represented in our database.

.. code-block:: C#

    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    	public int? CategoryID { get; set; }
    	public decimal? UnitPrice { get; set; }
        public bool Discontinued { get; set; }
        public DateTime? LastSupply { get; set; }
    }    

And we want project this to a very simple DTO:

.. code-block:: C#

    public class ProductDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int? CategoryID { get; set; }
    }

Telling to :underline:`Output` get a list of *ProductDto* writting ``mapper.Map<List<ProductDto>>(dbContext.Products)``, behind the scenes, the SQL produced will be:

.. code-block:: SQL

    SELECT 
        [t0].[ProductID], 
        [t0].[ProductName], 
        [t0].[CategoryID], 
        [t0].[UnitPrice], 
        [t0].[Discontinued], 
        [t0].[LastSupply]
    FROM [Products] AS [t0]

Notice that even our DTO represents only 3 properties of the Product Entity, all the 6 properties was requested.

Using the ``mapper.Project<ProductDto>(dbContext.Products)`` we fix that.

.. code-block:: SQL

    SELECT [t0].[ProductID], [t0].[ProductName], [t0].[CategoryID]
    FROM [Products] AS [t0]

This also prevent the lazy loading **SELECT N+1 problems**.

Let's add the category entity to illustrate that.

.. code-block:: C#

	public class Category
	{
		public int CategoryID { get; set; }
		public string CategoryName { get; set; }
		public List<Product> Products { get; set; }
	}

And it's respective DTO.

.. code-block:: C#

	public class CategoryDto
	{
		public int CategoryID { get; set; }
		public string CategoryName { get; set; }
		public List<ProductDto> Products { get; set; }
	}

When we run ``mapper.Map<List<CategoryDto>>(dbContext.Categories)``, one SQL will be produced for the *Category* and for each record of it another SQL will be 
produced to get all Products associated to that record.

Running ``mapper.Project<CategoryDto>(dbContext.Categories)`` we run against the database only once:

.. code-block:: SQL

    SELECT 
        [t0].[CategoryID], 
        [t0].[CategoryName], 
        [t0].[Description], 
        [t1].[ProductID], 
        [t1].[ProductName], 
        [t1].[CategoryID] AS [CategoryID2], (
            SELECT COUNT(*)
            FROM [Products] AS [t2]
            WHERE [t2].[CategoryID] = [t0].[CategoryID]
        ) AS [value]
    FROM [Categories] AS [t0]
    LEFT OUTER JOIN [Products] AS [t1] ON [t1].[CategoryID] = [t0].[CategoryID]
    ORDER BY [t0].[CategoryID], [t1].[ProductID]

As we can see, when we are working with *IQueryable* is much better use ``mapper.Project`` instead of ``mapper.Map``. For all other cases use ``mapper.Map``.