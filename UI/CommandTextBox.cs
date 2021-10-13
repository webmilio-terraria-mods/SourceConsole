using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace SourceConsole.UI;

public class CommandTextBox : UITextBox
{
    public CommandTextBox(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
    {
        ShowInputTicker = false;
        IgnoresMouseInteraction = false;
    }

    public override void Click(UIMouseEvent evt)
    {
        Writing = !Writing;
    }

    public override void Update(GameTime gameTime)
    {
        if (Focused)
        {
            PlayerInput.WritingText = true;
            Main.CurrentInputTextTakerOverride = this;
        }

        if (Main.inputTextEscape)
        {
            Writing = false;
        }

        base.Update(gameTime);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        if (!Writing)
            return;

        PlayerInput.WritingText = true;

        var text = Main.GetInputText(Text);
        SetText(text);
    }

    public bool Writing { get; set; }

    public bool Focused => Writing;
}