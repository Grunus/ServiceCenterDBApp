using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace WPFApp.Helpers
{
    public static class MessageDisplayHelper
    {
        public static void DisplayError(Exception ex)
        {
            string message;

            if (ex.GetType() == typeof(AggregateException))
                message = ex.InnerException.InnerException.Message;
            else if (ex.GetType() == typeof(DbUpdateException))
                message = ex.InnerException.Message;
            else
                message = ex.Message;

            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
