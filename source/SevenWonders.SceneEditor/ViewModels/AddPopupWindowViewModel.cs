using SkiaSharp;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SevenWonders.SceneEditor.ViewModels
{
    public class AddPopupWindowViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool AddActivated { get; set; }
        public ICommand OnAddCommand => m_onAddCommand;
        public ICommand OnBackCommand { get; set; }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                m_Id = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public bool Visible
        {
            get
            {
                return m_visible;
            }
            set
            {
                m_visible = value;
                OnPropertyChanged();
            }
        }


        public AddPopupWindowViewModel()
        {
            m_Name = string.Empty;
            m_visible = true;
            AddActivated = false;
            OnBackCommand = new Command(Clear);
            m_onAddCommand = new Command(OnAddCommandExecute, CanExecuteAdd);
        }

        public virtual void Clear()
        {
            Name = string.Empty;
            Id = 0;
            m_visible = true;
            AddActivated = false;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected virtual bool CanExecuteAdd()
        {
            return !string.IsNullOrEmpty(m_Name) && m_Id >= 0;
        }

        private void OnAddCommandExecute()
        {
            AddActivated = true;
        }

        protected Command m_onAddCommand;
        private string m_Name;
        private int m_Id;
        private bool m_visible;
    }
}
