using CommunityToolkit.Maui.Views;
using SevenWonders.SceneEditor.ViewModels;

namespace SevenWonders.SceneEditor.Views;

public partial class AddSpritePopupWindow : Popup
{
    public AddSpritePopupWindowViewModel ViewModel => m_viewModel;
    public AddSpritePopupWindow(AddSpritePopupWindowViewModel addSpritePopupWindowViewModel)
	{
		InitializeComponent();
		m_viewModel = addSpritePopupWindowViewModel;
        BindingContext = m_viewModel;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void FilePicker_Clicked(object sender, EventArgs e)
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

        m_viewModel.SelectedFileName = result.FileName;
        m_viewModel.SelectedFilePath = result.FullPath;
    }


    private async Task<FileResult?> PickAndShow(PickOptions options)
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

    private readonly AddSpritePopupWindowViewModel m_viewModel;
}