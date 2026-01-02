using CommunityToolkit.Maui.Views;
using Serilog;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.SceneEditor.ViewModels;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Numerics;

namespace SevenWonders.SceneEditor.Views
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            IInputManager inputManager = new InputManager();
            m_sceneFileHandler = new SceneFileHandler(new XmlHandler());
            m_engine = new Engine(new SceneManager(), inputManager, new ObjectManager(inputManager, m_sceneFileHandler), m_sceneFileHandler);
            if (!Directory.Exists(m_sceneFileHandler.ScenesPath))
            {
                Directory.CreateDirectory(m_sceneFileHandler.ScenesPath);
            }

            foreach (Scene scene in m_sceneFileHandler.LoadScenes())
            {
                m_engine.SceneManager.RegisterScene(scene);
            }
            m_mainPageViewModel = new MainPageViewModel(m_engine);
            m_currentPopup = null;
            SizeChanged += MainPage_SizeChanged;
            BindingContext = m_mainPageViewModel;
            new Thread(() =>
            {
                while (canvas is not null)
                {
                    canvas?.InvalidateSurface();
                    Thread.Sleep(500);
                }
            }).Start();
        }

        private void MainPage_SizeChanged(object? sender, EventArgs e)
        {
            m_currentPopupSize = new Size(Width * 0.2, Height * 0.4);

            if (m_currentPopup is not null)
            {
                m_currentPopup.Size = m_currentPopupSize;
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear();
            if (m_mainPageViewModel.LayerContentsViewModel.SelectedLayer is not null)
            {
                m_mainPageViewModel.LayerContentsViewModel.DrawSelectedLayer(e);
            }
            else if (m_mainPageViewModel.CurrentScene is not null)
            {
                m_mainPageViewModel.CurrentScene.Draw(e);
            }
        }

        private void Layer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            m_mainPageViewModel.LayerContentsViewModel.SetSelectedLayer(e.SelectedItem as LayerListViewModel);
        }

        private void Texture_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            m_mainPageViewModel.TextureContentsViewModel.SetSelectedTexture(e.SelectedItem as TextureListViewModel);
        }

        private void GameObject_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            m_mainPageViewModel.GameObjectContentsViewModel.SetSelectedGameObject(e.SelectedItem as GameObjectListViewModel);
        }

        private void Sprite_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            m_mainPageViewModel.GameObjectContentsViewModel.SetSelectedSprite(e.SelectedItem as SpriteListViewModel);
        }

        private async void Add_New_Texture_Clicked(object sender, EventArgs e)
        {
            AddTexturePopupWindow addTexturePopupWindow = new AddTexturePopupWindow(new AddTexturePopupWindowViewModel());
            m_currentPopup = addTexturePopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(addTexturePopupWindow);
            if (addTexturePopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.TextureContentsViewModel.AddTextureToLayer(addTexturePopupWindow.ViewModel.Name,
                                                      addTexturePopupWindow.ViewModel.Visible,
                                                      addTexturePopupWindow.ViewModel.TextureId,
                                                      addTexturePopupWindow.ViewModel.Width,
                                                      addTexturePopupWindow.ViewModel.Height,
                                                      addTexturePopupWindow.ViewModel.SelectedFilePath);
                addTexturePopupWindow.ViewModel.Clear();
            }
            m_currentPopup = null;
        }

        private async void Add_New_GameObject_Clicked(object sender, EventArgs e)
        {
            AddGameObjectPopupWindow addGameObjectPopupWindow = new AddGameObjectPopupWindow(new AddPopupWindowViewModel());
            m_currentPopup = addGameObjectPopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(addGameObjectPopupWindow);
            if (addGameObjectPopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.GameObjectContentsViewModel.AddGameObjectToLayer(addGameObjectPopupWindow.ViewModel.Name, addGameObjectPopupWindow.ViewModel.Visible);
                addGameObjectPopupWindow.ViewModel.Clear();
            }
            m_currentPopup = null;
        }

        private async void Add_New_Sprite_Clicked(object sender, EventArgs e)
        {
            AddSpritePopupWindow addSpritePopupWindow = new AddSpritePopupWindow(new AddSpritePopupWindowViewModel());
            m_currentPopup = addSpritePopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(addSpritePopupWindow);
            if (addSpritePopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.GameObjectContentsViewModel.AddSpriteToGameObject(addSpritePopupWindow.ViewModel.Name, 
                                                                                      addSpritePopupWindow.ViewModel.TextureName,
                                                                                      addSpritePopupWindow.ViewModel.Visible,
                                                                                      addSpritePopupWindow.ViewModel.Width,
                                                                                      addSpritePopupWindow.ViewModel.Height,
                                                                                      addSpritePopupWindow.ViewModel.SelectedFilePath,
                                                                                      addSpritePopupWindow.ViewModel.FrameHeight,
                                                                                      addSpritePopupWindow.ViewModel.FrameWidth,
                                                                                      addSpritePopupWindow.ViewModel.Rows,
                                                                                      addSpritePopupWindow.ViewModel.Columns);
                addSpritePopupWindow.ViewModel.Clear();
            }
            m_currentPopup = null;
        }

        private async void Add_New_Scene_Clicked(object sender, EventArgs e)
        {
            AddScenePopupWindow addScenePopupWindow = new AddScenePopupWindow(new AddPopupWindowViewModel());
            m_currentPopup = addScenePopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(addScenePopupWindow);
            if (addScenePopupWindow.ViewModel.AddActivated)
            {
                Scene scene = new Scene()
                {
                    Name = addScenePopupWindow.ViewModel.Name,
                    Visible = addScenePopupWindow.ViewModel.Visible
                };
                scene.Resize(new Vector2(m_width, m_height));
                m_engine.SceneManager.RegisterScene(scene);
                m_sceneFileHandler.SaveScene(scene, false);
                m_sceneFileHandler.LoadScenes();
                m_engine.SceneManager.SetCurrentScene(scene);
                m_mainPageViewModel.SetCurrentScene(scene);
                addScenePopupWindow.ViewModel.Clear();
            }
            m_currentPopup = null;
        }

        private async void Choose_Scene_Clicked(object sender, EventArgs e)
        {
            if (m_engine.SceneManager.Scenes.Count <= 0)
            {
                return;
            }

            ChooseScenePopupWindow chooseScenePopupWindow = new ChooseScenePopupWindow(new ChooseScenePopupWindowViewModel(m_engine.SceneManager.Scenes));
            m_currentPopup = chooseScenePopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(chooseScenePopupWindow);
            if (chooseScenePopupWindow.ViewModel.ChooseActivated && chooseScenePopupWindow.ViewModel.SelectedScene is not null)
            {
                chooseScenePopupWindow.ViewModel.SelectedScene.Resize(new Vector2(m_width, m_height));
                m_engine.SceneManager.SetCurrentScene(chooseScenePopupWindow.ViewModel.SelectedScene);
                m_mainPageViewModel.SetCurrentScene(chooseScenePopupWindow.ViewModel.SelectedScene);
                chooseScenePopupWindow.ViewModel.Clear();
            }
            m_currentPopup = null;

        }

        private async void Add_New_Layer_Clicked(object sender, EventArgs e)
        {
            AddLayerPopupWindow addLayerPopupWindow = new AddLayerPopupWindow(new AddPopupWindowViewModel());
            m_currentPopup = addLayerPopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(addLayerPopupWindow);
            if (addLayerPopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.LayerContentsViewModel.AddLayer(addLayerPopupWindow.ViewModel.Name, addLayerPopupWindow.ViewModel.Visible);
                addLayerPopupWindow.ViewModel.Clear();
            }
            m_currentPopup = null;
        }

        private void ShowTab(int tabIndex)
        {
            LayersContent.IsVisible = false;
            TexturesContent.IsVisible = false;
            GameObjectsContent.IsVisible = false;

            switch (tabIndex)
            {
                case 1: LayersContent.IsVisible = true; break;
                case 2: TexturesContent.IsVisible = true; break;
                case 3: GameObjectsContent.IsVisible = true; break;
            }
        }

        private void OnLayersClicked(object sender, EventArgs e) => ShowTab(1);
        private void OnTexturesClicked(object sender, EventArgs e) => ShowTab(2);
        private void OnGameObjectsClicked(object sender, EventArgs e) => ShowTab(3);
        private void Delete_Selected_Layer_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.LayerContentsViewModel.DeleteSelectedLayer();
        }

        private void Delete_Selected_Texture_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.TextureContentsViewModel.DeleteSelectedTexture();
        }

        private void Delete_Selected_GameObject_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.GameObjectContentsViewModel.DeleteSelectedGameObject();
        }

        private void Copy_Selected_GameObject_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.GameObjectContentsViewModel.CopySelectedGameObject();
        }

        private void Copy_Selected_Layer_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.LayerContentsViewModel.CopySelectedLayer();
        }

        private void Delete_Selected_Sprite_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.GameObjectContentsViewModel.DeleteSelectedSprite();
        }

        private void Save_Scene_Clicked(object sender, EventArgs e)
        {
            if (m_engine.SceneManager.CurrentScene is not null)
            {
                m_engine.SceneManager.CurrentScene.Resize(new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT));
                m_sceneFileHandler.SaveScene(m_engine.SceneManager.CurrentScene);
            }
        }

        private void OnCanvasSizeChanged(object sender, EventArgs e)
        {
            Grid? grid = sender as Grid;
            if (grid != null)
            {
                m_width = (float)grid.Width;
                m_height = (float)grid.Height;
                if (m_mainPageViewModel.CurrentScene is not null)
                {
                    m_mainPageViewModel.CurrentScene.Resize(new Vector2(m_width, m_height));
                    m_mainPageViewModel.UpdateCanvasSize();
                }
            }
        }

        private void OnTouchEffectAction(object sender, SKTouchEventArgs e)
        {
            m_engine.InputManager.OnTouchEvent(e);
        }

        private readonly MainPageViewModel m_mainPageViewModel;
        private Popup? m_currentPopup;
        private Size m_currentPopupSize;
        private float m_width = 1600;
        private float m_height = 900;
        private const float DEFAULT_WIDTH = 3840;
        private const float DEFAULT_HEIGHT = 2160;

        private readonly ISceneFileHandler m_sceneFileHandler;
        private readonly IEngine m_engine;
    }
}
