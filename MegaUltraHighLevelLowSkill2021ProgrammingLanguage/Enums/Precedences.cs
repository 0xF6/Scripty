namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Enums
{
    public enum Precedences
    {
        LOWEST,
        EQUALS, // == or !=
        LESSGREATER, // > or <
        SUM, // + or -
        PRODUCT, // * or /
        PREFIX, // -x or !x
        CALL, // someFunc(x)
        INDEX // arr[index]
    }
}