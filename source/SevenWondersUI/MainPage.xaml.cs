using System.Xml.Serialization;

namespace SevenWondersUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            CardList list = new CardList();
            XmlSerializer serializer = new XmlSerializer(typeof(CardList));

            string file = Path.Combine(Directory.GetCurrentDirectory(), "Data", "AllCards.xml");

            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                list = (CardList)serializer.Deserialize(fs);
            }
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
