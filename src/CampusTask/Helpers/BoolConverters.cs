using System.Globalization;
using CampusTask.Models;

namespace CampusTask.Helpers
{
    /// <summary>
    /// Inverse un booléen (IsEnabled = !IsBusy).
    /// </summary>
    public class InvertedBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is bool b ? !b : true;

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is bool b ? !b : false;
    }

    /// <summary>
    /// true si l'objet n'est pas null.
    /// </summary>
    public class NullToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value != null;

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// true si le statut de la tâche est Todo.
    /// </summary>
    public class IsTodoConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is AcademicTaskStatus status && status == AcademicTaskStatus.Todo;

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// true si le statut de la tâche est Completed.
    /// </summary>
    public class IsCompletedConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is AcademicTaskStatus status && status == AcademicTaskStatus.Completed;

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// Convertit un pourcentage (0-100) en valeur 0-1 pour ProgressBar.
    /// </summary>
    public class PercentageToProgressConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double d)
                return Math.Clamp(d / 100.0, 0, 1);
            if (value is int i)
                return Math.Clamp(i / 100.0, 0, 1);
            return 0.0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
