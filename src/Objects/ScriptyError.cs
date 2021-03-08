namespace Scripty.Objects
{
    using System.Collections.Generic;
    using Interfaces;

    public class ScriptyError : IObject
    {
        public ScriptyError(int code, IObject left, string op, IObject right)
        {
            switch (code)
            {
                case 2:
                    Message = $"[SC2] unknown operator: {left.Type()} {op} {right.Type()}";
                    Code = "[SC2]";
                    break;
                case 3:
                    Message = $"[SC3] type mismatch: {left.Type()} {op} {right.Type()}";
                    Code = "[SC2]";
                    break;
                case 4:
                    Message = $"[SC4] unknown operator: {op}{right.Type()}";
                    Code = "[SC3]";
                    break;
                case 5:
                    Message = $"[SC5] identifier not found: {op}";
                    Code = "[SC5]";
                    break;
                case 6:
                    Message = $"[SC6] {op} not a function";
                    Code = "[SC6]";
                    break;
                case 7:
                    Message = $"[SC7] wrong number of arguments: expected `{left.Inspect()}`, got {op}";
                    Code = "[SC7]";
                    break;
                case 8:
                    Message = $"[SC8] invalid operation: {op} has no length.";
                    Code = "[SC8]";
                    break;
                case 9:
                    Message = $"[SC9] index operator not supported for {left?.Type()}";
                    Code = "[SC9]";
                    break;
                case 10:
                    Message = $"[SC10] index `{right?.Inspect()}` is out of range for {left?.Inspect()}";
                    Code = "[SC10]";
                    break;
                case 11:
                    Message = "[SC11] null is not a function";
                    Code = "[SC11]";
                    break;
                case 12:
                    Message = $"[SC12] {op}() argument cannot be {left.Type()}";
                    Code = "[SC12]";
                    break;
                case 13:
                    Message = $"[SC13] cannot pass `{left.Inspect()}` to {op}()";
                    Code = "[SC13]";
                    break;
                case 14:
                    Message = $"[SC14] {left.Type()} is unusable as hash key";
                    Code = "[SC14]";
                    break;
                case 15:
                    Message = $"[SC15] `{left.Inspect()}` is not a hash";
                    Code = "[SC15]";
                    break;
                case 16:
                    Message = $"[SC16] `{left.Inspect()}` does not contain key `{right.Inspect()}`";
                    Code = "[SC16]";
                    break;
                default:
                    Message = "[SC1] Unknown error";
                    Code = "[SC1]";
                    break;
            }
        }

        public string Message { get; set; }
        public string Code { get; set; }

        public string Type() => ObjectType.ErrorObj;

        public string Inspect() => $"ERROR: {Message}";
        public Dictionary<string, IObject> Properties { get; set; }
    }
}