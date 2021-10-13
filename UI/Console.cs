using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using WebmilioCommons.UI;

namespace SourceConsole.UI;

public class Console : StandardUIState
{
    public static Color RootGreen { get; } = new(78, 90, 70);
    public static Color DarkGreen { get; } = new(53, 57, 47);

    private CommandTextBox _commandBox;

    public Console()
    {
        BuildInterface();
    }

    public void BuildInterface()
    {
        RemoveAllChildren();

        const float
            leftPadding = 0.25f,
            topPadding = 0.15f;

        UIElement root = new()
        {
            Left = new(0, leftPadding),
            Top = new(0, topPadding),

            Width = new(0, 1),
            Height = new(0, 1),
        };
        Append(root);

        {
            UIPanel mainPanel = new()
            {
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill,

                BackgroundColor = RootGreen,
            };
            root.Append(mainPanel);

            UITextPanel<string> history = new("History Here", textScale: 1f)
            {
                Width = StyleDimension.Fill,
                Height = new(-60, 1),
                
                BackgroundColor = DarkGreen,
                BorderColor = Color.Transparent,

                TextHAlign = 0,
                OverflowHidden = true
            };
            mainPanel.Append(history);

            for (int i = 0; i < 5; i++)
                history.SetText(history.Text + "\nCommand");

            UIElement bottomPanel = new()
            {
                Width = StyleDimension.Fill,
                Height = new StyleDimension(50, 0),

                VAlign = 1
            };
            mainPanel.Append(bottomPanel);

            _commandBox = new("", textScale: 1.5f)
            {
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill,

                VAlign = 1,
                BackgroundColor = DarkGreen,
                BorderColor = Color.Transparent,
            };
            bottomPanel.Append(_commandBox);

            SubmitButton submitButton = new()
            {
                Width = new(-10, 0.15f),
                Height = StyleDimension.Fill,

                HAlign = 1,

                BackgroundColor = DarkGreen,
                BorderColor = Color.Black,

                PressBackgroundColor = Color.DarkGray,
                PressBorderColor = Color.White,
            };

            bottomPanel.Append(submitButton);
        }
    }

    public override void OnActivate()
    {
        
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (ContainsPoint(Main.MouseScreen))
        {
            Main.LocalPlayer.mouseInterface = true;
        }
    }
}