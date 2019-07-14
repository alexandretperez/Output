using Output.UnitTests.Models.Chained;
using Output.UnitTests.Models.Dependency;
using System;

namespace Output.UnitTests.Services
{
    public interface ISomeDataService
    {
        SomeDataDto GetSomeData();
    }

    public class SomeDataService : ISomeDataService
    {
        private readonly IMapper _mapper;

        public SomeDataService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public SomeDataDto GetSomeData()
        {
            return _mapper.Map<SomeDataDto>(new SomeData(100, "Almanaque", new DateTime(2019, 06, 22)));
        }
    }
}