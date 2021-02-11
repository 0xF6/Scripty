namespace Scripty.Enums
{
    public enum Precedences
    {
        Lowest,
        Equals, // == or !=
        Lessgreater, // > or <
        Sum, // + or -
        Product, // * or /
        Prefix, // -x or !x
        Call, // someFunc(x)
        Index // arr[index]
    }
}