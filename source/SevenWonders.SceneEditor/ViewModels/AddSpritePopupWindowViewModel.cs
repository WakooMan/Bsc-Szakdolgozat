using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SevenWonders.SceneEditor.ViewModels
{
    public class AddSpritePopupWindowViewModel: AddTexturePopupWindowViewModel
    {
        public int FrameWidth
        {
            get
            {
                return m_frameWidth;
            }
            set
            {
                m_frameWidth = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int FrameHeight
        {
            get
            {
                return m_frameHeight;
            }
            set
            {
                m_frameHeight = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int Rows
        {
            get
            {
                return m_rows;
            }
            set
            {
                m_rows = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int Columns
        {
            get
            {
                return m_columns;
            }
            set
            {
                m_columns = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public string TextureName
        {
            get
            {
                return m_textureName;
            }
            set
            {
                m_textureName = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public AddSpritePopupWindowViewModel() : base()
        {
            m_textureName = string.Empty;
        }

        private int m_frameWidth;
        private int m_frameHeight;
        private int m_rows;
        private int m_columns;
        private string m_textureName;
        
    }
}
