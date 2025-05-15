using System.ComponentModel;
using System.Linq.Dynamic.Core;
using Zuby.ADGV;

namespace Ja2StracSaveEditor;

public class DataGridX<T> : AdvancedDataGridView
{
    private BindingList<T> _originalList = new();
    private List<T> _filteredList = new();

    public void ConfigureDatasource(BindingList<T> data)
    {
        _originalList = data ?? throw new ArgumentException("Data is null");

        _filteredList = new List<T>(_originalList);

        DataSource = _originalList;

        SortStringChanged += OnSortStringChanged;
        FilterStringChanged += OnFilterStringChanged;
    }

    private void OnFilterStringChanged(object sender, FilterEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FilterString))
        {
            DataSource = new BindingList<T>(_originalList.ToList());
            return;
        }

        try
        {
            var filterExpression = ConvertFilterString(FilterString);
            var query = _originalList.AsQueryable().Where(filterExpression);
            _filteredList = query.ToList();
            DataSource = new BindingList<T>(_filteredList);
        }
        catch (Exception ex)
        {
            Console.WriteLine($@"Filter error: {ex.Message}");
            DataSource = new BindingList<T>(_originalList.ToList());
        }
    }

    private void OnSortStringChanged(object sender, SortEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SortString)) return;

        var sortExpression = SortString.Replace("[", "").Replace("]", "");

        try
        {
            var listToSort = string.IsNullOrWhiteSpace(FilterString)
                ? _originalList.AsQueryable()
                : _filteredList.AsQueryable();

            var sortedList = listToSort.OrderBy(sortExpression).ToList();
            DataSource = new BindingList<T>(sortedList);
        }
        catch (Exception ex)
        {
            Console.WriteLine($@"Sort error: {ex.Message}");
        }
    }

    private string ConvertFilterString(string filter)
    {
        if (string.IsNullOrWhiteSpace(filter)) return "";

        filter = filter.Replace("'", "").Replace("\"", "");

        var expressions = filter
            .Replace("(", "").Replace(")", "")
            .Split(new[] { "AND" }, StringSplitOptions.RemoveEmptyEntries);

        var result = new List<string>();

        foreach (var part in expressions)
        {
            var trimmed = part.Trim();

            // Boolean equality
            if (trimmed.Contains("=") && !trimmed.Contains("IN"))
            {
                var tokens = trimmed.Split('=');
                var col = tokens[0].Trim();
                var val = tokens[1].Trim();
                result.Add($"{col} != null && {col} == {val}");
            }
            // IN clause
            else if (trimmed.Contains("IN"))
            {
                var parts = trimmed.Split(new[] { "IN" }, StringSplitOptions.None);
                var colName = GetStringBetween(parts[0], '[', ']');
                var values = parts[1].Trim(' ', '(', ')').Split(',');

                var orConditions = values.Select(v =>
                {
                    v = v.Trim();
                    return double.TryParse(v, out _) ? $"{colName} == {v}" : $"{colName}.Contains(\"{v.Trim('"')}\")";
                });

                result.Add($"{colName} != null && ({string.Join(" OR ", orConditions)})");
            }
            // Date LIKE
            else if (trimmed.Contains("LIKE") && trimmed.Contains("Convert["))
            {
                var cleaned = trimmed.Replace("Convert", "").Replace(", 'System.String'", "");
                var filters = cleaned.Split(new[] { "OR" }, StringSplitOptions.None);
                var colName = GetStringBetween(filters[0], '[', ']');

                var dateConditions = filters.Select(f =>
                {
                    var v = GetStringBetween(f, '%', '%');
                    return $"{colName}.Date == DateTime.Parse(\"{v.Trim()}\")";
                });

                result.Add($"{colName} != null && ({string.Join(" OR ", dateConditions)})");
            }
        }

        return string.Join(" AND ", result);
    }

    private string GetStringBetween(string input, char start, char end)
    {
        var startIndex = input.IndexOf(start) + 1;
        var endIndex = input.IndexOf(end, startIndex);
        return (startIndex > 0 && endIndex > startIndex)
            ? input.Substring(startIndex, endIndex - startIndex)
            : string.Empty;
    }
}
