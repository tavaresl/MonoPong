// See https://aka.ms/new-console-template for more information

using MonoGame.Core.Scripts.Scenes;
using MonoGame.Persistence.Scenes;

var scene = new Gameplay();

SceneManager.Save(scene, "out.json");