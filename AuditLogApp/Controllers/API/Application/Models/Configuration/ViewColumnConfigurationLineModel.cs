public class ViewColumnConfigurationLineModel
{
    public ViewColumnConfigurationLineModel() { }

    public ViewColumnConfigurationLineModel(string field)
    {
        Field = field;
    }

    public string Field { get; set; }
}