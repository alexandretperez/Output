namespace Output.Benchmarking.Models.Flattening
{
    public class Earth
    {
        public Continent Continent { get; set; }

        public static Earth Create()
        {
            return new Earth
            {
                Continent = new Continent
                {
                    Country = new Country
                    {
                        Name = "Brazil",
                        State = new State
                        {
                            Name = "Paraná",
                            City = new City
                            {
                                Name = "Curitiba"
                            }
                        }
                    }
                }
            };
        }
    }

    public class Continent
    {
        public Country Country { get; set; }
    }

    public class Country
    {
        public State State { get; set; }
        public string Name { get; set; }
    }

    public class State
    {
        public string Name { get; set; }
        public City City { get; set; }
    }

    public class City
    {
        public string Name { get; set; }
    }

    public class EarthDto
    {
        public string ContinentCountryName { get; set; }
        public string ContinentCountryStateName { get; set; }
        public string ContinentCountryStateCityName { get; set; }
    }
}