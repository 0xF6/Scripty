namespace Scripty.Objects
{
    using Interfaces;

    public class ScriptyError : IObject
    {
        public ScriptyError(int code, IObject left, string op, IObject right)
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
                    Message = $"[MUHL7] wrong number of arguments: expected `{left.Inspect()}`, got {op}";
                    Code = "[MUHL7]";
                    break;
                case 8:
                    Message = $"[MUHL8] invalid operation: {op} has no length.";
                    Code = "[MUHL8]";
                    break;
                case 9:
                    Message = $"[MUHL9] index operator not supported for {left?.Type()}";
                    Code = "[MUHL9]";
                    break;
                case 10:
                    Message = $"[MUHL10] index `{right?.Inspect()}` is out of range for {left?.Inspect()}";
                    Code = "[MUHL10]";
                    break;
                case 11:
                    Message = "[MUHL11] null is not a function";
                    Code = "[MUHL11]";
                    break;
                case 12:
                    Message = $"[MUHL12] {op}() argument cannot be {left.Type()}";
                    Code = "[MUHL12]";
                    break;
                case 13:
                    Message = $"[MUHL13] cannot pass `{left.Inspect()}` to {op}()";
                    Code = "[MUHL13]";
                    break;
                case 14:
                    Message = $"[MUHL14] {left.Type()} is unusable as hash key";
                    Code = "[MUHL14]";
                    break;
                case 15:
                    Message = $"[MUHL15] `{left.Inspect()}` is not a hash";
                    Code = "[MUHL15]";
                    break;
                case 16:
                    Message = $"[MUHL16] `{left.Inspect()}` does not contain key `{right.Inspect()}`";
                    Code = "[MUHL16]";
                    break;
                default:
                    Message = "[MUHL1] Unknown error";
                    Code = "[MUHL1]";
                    break;
            }
        }

        public string Message { get; set; }
        public string Code { get; set; }

        public string Type() => ObjectType.ErrorObj;

        public string Inspect() => $"ERROR: {Message}";
    }
}