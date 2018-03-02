using Output.Benchmarking.Models.Complex;
using Output.Benchmarking.Models.Intense;
using System;

namespace Output.Benchmarking.Tests
{
    public class IntenseTest : TestBase<Profile, ProfileDto>
    {
        protected override string GetName() => nameof(IntenseTest);

        protected override Profile InitializeSource()
        {
            var profile = new Profile(Guid.NewGuid(), new Name("First", "Profile"), new Age(new DateTime(1983, 7, 30)));
            profile.FavoriteMovies.Add(Movie.LordOfTheRings());
            profile.FavoriteMovies.Add(Movie.TheMatrix());
            profile.FavoriteMovies.Add(Movie.TheRevenant());
            return profile;
        }
    }
}