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
using System.Windows.Media.Imaging;

namespace SmartAudioApp
{
    public partial class HelpModeContact : PhoneApplicationPage
    {
        #region .:.Propriedades.:.
        private MyPhone myPhone = new MyPhone();
        private MyGPS myGPS = new MyGPS();
        private Sound sound = new Sound();
        private string itemSelected = "";
        private List<Contact> contacts;
        private List<List<ListBoxItem>> listListBoxItens = new List<List<ListBoxItem>>();
        private ResourceManager resouceManager = new ResourceManager("SmartAudioApp.Resources", typeof(Resources).Assembly);
        private int currentPage = 0;
        private SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
        #endregion

        #region .:.Inicializadores.:.
        public HelpModeContact()
        {
            InitializeComponent();
            getContacts();
            sound.play("friendmode");
            Menu.DoubleTap += DoubleTap;
        }
        #endregion

        #region .:.Métodos Protected.:.
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            sound.play("menu");
        }
        #endregion

        #region .:.Métodos Privados.:.
        private void DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (itemSelected == "previousPage")
            {
                listBoxItem_Page_DoubleTap();
            }
            else if (itemSelected == "nextPage")
            {
                listBoxItem_Page_DoubleTap();
            }
            else
            {
                listBoxItem_DoubleTap();
            }
        }


        void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            List<ListBoxItem> listBoxItens = new List<ListBoxItem>();
            ListBoxItem listBoxItem = new ListBoxItem();
            contacts = e.Results.ToList();
            int colors = 0;
            int aux = 0;
            int itensOnPage = 0;
            Grid grid = new Grid();
            ColumnDefinition columDefinition = new ColumnDefinition();
            GridLength gridLength = new GridLength();
            TextBlock textBlock = new TextBlock();
            Image image = new Image();

            foreach (var result in e.Results)
            {
                if (aux == 3)
                {
                    listBoxItem = new ListBoxItem();
                    //listBoxItem.Content = resouceManager.GetString("nextPage").ToLower();

                    grid = new Grid();
                    columDefinition = new ColumnDefinition();
                    gridLength = new GridLength(300);
                    columDefinition.Width = gridLength;

                    textBlock = new TextBlock();
                    textBlock.Text = resouceManager.GetString("nextPage").ToLower();
                    textBlock.Margin = new Thickness(10, 25, 0, 25);
                    textBlock.VerticalAlignment = VerticalAlignment.Center;

                    image = new Image();
                    image.HorizontalAlignment = HorizontalAlignment.Right;
                    image.VerticalAlignment = VerticalAlignment.Center;
                    image.Height = 40;
                    image.Margin = new Thickness(10, 25, 0, 25);
                    image.Source = new BitmapImage(new Uri("Images/Icons/next-icon.png", UriKind.Relative));

                    columDefinition = new ColumnDefinition();
                    gridLength = new GridLength(350);
                    columDefinition.Width = gridLength;
                    grid.ColumnDefinitions.Add(columDefinition);

                    columDefinition = new ColumnDefinition();
                    gridLength = new GridLength(80);
                    columDefinition.Width = gridLength;
                    grid.ColumnDefinitions.Add(columDefinition);

                    grid.Children.Add(textBlock);
                    grid.Children.Add(image);

                    listBoxItem.Content = grid;

                    listBoxItem.Background = HexToSolidColorBrush("FA6800");
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
                    //listBoxItem.Content = resouceManager.GetString("previousPage").ToLower();

                    grid = new Grid();
                    columDefinition = new ColumnDefinition();
                    gridLength = new GridLength(300);

                    columDefinition.Width = gridLength;

                    textBlock = new TextBlock();
                    textBlock.Text = resouceManager.GetString("previousPage").ToLower();
                    textBlock.Margin = new Thickness(10, 25, 0, 25);
                    textBlock.VerticalAlignment = VerticalAlignment.Center;

                    image = new Image();
                    image.HorizontalAlignment = HorizontalAlignment.Right;
                    image.VerticalAlignment = VerticalAlignment.Center;
                    image.Height = 40;
                    image.Margin = new Thickness(10, 25, 0, 25);
                    image.Source = new BitmapImage(new Uri("Images/Icons/previous-icon.png", UriKind.Relative));

                    columDefinition = new ColumnDefinition();
                    gridLength = new GridLength(350);
                    columDefinition.Width = gridLength;
                    grid.ColumnDefinitions.Add(columDefinition);

                    columDefinition = new ColumnDefinition();
                    gridLength = new GridLength(80);
                    columDefinition.Width = gridLength;
                    grid.ColumnDefinitions.Add(columDefinition);

                    grid.Children.Add(textBlock);
                    grid.Children.Add(image);

                    listBoxItem.Content = grid;

                    listBoxItem.Background = HexToSolidColorBrush("1BA1E2");
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
                    //listBoxItem.Content = result.DisplayName.ToLower();

                    grid = new Grid();

                    textBlock = new TextBlock();
                    textBlock.Text = result.DisplayName.ToLower();
                    textBlock.Margin = new Thickness(10, 25, 0, 25);
                    textBlock.VerticalAlignment = VerticalAlignment.Center;

                    image = new Image();
                    image.HorizontalAlignment = HorizontalAlignment.Right;
                    image.VerticalAlignment = VerticalAlignment.Center;
                    image.Height = 50;
                    image.Margin = new Thickness(10, 25, 0, 25);
                    image.Source = new BitmapImage(new Uri("Images/Icons/friend-branco.png", UriKind.Relative));

                    columDefinition = new ColumnDefinition();
                    gridLength = new GridLength(350);
                    columDefinition.Width = gridLength;
                    grid.ColumnDefinitions.Add(columDefinition);

                    columDefinition = new ColumnDefinition();
                    gridLength = new GridLength(80);
                    columDefinition.Width = gridLength;
                    grid.ColumnDefinitions.Add(columDefinition);

                    grid.Children.Add(textBlock);
                    grid.Children.Add(image);

                    listBoxItem.Content = grid;

                    listBoxItem.Margin = new Thickness(30, 00, 20, 20);
                    listBoxItem.Width = 380;
                    listBoxItem.Height = 100;
                    listBoxItem.FontSize = 30;


                    if (colors == 1)
                    {
                        listBoxItem.Background = HexToSolidColorBrush("F0A30A");
                    }
                    else if (colors == 2)
                    {
                        listBoxItem.Background = HexToSolidColorBrush("D80073");
                    }
                    else if (colors == 3)
                    {
                        listBoxItem.Background = HexToSolidColorBrush("60A817");
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
                /*
                while (listBoxContact.ItemsSource.GetEnumerator().MoveNext())
                    itensOnPage++;

                playSound("This page has  " + (itensOnPage) + " itens.");
                 * */
            }
            else
            {
                listBoxContact.ItemsSource = listBoxItens;
                playSound("This page has  " + (listBoxItens.Count) + " itens.");
            }
        }

        void listBoxItem_Page_DoubleTap()
        {
            if (itemSelected == "previousPage")
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
            else if (itemSelected == "nextPage")
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

        void listBoxItem_DoubleTap()
        {
            Contact contact = (from con in contacts
                               where con.DisplayName == itemSelected
                               select con).FirstOrDefault();
            if (contact == null)
            {
                playSound("Contact not found.");
                return;
            }

            if (contact.EmailAddresses.Count() == 0)
            {
                playSound("Email address not available.");
                return;
            }

            WebService1SoapClient webService = new WebService1SoapClient();
            updateUserLocation();
            //webService.sendEmailForFolloUserByWindowsPhoneIdAsync(myPhone.serializedDeviceUniqueId(),contact.EmailAddresses.First().EmailAddress );

            //Moq
            webService.sendEmailForFolloUserByWindowsPhoneIdAsync(myPhone.serializedDeviceUniqueId(), "greganatti@gmail.com");
            playSound("success");
        }


        void listBoxItem_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)sender;

            Contact contact = (from con in contacts
                               where con.DisplayName.ToLower() == ((listBoxItem.Content as Grid).Children[0] as TextBlock).Text
                               select con).FirstOrDefault();


            if (contact == null)
            {
                playSound("Contact not found.");
                return;
            }

            if (contact.EmailAddresses.Count() == 0)
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

            if (((listBoxItem.Content as Grid).Children[0] as TextBlock).Text == resouceManager.GetString("previousPage").ToString())
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
            else if (((listBoxItem.Content as Grid).Children[0] as TextBlock).Text == resouceManager.GetString("nextPage").ToString())
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
            if (((listBoxItem.Content as Grid).Children[0] as TextBlock).Text == resouceManager.GetString("previousPage").ToString())
            {
                sound.play("previouspage");
                itemSelected = "previousPage";
            }
            else if (((listBoxItem.Content as Grid).Children[0] as TextBlock).Text == resouceManager.GetString("nextPage").ToString())
            {
                sound.play("nextpage");
                itemSelected = "nextPage";
            }
            else
            {
                playSound(((listBoxItem.Content as Grid).Children[0] as TextBlock).Text);
                itemSelected = ((listBoxItem.Content as Grid).Children[0] as TextBlock).Text;
            }

        }

        public static SolidColorBrush HexToSolidColorBrush(object value)
        {
            byte alpha;
            byte pos = 0;

            string hex = value.ToString().Replace("#", "");

            if (hex.Length == 8)
            {
                alpha = System.Convert.ToByte(hex.Substring(pos, 2), 16);
                pos = 2;
            }
            else
            {
                alpha = System.Convert.ToByte("ff", 16);
            }

            byte red = System.Convert.ToByte(hex.Substring(pos, 2), 16);

            pos += 2;
            byte green = System.Convert.ToByte(hex.Substring(pos, 2), 16);

            pos += 2;
            byte blue = System.Convert.ToByte(hex.Substring(pos, 2), 16);

            return new SolidColorBrush(Color.FromArgb(alpha, red, green, blue));
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

        #endregion

        #region .:.Métodos Públicos.:.
        public void getContacts()
        {
            Contacts cons = new Contacts();

            cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);
            cons.SearchAsync(string.Empty, FilterKind.None, null);
        }
        #endregion
    }
}