using Library.Miscellaneous;
using System.Windows.Markup;

namespace WPFApp.Converters
{
    public class EnumToStringItemsSource : MarkupExtension
    {
        private readonly Type _type;

        public EnumToStringItemsSource(Type type)
        {
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type)
                .Cast<object>()
                .Select(e => new { Value = e, DisplayName = (e as Enum).GetEnumDescription() });
        }
    }
}
