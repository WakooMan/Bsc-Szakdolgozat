using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Military;
using GameLogic.Elements.Wonders;
using SevenWonders.Common;
using System.Xml.Serialization;

namespace SevenWondersUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            IXmlHandler xmlHandler = new XmlHandler();
            IMilitaryBoard militaryBoard = new MilitaryBoardFactory(xmlHandler).Create();
            IGameElements gameElements = new GameElements(new MainCardListFactory(xmlHandler), new WonderListFactory(xmlHandler), new DevelopmentListFactory(xmlHandler));
            var sm = gameElements.Developments;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
