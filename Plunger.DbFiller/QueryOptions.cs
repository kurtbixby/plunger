namespace Plunger.DbFiller;

public class QueryOptions
{
    private HashSet<string> Fields { get; set; } = new HashSet<string>();
    private Dictionary<string, string> WhereOptions { get; set; } = new Dictionary<string, string>();
    private Dictionary<string, string> OtherOptions { get; set; } = new Dictionary<string, string>();

    public string QueryString()
    {
        var fields = string.Join(", ", Fields);
        var where = string.Join(" & ", WhereOptions.Select(kvp => $"{kvp.Key} {kvp.Value}"));
        var other = string.Join("; ", OtherOptions.Select(kvp => $"{kvp.Key} {kvp.Value}"));

        return $"fields {fields}; where {where}; {other};";
    }

    public void AddField(string field)
    {
        Fields.Add(field);
    }

    public void AddFields(IEnumerable<string> fields)
    {
        Fields.UnionWith(fields);
    }

    public void AddWhereOption(string field, string value)
    {
        WhereOptions.Add(field, value);
    }
    
    public void AddOtherOption(string field, string value)
    {
        OtherOptions.Add(field, value);
    }
}