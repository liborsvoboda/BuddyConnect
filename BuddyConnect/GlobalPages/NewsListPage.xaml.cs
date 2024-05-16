using BuddyConnect.Functions;
using System.Collections.ObjectModel;

namespace BuddyConnect
{

    public class Monkey {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFavorite { get; set; }
    }


    public partial class NewsListPage : ContentPage, GlobalServices
    {


        private List<Monkey> source = new List<Monkey>();

        void CreateMonkeyCollection() {
            source.Add(new Monkey { Name = "Baboon", Location = "Africa & Asia", Details = "Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.", ImageUrl = "https://kliknetezde.cz/EIC&ESBdocs/EIC-Gallery/img/31.png" });
            source.Add(new Monkey { Name = "Capuchin Monkey", Location = "Central & South America", Details = "The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.", ImageUrl = "https://kliknetezde.cz/EIC&ESBdocs/EIC-Gallery/img/9.png" });
            source.Add(new Monkey { Name = "Blue Monkey", Location = "Central and East Africa", Details = "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia", ImageUrl = "https://kliknetezde.cz/EIC&ESBdocs/EIC-Gallery/img/31.png" });
            source.Add(new Monkey { Name = "Squirrel Monkey", Location = "Central & South America", Details = "The squirrel monkeys are the New World monkeys of the genus Saimiri. They are the only genus in the subfamily Saimirinae. The name of the genus Saimiri is of Tupi origin, and was also used as an English name by early researchers.", ImageUrl = "https://kliknetezde.cz/EIC&ESBdocs/ESB-Gallery/img/42.png" });
            source.Add(new Monkey { Name = "Golden Lion Tamarin", Location = "Brazil", Details = "The golden lion tamarin also known as the golden marmoset, is a small New World monkey of the family Callitrichidae.", ImageUrl = "https://kliknetezde.cz/EIC&ESBdocs/ESB-Gallery/img/42.png" });
            source.Add(new Monkey { Name = "Howler Monkey", Location = "South America", Details = "Howler monkeys are among the largest of the New World monkeys. Fifteen species are currently recognised. Previously classified in the family Cebidae, they are now placed in the family Atelidae.", ImageUrl = "https://kliknetezde.cz/EIC&ESBdocs/ESB-Gallery/img/42.png" });
            cv_carousel.ItemsSource = new ObservableCollection<Monkey>(source);
        }


        public NewsListPage() {
            InitializeComponent();
            _ = LoadStartUpData();
            CreateMonkeyCollection();
        }


        public async Task Dismiss() {
            await Navigation.PopModalAsync();
        }


        public async Task<bool> LoadStartUpData() {
            TranslatePageObjects();
            return true;
        }


        //List Of All Translated Object For Reload By LoadStartUpData()
        private void TranslatePageObjects() {
        }


    }
}
