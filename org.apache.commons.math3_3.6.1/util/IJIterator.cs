namespace org.apache.commons.math3.util
{
    public interface IJIterator<E>
    {
        bool HasNext();

        E Next();

        void Remove();
    }
}