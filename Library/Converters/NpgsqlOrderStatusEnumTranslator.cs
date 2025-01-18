using Library.Enums;
using Npgsql;
using Library.Miscellaneous;

namespace Library.Converters
{
    public class NpgsqlOrderStatusEnumTranslator : INpgsqlNameTranslator
    {
        public string TranslateMemberName(string clrName)
        {
            return Enum.Parse<OrderStatus>(clrName, true).GetEnumDescription();
        }

        public string TranslateTypeName(string clrName)
        {
            return "order_status";
        }
    }
}
