﻿namespace Output.UnitTests.Models.RealWorld
{
    public class Actor
    {
        public Actor(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public class ActorDto
    {
        public string Name { get; set; }
    }
}