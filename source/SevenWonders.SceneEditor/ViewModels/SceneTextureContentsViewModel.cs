using SevenWonders.GameEngine;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SevenWonders.SceneEditor.ViewModels
{
    /// <summary>
    /// Manages the scene-level <see cref="Texture"/> assets stored in <see cref="Scene.Textures"/>.
    /// These are the raw image assets that <see cref="TextureObject"/> instances reference by Id.
    /// </summary>
    public class SceneTextureContentsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<SceneTextureListViewModel> SceneTextureViews { get; set; }

        public Scene? CurrentScene
        {
            get => m_currentScene;
            set
            {
                if (m_currentScene != value)
                {
                    m_currentScene = value;
                    SelectedSceneTexture = null;
                    SceneTextureViews.Clear();
                    if (m_currentScene is not null)
                    {
                        foreach (SceneTextureListViewModel vm in m_currentScene.Textures.Select(t => new SceneTextureListViewModel(t)))
                        {
                            SceneTextureViews.Add(vm);
                        }
                    }
                }
            }
        }

        public Texture? SelectedSceneTexture
        {
            get => m_selectedSceneTexture;
            set
            {
                m_selectedSceneTexture = value;
                OnPropertyChanged(nameof(IsSelectedSceneTextureAvailable));
                OnPropertyChanged(nameof(SelectedSceneTextureFileName));
                OnPropertyChanged(nameof(SelectedSceneTextureId));
                OnPropertyChanged(nameof(SelectedSceneTextureView));
            }
        }

        public SceneTextureListViewModel? SelectedSceneTextureView
        {
            get
            {
                if (m_selectedSceneTexture is null) return null;
                return SceneTextureViews.FirstOrDefault(t => t.Id == m_selectedSceneTexture.Id);
            }
            set
            {
                SetSelectedSceneTexture(value);
            }
        }

        public bool IsSelectedSceneTextureAvailable => SelectedSceneTexture is not null;

        public string SelectedSceneTextureFileName => SelectedSceneTexture?.FileName ?? string.Empty;

        public int SelectedSceneTextureId => SelectedSceneTexture?.Id ?? -1;

        public SceneTextureContentsViewModel(IEngine engine)
        {
            m_engine = engine;
            SceneTextureViews = new ObservableCollection<SceneTextureListViewModel>();
        }

        /// <summary>
        /// Copies the chosen image file into the scene folder, creates a <see cref="Texture"/>,
        /// registers it in the scene and its <see cref="TextureRegistry"/>.
        /// </summary>
        public void AddSceneTexture(string fullPath)
        {
            if (m_currentScene is null)
            {
                return;
            }

            string fileName = Path.GetFileName(fullPath);
            string sceneFolder = m_engine.SceneFileHandler.ReceiveSceneFolder(m_currentScene);
            string destinationFileName = Path.Combine(sceneFolder, fileName);
            if (!File.Exists(destinationFileName))
            {
                File.Copy(fullPath, destinationFileName);
            }

            Texture texture = new Texture()
            {
                FileName = fileName,
            };

            // Scene.AddTexture assigns the Id, loads the texture and registers it.
            m_engine.ObjectManager.AddTexture(m_currentScene, texture);
            SceneTextureViews.Add(new SceneTextureListViewModel(texture));
            SelectedSceneTexture = texture;
        }

        public void SetSelectedSceneTexture(SceneTextureListViewModel? listViewModel)
        {
            if (m_currentScene is null || listViewModel is null)
            {
                return;
            }

            SelectedSceneTexture = m_currentScene.Textures.FirstOrDefault(t => t.Id == listViewModel.Id);
        }

        public void DeleteSelectedSceneTexture()
        {
            if (m_currentScene is null || SelectedSceneTexture is null)
            {
                return;
            }

            SceneTextureListViewModel? vm = SceneTextureViews.FirstOrDefault(t => t.Id == SelectedSceneTexture.Id);
            if (vm is not null)
            {
                SceneTextureViews.Remove(vm);
            }

            m_currentScene.Textures.Remove(SelectedSceneTexture);
            SelectedSceneTexture = null;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private Scene? m_currentScene;
        private Texture? m_selectedSceneTexture;
        private readonly IEngine m_engine;
    }
}
