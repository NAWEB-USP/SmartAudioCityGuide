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
using Microsoft.Phone.UserData;
using TranslatorService.Speech;
using System.Threading;
using System.Globalization;
using SmartAudioApp.ServicesReference;
using System.Resources;

namespace SmartAudioApp
{
    public partial class HelpModeContact : PhoneApplicationPage
    {
        private MyPhone myPhone = new MyPhone();
        private MyGPS myGPS = new MyGPS();
        private Sound sound = new Sound();
        private List<Contact> contacts;
        private List<List<ListBoxItem>> listListBoxItens = new List<List<ListBoxItem>>();
        private ResourceManager resouceManager = new ResourceManager("SmartAudioApp.Resources", typeof(Resources).Assembly);
        private int currentPage = 0;
        private SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
        public HelpModeContact()
        {
            InitializeComponent();
            getContacts();
            sound.play("friendmode");
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            sound.play("menu");
        }

        public void getContacts()
        {
            Contacts cons = new Contacts();

            cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);
            cons.SearchAsync(string.Empty, FilterKind.None, null);
        }

        void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            List<ListBoxItem> listBoxItens = new List<ListBoxItem>();
            ListBoxItem listBoxItem = new ListBoxItem();
            contacts = e.Results.ToList();
            int colors = 0;
            int aux = 0;

            foreach (var result in e.Results)
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
                if (result.EmailAddresses.Count() != 0)
                {
                    listBoxItem = new ListBoxItem();
                    listBoxItem.Content = result.DisplayName;
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
                if (listListBoxItens.Count() != e.Results.Count() / 3 + 1)
                    listListBoxItens.Add(listBoxItens);
                listBoxContact.ItemsSource = listListBoxItens[0];
            }
            else
            {
                listBoxContact.ItemsSource = listBoxItens;
            }
        }

        


        void listBoxItem_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)sender;

            Contact contact = (from con in contacts
                               where con.DisplayName == listBoxItem.Content.ToString()
                               select con).FirstOrDefault();


            if (contact == null)
            {
                playSound("Contact not found.");
                return;
            }

            if (contact.EmailAddresses.Count() == 0 )
            {
                playSound("Email address not available.");
                return;
            }
            
            WebService1SoapClient webService = new WebService1SoapClient();
            updateUserLocation();
            //webService.sendEmailForFolloUserByWindowsPhoneIdAsync(myPhone.serializedDeviceUniqueId(),contact.EmailAddresses.First().EmailAddress );
            webService.sendEmailForFolloUserByWindowsPhoneIdAsync(myPhone.serializedDeviceUniqueId(), "greganatti@gmail.com");
            playSound("success");
        
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
                        listBoxContact.ItemsSource = listListBoxItens[currentPage];
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
                        listBoxContact.ItemsSource = listListBoxItens[currentPage];
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
            else playSound(listBoxItem.Content.ToString());
                   
        }

        private void playSound(string sound)
        {
            speech.SpeakAsync(sound, CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToString());
        }


        private void updateUserLocation()
        {
            string phoneId = myPhone.serializedDeviceUniqueId();
            WebService1SoapClient webService = new WebService1SoapClient();
            Thread thread = new Thread(new ThreadStart((Action)(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    webService.updateLoctionForUserAsync(phoneId, myGPS.actualLocation.Position.Location.Latitude, myGPS.actualLocation.Position.Location.Longitude);
                }
            })));

            thread.Start();
        }
    }
}