using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects
{
    public class Error : IObject
    {
        public Error(int code, IObject? left, string? op, IObject? right)
        {
            switch (code)
            {
                case 2:
                    Message = $"[MUHL2] unknown operator: {left.Type()} {op} {right.Type()}";
                    Code = "[MUHL2]";
                    break;
                case 3:
                    Message = $"[MUHL3] type mismatch: {left.Type()} {op} {right.Type()}";
                    Code = "[MUHL2]";
                    break;
                case 4:
                    Message = $"[MUHL4] unknown operator: {op}{right.Type()}";
                    Code = "[MUHL3]";
                    break;
                case 5:
                    Message = $"[MUHL5] identifier not found: {op}";
                    Code = "[MUHL5]";
                    break;
                case 6:
                    Message = $"[MUHL6] {op} not a function";
                    Code = "[MUHL6]";
                    break;
                case 7:
                    Message = $"[MUHL7] wrong number of arguments: expected 1, got {op}";
                    Code = "[MUHL7]";
                    break;
                case 8:
                    Message = $"[MUHL8] invalid operation: {op} has no length.";
                    Code = "[MUHL8]";
                    break;
                default:
                    Message = "[MUHL1] Unknown error";
                    Code = "[MUHL1]";
                    break;
            }
        }

        public string Message { get; set; }
        public string Code { get; set; }

        public string Type()
        {
            return ObjectType.ErrorObj;
        }

        public string Inspect()
        {
            return $"ERROR: {Message}";
        }
    }
}