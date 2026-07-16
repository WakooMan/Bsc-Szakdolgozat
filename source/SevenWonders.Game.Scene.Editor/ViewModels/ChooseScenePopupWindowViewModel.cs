using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SevenWonders.Game.Scene.Editor.ViewModels
{
    public class ChooseScenePopupWindowViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public bool ChooseActivated { get; set; }
        public ICommand OnChooseCommand => m_onChooseCommand;
        public ICommand OnBackCommand { get; set; }
        public ObservableCollection<Engine.SceneHandling.Scene> Scenes { get; }
        public Engine.SceneHandling.Scene? SelectedScene
        {
            get
            {
                return m_selectedScene;
            }
            set
            {
                m_selectedScene = value;
                OnPropertyChanged();
                m_onChooseCommand.ChangeCanExecute();
            }
        }

        public ChooseScenePopupWindowViewModel(IReadOnlyList<Engine.SceneHandling.Scene> scenes)
        {
            Scenes = new ObservableCollection<Engine.SceneHandling.Scene>(scenes);
            m_onChooseCommand = new Command(OnChooseCommandExecute, CanExecuteChoose);
            OnBackCommand = new Command(Clear);
            ChooseActivated = false;
            SelectedScene = null;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void Clear()
        {
            SelectedScene = null;
            Scenes.Clear();
            ChooseActivated = false;
        }

        protected virtual bool CanExecuteChoose()
        {
            return SelectedScene is not null;
        }

        private void OnChooseCommandExecute()
        {
            ChooseActivated = true;
        }

        private Command m_onChooseCommand;
        private Engine.SceneHandling.Scene? m_selectedScene;

    }
}
