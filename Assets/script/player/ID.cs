public class ID
{
    private static int nextId = 1;
    public int Value { get; private set; }

    public ID()
    {
        Value = nextId++;
    }

    public ID(int value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
