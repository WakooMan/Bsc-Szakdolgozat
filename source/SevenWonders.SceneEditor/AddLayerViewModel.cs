using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.SceneEditor
{
    public class AddLayerViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public AddLayerViewModel()
        {
            m_layerName = string.Empty;
        }

        public string LayerName
        {
            get
            {
                return m_layerName;
            }
            set
            {
                m_layerName = value;
                OnPropertyChanged();
            }
        }

        public int LayerId
        {
            get
            {
                return m_layerId;
            }
            set
            {
                m_layerId = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private string m_layerName;
        private int m_layerId;
    }
}
