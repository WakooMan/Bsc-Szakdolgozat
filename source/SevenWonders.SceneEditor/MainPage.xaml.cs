using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace SevenWonders.SceneEditor
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            m_mainPageViewModel = mainPageViewModel;
            BindingContext = m_mainPageViewModel;
            new Thread(() => {
                while (true)
                {
                    canvas.InvalidateSurface();
                    Thread.Sleep(500);
                }
            }).Start();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            m_mainPageViewModel.DrawSelectedLayer(e);
        }

        private readonly MainPageViewModel m_mainPageViewModel;

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            m_mainPageViewModel.SetSelectedLayer(e.SelectedItem as LayerViewModel);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".png", ".jpg" } }, // file extension
                });

            PickOptions options = new()
            {
                PickerTitle = "Please select a picture",
                FileTypes = customFileType,
            };
            var result = await PickAndShow(options);
            if (result is null)
            {
                return;
            }

            m_mainPageViewModel.AddTextureToLayer(result.FullPath, result.FileName);
        }

        private async Task<FileResult> PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        using var stream = await result.OpenReadAsync();
                        var image = ImageSource.FromStream(() => stream);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return null;
        }
    }
}
