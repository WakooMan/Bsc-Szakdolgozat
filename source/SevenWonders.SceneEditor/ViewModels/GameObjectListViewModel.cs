using SevenWonders.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.SceneEditor.ViewModels
{
    public class GameObjectListViewModel: BaseListViewModel
    {
        public GameObjectListViewModel(GameObject gameObject)
        {
            m_gameObject = gameObject;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Id));
        }

        public override string Name => m_gameObject.Name;

        public override int Id => m_gameObject.Id;

        private readonly GameObject m_gameObject;
    }
}
