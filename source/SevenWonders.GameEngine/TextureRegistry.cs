namespace SevenWonders.GameEngine
{
    public class TextureRegistry
    {
        private readonly Dictionary<int, Texture> m_textures = new();

        public void Register(IEnumerable<Texture> textures)
        {
            foreach (var texture in textures)
            {
                m_textures[texture.Id] = texture;
            }
        }

        public void Register(Texture texture)
        {
            m_textures[texture.Id] = texture;
        }

        public Texture Get(int textureId)
        {
            return m_textures.TryGetValue(textureId, out var texture) ? texture : throw new InvalidOperationException("Cannot find the specified texture in the texture registry!");
        }

        public void Clear()
        {
            m_textures.Clear();
        }
    }
}