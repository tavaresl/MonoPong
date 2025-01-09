using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components.Paddles;
using MonoGame.Core.Scripts.Entities;
using MonoGame.Data;
using MonoGame.Data.Collision;
using MonoGame.Data.Components.Drawables.Textures.Shapes;
using Score = MonoGame.Core.Scripts.Entities.Score;

namespace MonoGame.Core.Scripts.Scenes;

public class Gameplay : Scene
{
    public Gameplay()
    {
        var ball = new Ball();
        var player = new Player { Ball = ball };
        var enemy = new Enemy { Ball = ball };
        var score = new Score();
        var line = new Entity { Transform = new Transform { Position = new Vector2(640, 0) }};
        var paddleSize = new Vector2(24, 96);
        
        line.AddComponent(new DashedLineTexture
        {
            AnchorPoint = new Vector2(0.5f, 0f),
            StrokeWidth = 4,
            Length = 720
        });
        score.AddComponent(new Components.Score());
        player.AddComponent(new RectangleTexture { Size = paddleSize, Color = Color.White, AnchorPoint = new Vector2(0.5f, 0.5f) });
        player.AddComponent(new PaddleController { Ball = ball, Size = paddleSize, Handler = new PlayerControl()});
        player.AddComponent(new AabbCollider
        {
            X = (int)(player.Transform.Position.X - paddleSize.X / 2),
            Y = (int)(player.Transform.Position.X - paddleSize.X / 2),
            Width = (int)paddleSize.X,
            Height = (int)paddleSize.Y
        });
        enemy.AddComponent(new RectangleTexture { Size = paddleSize, Color = Color.White, AnchorPoint = new Vector2(0.5f, 0.5f) });
        enemy.AddComponent(new PaddleController { Ball = ball, Size = paddleSize, Handler = new EnemyControl()});
        enemy.AddComponent(new AabbCollider
        {
            X = (int)(player.Transform.Position.X - paddleSize.X / 2),
            Y = (int)(player.Transform.Position.X - paddleSize.X / 2),
            Width = (int)paddleSize.X,
            Height = (int)paddleSize.Y
        });
        ball.AddComponent(new CircleTexture { Radius = ball.Radius, AnchorPoint = new Vector2(0.5f, 0.5f) });
        ball.AddComponent(new CircleCollider{ Radius = ball.Radius });
        
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