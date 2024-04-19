namespace GraphVisualization.GraphsUI
{
    public class CellUI : Button
    {
        public Color DefaultColor { get; init; }

        public CellUI(Color defaultColor)
        {
            DefaultColor = defaultColor;
        }
    }
}
