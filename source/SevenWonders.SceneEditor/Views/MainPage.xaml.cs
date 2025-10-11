using CommunityToolkit.Maui.Views;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.SceneEditor.Helpers;
using SevenWonders.SceneEditor.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.IO.Compression;
using System.Xml.Linq;

namespace SevenWonders.SceneEditor.Views
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            m_sceneFileHandler = new SceneFileHandler(new XmlHandler());
            m_sceneManager = new SceneManager();
            if (!Directory.Exists(m_sceneFileHandler.ScenesPath))
            {
                Directory.CreateDirectory(m_sceneFileHandler.ScenesPath);
            }

            foreach (Scene scene in m_sceneFileHandler.LoadScenes())
            {
                m_sceneManager.RegisterScene(scene);
            }
            m_mainPageViewModel = mainPageViewModel;
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
                                                      addTexturePopupWindow.ViewModel.Id,
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
                m_mainPageViewModel.GameObjectContentsViewModel.AddGameObjectToLayer(addGameObjectPopupWindow.ViewModel.Name, addGameObjectPopupWindow.ViewModel.Id, addGameObjectPopupWindow.ViewModel.Visible);
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
                                                                                      addSpritePopupWindow.ViewModel.Id, 
                                                                                      addSpritePopupWindow.ViewModel.Visible,
                                                                                      addSpritePopupWindow.ViewModel.TextureId,
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
                    Id = addScenePopupWindow.ViewModel.Id,
                    Visible = addScenePopupWindow.ViewModel.Visible
                };
                m_sceneManager.RegisterScene(scene);
                m_sceneFileHandler.SaveScene(scene, false);
                m_sceneFileHandler.LoadScenes();
                m_sceneManager.SetCurrentScene(scene);
                m_mainPageViewModel.SetCurrentScene(scene);
                addScenePopupWindow.ViewModel.Clear();
            }
            m_currentPopup = null;
        }

        private async void Choose_Scene_Clicked(object sender, EventArgs e)
        {
            if (m_sceneManager.Scenes.Count <= 0)
            {
                return;
            }

            ChooseScenePopupWindow chooseScenePopupWindow = new ChooseScenePopupWindow(new ChooseScenePopupWindowViewModel(m_sceneManager.Scenes));
            m_currentPopup = chooseScenePopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(chooseScenePopupWindow);
            if (chooseScenePopupWindow.ViewModel.ChooseActivated && chooseScenePopupWindow.ViewModel.SelectedScene is not null)
            {
                m_sceneManager.SetCurrentScene(chooseScenePopupWindow.ViewModel.SelectedScene);
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
                m_mainPageViewModel.LayerContentsViewModel.AddLayer(addLayerPopupWindow.ViewModel.Name, addLayerPopupWindow.ViewModel.Id, addLayerPopupWindow.ViewModel.Visible);
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
            m_sceneFileHandler.SaveScene(m_sceneManager.CurrentScene);
        }

        private readonly MainPageViewModel m_mainPageViewModel;
        private Popup? m_currentPopup;
        private Size m_currentPopupSize;

        private readonly ISceneFileHandler m_sceneFileHandler;
        private readonly ISceneManager m_sceneManager;
    }
}
