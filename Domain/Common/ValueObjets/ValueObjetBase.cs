namespace Domain.Common.ValueObjets
{
    public abstract class ValueObject

    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj is not ValueObject other || GetType() != other.GetType())
                return false;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                    HashCode.Combine(current, obj != null ? obj.GetHashCode() : 0));
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            return a is null ? b is null : a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }
    }
}
