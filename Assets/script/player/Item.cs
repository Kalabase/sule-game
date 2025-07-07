using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string id;
    public List<string> generalType;
    public List<string> specificType;
    public List<string> functions;

    public Item()
    {
        functions = new List<string> { "select" };
    }

    public override string ToString()
    {
        return $"Item: {id}";
    }
}