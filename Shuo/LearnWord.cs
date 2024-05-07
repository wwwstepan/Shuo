namespace Shuo;

public class LearnWord
{
    public string Ru { get; set; } = string.Empty;
    public string Ch { get; set; } = string.Empty;

    public override string ToString() => $"{Ru} {Ch}";
}
