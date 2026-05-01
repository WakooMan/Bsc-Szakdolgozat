using SevenWonders.Game.Engine;

namespace SevenWonders.Game.Presenter
{
    public class TextureIdHandler : ITextureIdHandler
    {
        public TextureIdHandler(ISceneManager sceneManager)
        {
            m_sceneManager = sceneManager;
        }
        public int GetTextureId(string textureName)
        {
            if (m_sceneManager.CurrentScene is not null)
            {
                Texture? texture = m_sceneManager.CurrentScene.Textures.Find(texture => Path.GetFileNameWithoutExtension(texture.FileName).ToLower() == textureName.ToLower());
                if (texture is null)
                {
                    throw new InvalidOperationException($"Texture with name '{textureName}' not found");
                }

                return texture.Id;
            }

            throw new InvalidOperationException("CurrentScene is null!");
        }

        private readonly ISceneManager m_sceneManager;
    }
}
