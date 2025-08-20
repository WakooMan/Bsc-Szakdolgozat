using CommunityToolkit.Maui.Views;
using SevenWonders.SceneEditor.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace SevenWonders.SceneEditor.Views
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
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
            m_mainPageViewModel.TextureContentsViewModel.DrawSelectedLayer(e);
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

        private void Add_New_GameObject_Clicked(object sender, EventArgs e)
        {
        }

        private async void Add_New_Scene_Clicked(object sender, EventArgs e)
        {
            AddScenePopupWindow addScenePopupWindow = new AddScenePopupWindow(new AddPopupWindowViewModel());
            m_currentPopup = addScenePopupWindow;
            m_currentPopup.Size = m_currentPopupSize;
            await this.ShowPopupAsync(addScenePopupWindow);
            if (addScenePopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.SetCurrentScene(addScenePopupWindow.ViewModel.Name, addScenePopupWindow.ViewModel.Id, addScenePopupWindow.ViewModel.Visible);
                addScenePopupWindow.ViewModel.Clear();
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

        private readonly MainPageViewModel m_mainPageViewModel;
        private Popup? m_currentPopup;
        private Size m_currentPopupSize;

        private void Delete_Selected_Layer_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.LayerContentsViewModel.DeleteSelectedLayer();
        }

        private void Delete_Selected_Texture_Clicked(object sender, EventArgs e)
        {
            m_mainPageViewModel.TextureContentsViewModel.DeleteSelectedTexture();
        }
    }
}
