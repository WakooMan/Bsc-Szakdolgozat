using CommunityToolkit.Maui.Views;
using SevenWonders.Common;
using SevenWonders.Game.Engine;
using SevenWonders.Game.Engine.SceneHandling;
using SevenWonders.Game.Scene.Editor.Helpers;
using SevenWonders.Game.Scene.Editor.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Configuration;
using System.Numerics;

namespace SevenWonders.Game.Scene.Editor.Views
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainPageViewModel mainPageViewModel, IEngine engine, ISceneLoader sceneLoader)
        {
            m_engine = engine;
            m_sceneLoader = sceneLoader;
            m_mainPageViewModel = mainPageViewModel;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var logFileName = ConfigurationManager.AppSettings["logFileName"];
            if (logFileName is not null)
            {
                GameLog.InitializeFileLogger(FileSystem.AppDataDirectory, logFileName);
            }

            while (m_gameView is null)
            {
                await Task.Delay(100);
            }

            foreach (Engine.SceneHandling.Scene scene in await m_sceneLoader.LoadScenes())
            {
                m_engine.SceneManager.RegisterScene(scene);
                SceneIdHandler.OrderIds(scene);
            }
            m_currentPopup = null;
            SizeChanged += MainPage_SizeChanged;
            BindingContext = m_mainPageViewModel;
            m_engine.RedrawRequested += (e, args) => m_gameView?.InvalidateSurface();
            m_engine.Startup();
        }

        private void MainPage_SizeChanged(object? sender, EventArgs e)
        {
            m_currentPopupSize = new Size(Width * 0.2, Height * 0.4);

            if (m_currentPopup is not null)
            {
                m_currentPopup.Size = m_currentPopupSize;
            }
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (m_engine.SceneManager.CurrentScene is not null && m_mainPageViewModel.LayerContentsViewModel.SelectedLayer is not null)
            {
                e.Surface.Canvas.Clear(SKColors.Black);
                m_mainPageViewModel.LayerContentsViewModel.DrawSelectedLayer(e.Surface.Canvas, m_engine.SceneManager.CurrentScene.TextureRegistry, m_width, m_height);
            }
            else if (m_mainPageViewModel.CurrentScene is not null)
            {
                m_engine.SceneManager.Render(e.Surface.Canvas);
            }
        }

        private async void Add_New_Texture_Clicked(object sender, EventArgs e)
        {
            AddTextureObjectPopupWindow addTextureObjectPopupWindow = new AddTextureObjectPopupWindow(new AddTextureObjectPopupWindowViewModel(m_mainPageViewModel.SceneTextureContentsViewModel.SceneTextureViews));
            m_currentPopup = addTextureObjectPopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(addTextureObjectPopupWindow);
            if (addTextureObjectPopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.TextureContentsViewModel.AddTextureObjectToLayer(
                    addTextureObjectPopupWindow.ViewModel.Name,
                    addTextureObjectPopupWindow.ViewModel.Visible,
                    addTextureObjectPopupWindow.ViewModel.Width,
                    addTextureObjectPopupWindow.ViewModel.Height,
                    addTextureObjectPopupWindow.ViewModel.SelectedTextureId);
                addTextureObjectPopupWindow.ViewModel.Clear();
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

        private async void Add_New_Button_Clicked(object sender, EventArgs e)
        {
            AddButtonPopupWindow addButtonPopupWindow = new AddButtonPopupWindow(new AddButtonPopupWindowViewModel());
            m_currentPopup = addButtonPopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(addButtonPopupWindow);
            if (addButtonPopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.ButtonContentsViewModel.AddButtonToLayer(addButtonPopupWindow.ViewModel.Name, addButtonPopupWindow.ViewModel.ButtonText, addButtonPopupWindow.ViewModel.FontSize , addButtonPopupWindow.ViewModel.Visible, addButtonPopupWindow.ViewModel.Width, addButtonPopupWindow.ViewModel.Height, addButtonPopupWindow.ViewModel.SelectedTextureId);
                addButtonPopupWindow.ViewModel.Clear();
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
                                                                                      addSpritePopupWindow.ViewModel.TextureId,
                                                                                      addSpritePopupWindow.ViewModel.Visible,
                                                                                      addSpritePopupWindow.ViewModel.FrameHeight,
                                                                                      addSpritePopupWindow.ViewModel.FrameWidth,
                                                                                      addSpritePopupWindow.ViewModel.Rows,
                                                                                      addSpritePopupWindow.ViewModel.Columns);
                addSpritePopupWindow.ViewModel.Clear();
            }
            m_currentPopup = null;
        }

        private async void Add_New_Scene_Texture_Clicked(object sender, EventArgs e)
        {
            AddSceneTexturePopupWindow addSceneTexturePopupWindow = new AddSceneTexturePopupWindow(new AddSceneTexturePopupWindowViewModel());
            m_currentPopup = addSceneTexturePopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(addSceneTexturePopupWindow);
            if (addSceneTexturePopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.SceneTextureContentsViewModel.AddSceneTexture(addSceneTexturePopupWindow.ViewModel.SelectedFilePath);
                addSceneTexturePopupWindow.ViewModel.Clear();
            }
        }

        private async void Add_New_Scene_Clicked(object sender, EventArgs e)
        {
            AddScenePopupWindow addScenePopupWindow = new AddScenePopupWindow(new AddPopupWindowViewModel());
            m_currentPopup = addScenePopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(addScenePopupWindow);
            if (addScenePopupWindow.ViewModel.AddActivated)
            {
                Engine.SceneHandling.Scene scene = new Engine.SceneHandling.Scene()
                {
                    Name = addScenePopupWindow.ViewModel.Name,
                    Visible = addScenePopupWindow.ViewModel.Visible
                };
                scene.Resize(new Vector2(m_width, m_height));
                m_engine.SceneManager.RegisterScene(scene);
                m_sceneLoader.SaveScene(scene, false);
                await m_sceneLoader.LoadScenes();
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
            TextureObjectsContent.IsVisible = false;
            GameObjectsContent.IsVisible = false;
            ButtonsContent.IsVisible = false;
            TextLabelsContent.IsVisible = false;

            switch (tabIndex)
            {
                case 1: LayersContent.IsVisible = true; break;
                case 2: TexturesContent.IsVisible = true; break;
                case 3: TextureObjectsContent.IsVisible = true; break;
                case 4: GameObjectsContent.IsVisible = true; break;
                case 5: ButtonsContent.IsVisible = true; break;
                case 6: TextLabelsContent.IsVisible = true; break;
            }
        }

        private void OnLayersClicked(object sender, EventArgs e) => ShowTab(1);
        private void OnTexturesClicked(object sender, EventArgs e) => ShowTab(2);
        private void OnTextureObjectsClicked(object sender, EventArgs e) => ShowTab(3);
        private void OnGameObjectsClicked(object sender, EventArgs e) => ShowTab(4);
        private void OnButtonsClicked(object sender, EventArgs e) => ShowTab(5);
        private void OnTextLabelsClicked(object sender, EventArgs e) => ShowTab(6);

        private async void Add_New_TextLabel_Clicked(object sender, EventArgs e)
        {
            AddTextLabelPopupWindow addTextLabelPopupWindow = new AddTextLabelPopupWindow(new AddTextLabelPopupWindowViewModel());
            m_currentPopup = addTextLabelPopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(addTextLabelPopupWindow);
            if (addTextLabelPopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.TextLabelContentsViewModel.AddTextLabelToLayer(
                    addTextLabelPopupWindow.ViewModel.Name,
                    addTextLabelPopupWindow.ViewModel.LabelText,
                    addTextLabelPopupWindow.ViewModel.FontSize,
                    addTextLabelPopupWindow.ViewModel.Visible,
                    addTextLabelPopupWindow.ViewModel.Width,
                    addTextLabelPopupWindow.ViewModel.Height,
                    addTextLabelPopupWindow.ViewModel.BackgroundTextureId);
                addTextLabelPopupWindow.ViewModel.Clear();
            }
            m_currentPopup = null;
        }

        private void Delete_Selected_TextLabel_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.TextLabelContentsViewModel.DeleteSelectedTextLabel();
        }

        private void Copy_Selected_TextLabel_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.TextLabelContentsViewModel.CopySelectedTextLabel();
        }

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

        private void Delete_Selected_Button_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.ButtonContentsViewModel.DeleteSelectedButton();
        }

        private void Copy_Selected_GameObject_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.GameObjectContentsViewModel.CopySelectedGameObject();
        }

        private void Copy_Selected_Layer_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.LayerContentsViewModel.CopySelectedLayer();
        }

        private void Copy_Selected_Button_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.ButtonContentsViewModel.CopySelectedButton();
        }

        private void Delete_Selected_Sprite_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.GameObjectContentsViewModel.DeleteSelectedSprite();
        }

        private void Delete_Selected_Scene_Texture_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.SceneTextureContentsViewModel.DeleteSelectedSceneTexture();
        }

        private void Save_Scene_Clicked(object sender, EventArgs e)
        {
            if (m_engine.SceneManager.CurrentScene is not null)
            {
                m_engine.SceneManager.CurrentScene.Resize(new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT));
                m_sceneLoader.SaveScene(m_engine.SceneManager.CurrentScene);
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

        private void OnTouch(object sender, SKTouchEventArgs e)
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

        private readonly ISceneLoader m_sceneLoader;
        private readonly IEngine m_engine;
    }
}
