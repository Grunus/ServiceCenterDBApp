using System.ComponentModel;

namespace Library.Miscellaneous
{
    //Thanks to stack overflow, although I'll still soon need to learn reflection :(
    public static class ExtensionMethodsForEnums
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
            throw new ArgumentException("Enum value hasn't been found", nameof(enumValue));
        }

        public static T GetEnumValueByDescription<T>(this string description) where T : Enum
        {
            foreach (Enum enumItem in Enum.GetValues(typeof(T)))
            {
                if (enumItem.GetEnumDescription() == description)
                {
                    return (T)enumItem;
                }
            }
            throw new ArgumentException("Enum description hasn't been found", nameof(description));
        }
    }
}
