using System;

namespace Output
{
    public class TypePair : IEquatable<TypePair>
    {
        public TypePair(Type input, Type output)
        {
            Input = input;
            Output = output;
            _hashCode = CalculateHash(input, output);
        }

        public Type Input { get; }
        public Type Output { get; }

        private readonly int _hashCode;

        public bool Equals(TypePair other)
        {
            if (other is null)
                return false;

            return Input == other.Input && Output == other.Output;
        }

        public static int CalculateHash(Type input, Type output)
        {
            return unchecked((input.GetHashCode() * 907) + output.GetHashCode());
        }

        public override int GetHashCode() => _hashCode;

        public override bool Equals(object obj)
        {
            return Equals(obj as MapJob);
        }

        public static bool operator ==(TypePair left, TypePair right)
        {
            return left is null
                ? right is null
                : left.Equals(right);
        }

        public static bool operator !=(TypePair left, TypePair right)
        {
            return left is null
                ? !(right is null)
                : !left.Equals(right);
        }
    }
}