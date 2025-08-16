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

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            m_mainPageViewModel.DrawSelectedLayer(e);
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            m_mainPageViewModel.SetSelectedLayer(e.SelectedItem as LayerViewModel);
        }

        private async void Add_New_Texture_Clicked(object sender, EventArgs e)
        {
            AddTexturePopupWindow addTexturePopupWindow = new AddTexturePopupWindow(new AddTexturePopupWindowViewModel());
            await this.ShowPopupAsync(addTexturePopupWindow);
            if (addTexturePopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.AddTextureToLayer(addTexturePopupWindow.ViewModel.Name,
                                                      addTexturePopupWindow.ViewModel.Id,
                                                      addTexturePopupWindow.ViewModel.Visible,
                                                      addTexturePopupWindow.ViewModel.TextureId,
                                                      addTexturePopupWindow.ViewModel.Width,
                                                      addTexturePopupWindow.ViewModel.Height,
                                                      addTexturePopupWindow.ViewModel.SelectedFilePath);
                addTexturePopupWindow.ViewModel.AddActivated = false;
            }
        }

        private void Add_New_GameObject_Clicked(object sender, EventArgs e)
        {
        }

        private async void Add_New_Scene_Clicked(object sender, EventArgs e)
        {
            AddScenePopupWindow addScenePopupWindow = new AddScenePopupWindow(new AddPopupWindowViewModel());
            await this.ShowPopupAsync(addScenePopupWindow);
            if (addScenePopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.SetCurrentScene(addScenePopupWindow.ViewModel.Name, addScenePopupWindow.ViewModel.Id, addScenePopupWindow.ViewModel.Visible);
                addScenePopupWindow.ViewModel.Clear();
            }
        }

        private async void Add_New_Layer_Clicked(object sender, EventArgs e)
        {
            AddLayerPopupWindow addLayerPopupWindow = new AddLayerPopupWindow(new AddPopupWindowViewModel());
            await this.ShowPopupAsync(addLayerPopupWindow);
            if (addLayerPopupWindow.ViewModel.AddActivated)
            {
                m_mainPageViewModel.AddLayer(addLayerPopupWindow.ViewModel.Name, addLayerPopupWindow.ViewModel.Id, addLayerPopupWindow.ViewModel.Visible);
                addLayerPopupWindow.ViewModel.AddActivated = false;
            }
        }

        private readonly MainPageViewModel m_mainPageViewModel;
    }
}
