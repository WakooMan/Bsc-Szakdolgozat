using SevenWonders.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.SceneEditor.Helpers
{
    public static class SceneIdHandler
    {
        public static void OrderIds(Scene scene)
        {
            int currentId = 0;
            scene.Textures.ForEach(texture =>
            {
                int oldId = texture.Id;
                int newId = currentId;
                texture.Id = newId;

                scene.Layers.ForEach(layer =>
                {
                    foreach (ButtonObject buttonObject in layer.ButtonObjects)
                    {
                        if (buttonObject.BackgroundTextureId == oldId)
                        {
                            buttonObject.BackgroundTextureId = newId;
                        }
                    }

                    foreach (TextLabel textLabel in layer.TextLabels)
                    {
                        if (textLabel.BackgroundTextureId == oldId)
                        {
                            textLabel.BackgroundTextureId = newId;
                        }
                    }

                    foreach (TextureObject textureObject in layer.TextureObjects)
                    {
                        if (textureObject.TextureId == oldId)
                        {
                            textureObject.TextureId = newId;
                        }
                    }

                    foreach (GameObject gameObject in layer.GameObjects)
                    {
                        gameObject.Animations.ForEach(animation =>
                        {
                            animation.Frames.ForEach(frame =>
                            {
                                if (frame.TextureId == oldId)
                                {
                                    frame.TextureId = newId;
                                }
                            });
                        });
                    }
                });

                scene.InitializeTextureRegistry();
                currentId++;
            });
            scene.Layers.ForEach(layer =>
            {
                layer.Id = currentId++;
                foreach (SceneObject sceneObject in layer.SceneObjects)
                {
                    sceneObject.Id = currentId++;
                }
            });
        }
    }
}
