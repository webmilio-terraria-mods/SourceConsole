using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SourceConsole.UI;

public class SubmitButton : UITextPanel<string>
{
    private Color[] _colors = new Color[2];

    public SubmitButton() : base("Submit", textScale: 1.5f)
    {
    }

    public override void MouseDown(UIMouseEvent evt)
    {
        _colors[0] = BackgroundColor;
        _colors[1] = BorderColor;

        BackgroundColor = PressBackgroundColor;
        BorderColor = PressBorderColor;
    }

    public override void MouseUp(UIMouseEvent evt)
    {
        BackgroundColor = _colors[0];
        BorderColor = _colors[1];
    }

    public override void Update(GameTime gameTime)
    {
        if (IsMouseHovering)
            Main.LocalPlayer.mouseInterface = true;
    }

    public Color PressBackgroundColor { get; set; }
    public Color PressBorderColor { get; set; }
}