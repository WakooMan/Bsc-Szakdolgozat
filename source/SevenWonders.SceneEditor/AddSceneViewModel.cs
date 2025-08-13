using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SevenWonders.SceneEditor
{
    public class AddSceneViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public AddSceneViewModel()
        {
            m_sceneName = string.Empty;
        }

        public string SceneName
        {
            get
            {
                return m_sceneName;
            }
            set
            {
                m_sceneName = value;
                OnPropertyChanged();
            }
        }

        public int SceneId
        {
            get
            {
                return m_sceneId;
            }
            set
            {
                m_sceneId = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private string m_sceneName;
        private int m_sceneId;
    }
}
