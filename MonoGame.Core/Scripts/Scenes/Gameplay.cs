using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;
using MonoGame.Data;
using MonoGame.Data.Collision;
using MonoGame.Data.Drawing;
using MonoGame.Data.Drawing.GUI;
using MonoGame.Data.Drawing.Sprites.Shapes;
using Score = MonoGame.Core.Scripts.Components.Score;

namespace MonoGame.Core.Scripts.Scenes;

public class Gameplay : Scene
{
    public Gameplay()
    {
        const float ballRadius = 16f;
        const float paddleSizeX = 24f;
        const float paddleSizeY = 96f;
        var paddleSize = new Vector2(paddleSizeX, paddleSizeY);

        var ball = new Entity { Transform = new Transform { Position = new Vector2(640, 360) } };
        var player = new Entity { Transform = new Transform { Position = new Vector2(128, 360) } };
        var enemy = new Entity { Transform = new Transform { Position = new Vector2(1280 - 128, 360) } };
        var score = new Entity { Transform = new Transform { Position = new Vector2(640, 20) } };
        var line = new Entity { Transform = new Transform { Position = new Vector2(640, 0) } };
        
        line.AddComponent(new DashedLineTexture
        {
            AnchorPoint = new Vector2(0.5f, 0f),
            StrokeWidth = 4,
            Length = 720
        });
        score.AddComponent(new TextLabel
        {
            FontName = "Fonts/ArialRegular72",
            AnchorPoint = new Vector2(.5f, 0)
        });
        score.AddComponent(new Score());
        player.AddComponent(new RectangleTexture { Size = paddleSize, Color = Color.White, AnchorPoint = new Vector2(0.5f, 0.5f) });
        player.AddComponent(new Paddle { Ball = ball, Size = paddleSize });
        player.AddComponent(new AabbCollider
        {
            X = -(int)paddleSize.X / 2,
            Y = -(int)paddleSize.Y / 2,
            Width = (int)paddleSize.X,
            Height = (int)paddleSize.Y
        });
        enemy.AddComponent(new RectangleTexture { Size = paddleSize, Color = Color.White, AnchorPoint = new Vector2(0.5f, 0.5f) });
        enemy.AddComponent(new Paddle { Ball = ball, Size = paddleSize });
        enemy.AddComponent(new AabbCollider
        {
            X = -(int)paddleSize.X / 2,
            Y = -(int)paddleSize.Y / 2,
            Width = (int)paddleSize.X,
            Height = (int)paddleSize.Y
        });
        ball.AddComponent(new CircleTexture { Radius = ballRadius, AnchorPoint = new Vector2(0.5f, 0.5f) });
        ball.AddComponent(new CircleCollider { Radius = ballRadius });
        ball.AddComponent(new BallController
        {
            Acceleration = 4f,
            Speed = 512f,
            Dir = Vector2.Add(Vector2.UnitY, -Vector2.UnitX),
        });
        
        AddChild(line);
        AddChild(ball);
        AddChild(player);
        AddChild(enemy);
        AddChild(score);
        
        AddComponent(new Components.Gameplay
        {
            Ball = ball,
            Player = player,
            Enemy = enemy,
            Score = score
        });
    }
}