using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using TranslatorService.Speech;
using System.Globalization;
using System.Resources;
using Microsoft.Xna.Framework.Audio;
using System.Text;

namespace SmartAudioApp
{
    public partial class ListRoute : PhoneApplicationPage
    {
        private Database database = new Database();
        private List<List<ListBoxItem>> listListBoxItens = new List<List<ListBoxItem>>();
        private ResourceManager resouceManager = new ResourceManager("SmartAudioApp.Resources", typeof(Resources).Assembly);
        private Sound sound = new Sound();
        private MyGPS myGPS = new MyGPS();
        private int currentPage = 0;
        private SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
        
        public ListRoute()
        {
            sound.play("selectlocation");
            InitializeComponent();
            System.Threading.Thread.Sleep(1000);
            getListOfPlaces();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            sound.play("routemode");
        }

        public void getListOfPlaces()
        {
            List<coordinatesAndSound> listItensDatabase = new List<coordinatesAndSound>();
            List<ListBoxItem> listBoxItens = new List<ListBoxItem>();

            listItensDatabase = database.getListItems();

            ListBoxItem listBoxItem = new ListBoxItem();
            int colors = 0;
            int aux = 0;

            if (listItensDatabase != null)
            {
                foreach (coordinatesAndSound itemForDatabase in listItensDatabase)
                {
                    if (aux == 3)
                    {
                        listBoxItem = new ListBoxItem();
                        listBoxItem.Content = resouceManager.GetString("nextPage");
                        listBoxItem.Background = new SolidColorBrush(Color.FromArgb(255, 45, 45, 45));
                        listBoxItem.Margin = new Thickness(30, 00, 20, 20);
                        listBoxItem.Width = 380;
                        listBoxItem.Height = 100;
                        listBoxItem.FontSize = 30;

                        listBoxItem.MouseEnter += new MouseEventHandler(listBoxItem_MouseEnter);
                        listBoxItem.Hold += new EventHandler<System.Windows.Input.GestureEventArgs>(listBoxItemForPage_Hold);

                        colors = 1;
                        listBoxItens.Add(listBoxItem);

                        aux = 0;
                        listListBoxItens.Add(listBoxItens);


                    }

                    if (aux == 0)
                    {
                        listBoxItens = new List<ListBoxItem>();
                        listBoxItem = new ListBoxItem();
                        listBoxItem.Content = resouceManager.GetString("previousPage");
                        listBoxItem.Background = new SolidColorBrush(Color.FromArgb(255, 16, 128, 221));
                        listBoxItem.Margin = new Thickness(30, 00, 20, 20);
                        listBoxItem.Width = 380;
                        listBoxItem.Height = 100;
                        listBoxItem.FontSize = 30;

                        listBoxItem.MouseEnter += new MouseEventHandler(listBoxItem_MouseEnter);
                        listBoxItem.Hold += new EventHandler<System.Windows.Input.GestureEventArgs>(listBoxItemForPage_Hold);

                        colors = 1;
                        listBoxItens.Add(listBoxItem);
                    }
                    if (itemForDatabase != null)
                    {
                        listBoxItem = new ListBoxItem();
                        listBoxItem.Content = itemForDatabase.id;
                        listBoxItem.Margin = new Thickness(30, 00, 20, 20);
                        listBoxItem.Width = 380;
                        listBoxItem.Height = 100;
                        listBoxItem.FontSize = 30;


                        if (colors == 1)
                        {
                            listBoxItem.Background = new SolidColorBrush(Color.FromArgb(255, 250, 150, 9));
                        }
                        else if (colors == 2)
                        {
                            listBoxItem.Background = new SolidColorBrush(Color.FromArgb(255, 229, 20, 0));
                        }
                        else if (colors == 3)
                        {
                            listBoxItem.Background = new SolidColorBrush(Color.FromArgb(255, 51, 153, 51));
                            colors = 0;
                        }

                        listBoxItem.MouseEnter += new MouseEventHandler(listBoxItem_MouseEnter);
                        listBoxItem.Hold += new EventHandler<System.Windows.Input.GestureEventArgs>(listBoxItem_Hold);

                        colors++;
                        aux++;
                        listBoxItens.Add(listBoxItem);
                    }
                }
                if (listListBoxItens.Count() != 0)
                {
                    if (listListBoxItens.Count() != listItensDatabase.Count() / 3 + 1)
                        listListBoxItens.Add(listBoxItens);
                    listBoxRoute.ItemsSource = listListBoxItens[0];
                }
                else
                {
                    listBoxRoute.ItemsSource = listBoxItens;
                }
            }
            else
            {
                sound.play("noplaces");
            }
        }

        void listBoxItem_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                ListBoxItem listBoxItem = (ListBoxItem)sender;
                coordinatesAndSound itemDatabase = database.findLocalDataBaseForLatLongAndSoundById(Convert.ToInt32(listBoxItem.Content));
                (Application.Current as App).latShared = itemDatabase.lat;
                (Application.Current as App).lonShared = itemDatabase.lon;
                sound.play("calcrota");
                NavigationService.Navigate(new Uri("/RouteMap.xaml", UriKind.Relative));
            }
            catch (Exception)
            {
                sound.play("error");
            }
        }

        void listBoxItemForPage_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)sender;


            if (listBoxItem.Content.ToString() == resouceManager.GetString("previousPage").ToString())
            {
                if (currentPage == 0)
                {
                    playSound("Page " + (currentPage + 1) + " of " + listListBoxItens.Count);
                }
                else
                {
                    currentPage = currentPage - 1;
                    if (listListBoxItens.Count > currentPage)
                    {
                        listBoxRoute.ItemsSource = listListBoxItens[currentPage];
                        playSound("Page " + (currentPage + 1) + " of " + listListBoxItens.Count);
                    }
                }
                
            }
            else if (listBoxItem.Content.ToString() == resouceManager.GetString("nextPage").ToString())
            {
                if (currentPage + 1 == listListBoxItens.Count)
                {
                    playSound("Page " + (currentPage + 1) + " of " + listListBoxItens.Count);
                }
                else
                {
                    currentPage = currentPage + 1;
                    if (listListBoxItens.Count > currentPage)
                    {
                        listBoxRoute.ItemsSource = listListBoxItens[currentPage];
                        playSound("Page " + (currentPage + 1) + " of " + listListBoxItens.Count);
                    }
                }
                return;
            }
        }

        void listBoxItem_MouseEnter(object sender, MouseEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)sender;
            if (listBoxItem.Content.ToString() == resouceManager.GetString("previousPage").ToString())
                sound.play("previouspage");
            else if (listBoxItem.Content.ToString() == resouceManager.GetString("nextPage").ToString())
                sound.play("nextpage");
            else
            {
                coordinatesAndSound itemDatabase = database.findLocalDataBaseForLatLongAndSoundById(Convert.ToInt32(listBoxItem.Content));
                byte[] soundOfText = convertStringToBytes(itemDatabase.sound);
                SoundEffect soundEffect = new SoundEffect(soundOfText, 8000, AudioChannels.Stereo);
                soundEffect.Play();
            }
        }

        private void playSound(string sound)
        {
            speech.SpeakAsync(sound, CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToString());
        }

        private byte[] convertStringToBytes(string stringResult)
        {
            UnicodeEncoding encoder = new UnicodeEncoding();
            byte[] bytes = Convert.FromBase64String(stringResult);
            return bytes;
        }


    }
}