using System;
using System.Collections.Generic;
using System.Linq;

namespace Output.UnitTests.Models.Basics
{
    public class Collection
    {
        public Collection()
        {
            IEnumerable_IQueryable = new[] { 1, 2, 3 }.Select(p => p);
            List_IEnumerable = new List<string> { Guid.NewGuid().ToString() };
            IQueryable_Array = new[] { DateTime.Now }.AsQueryable();
            ICollection_HashSet = new[] { "Hello", "World" };
        }

        public IEnumerable<int> IEnumerable_IQueryable { get; set; }
        public List<string> List_IEnumerable { get; set; }
        public IQueryable<DateTime> IQueryable_Array { get; set; }
        public ICollection<string> ICollection_HashSet { get; set; }
    }

    public class CollectionDto
    {
        public IQueryable<int> IEnumerable_IQueryable { get; set; }
        public IEnumerable<string> List_IEnumerable { get; set; }
        public DateTime[] IQueryable_Array { get; set; }
        public HashSet<string> ICollection_HashSet { get; set; }
    }
}