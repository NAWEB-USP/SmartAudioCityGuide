﻿using System;
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
using System.Windows.Media.Imaging;

namespace SmartAudioApp
{
    public partial class ListRoute : PhoneApplicationPage
    {
        #region .:.Propriedades.:.
        private Database database = new Database();
        private List<List<ListBoxItem>> listListBoxItens = new List<List<ListBoxItem>>();
        private ResourceManager resouceManager = new ResourceManager("SmartAudioApp.Resources", typeof(Resources).Assembly);
        private Sound sound = new Sound();
        private MyGPS myGPS = new MyGPS();
        private int currentPage = 0;
        private SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
        private string itemSelected = "";
        #endregion

        #region .:.Inicializadores.:.
        public ListRoute()
        {
            sound.play("selectlocation");
            InitializeComponent();
            System.Threading.Thread.Sleep(1000);
            Menu.DoubleTap += DoubleTap;
            getListOfPlaces();
        }
        #endregion

        #region .:.Métodos Públicos.:.

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

        public void getListOfPlaces()
        {
            List<coordinatesAndSound> listItensDatabase = new List<coordinatesAndSound>();
            List<ListBoxItem> listBoxItens = new List<ListBoxItem>();

            listItensDatabase = database.getListItems();

            ListBoxItem listBoxItem = new ListBoxItem();
            int colors = 0;
            int aux = 0;
            Grid grid = new Grid();
            ColumnDefinition columDefinition = new ColumnDefinition();
            GridLength gridLength = new GridLength();
            TextBlock textBlock = new TextBlock();
            Image image = new Image();


            if (listItensDatabase != null)
            {
                foreach (coordinatesAndSound itemForDatabase in listItensDatabase)
                {
                    if (aux == 3)
                    {
                        listBoxItem = new ListBoxItem();
                        //listBoxItem.Content = resouceManager.GetString("nextPage");

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
                        //listBoxItem.Content = resouceManager.GetString("previousPage");

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
                    if (itemForDatabase != null)
                    {
                        listBoxItem = new ListBoxItem();
                        //listBoxItem.Content = itemForDatabase.id;

                        grid = new Grid();
                        columDefinition = new ColumnDefinition();
                        gridLength = new GridLength(300);
                        columDefinition.Width = gridLength;

                        textBlock = new TextBlock();
                        textBlock.Text = Convert.ToString(itemForDatabase.id);
                        textBlock.Margin = new Thickness(10, 25, 0, 25);
                        textBlock.VerticalAlignment = VerticalAlignment.Center;

                        image = new Image();
                        image.HorizontalAlignment = HorizontalAlignment.Right;
                        image.VerticalAlignment = VerticalAlignment.Center;
                        image.Height = 40;
                        image.Margin = new Thickness(10, 25, 0, 25);
                        image.Source = new BitmapImage(new Uri("Images/Icons/explore-branco.png", UriKind.Relative));

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
                        listBoxRoute.ItemsSource = listListBoxItens[currentPage];
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
                        listBoxRoute.ItemsSource = listListBoxItens[currentPage];
                        playSound("Page " + (currentPage + 1) + " of " + listListBoxItens.Count);
                    }
                }
                return;
            }
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
                        listBoxRoute.ItemsSource = listListBoxItens[currentPage];
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
                        listBoxRoute.ItemsSource = listListBoxItens[currentPage];
                        playSound("Page " + (currentPage + 1) + " of " + listListBoxItens.Count);
                    }
                }
                return;
            }
        }

        void listBoxItem_DoubleTap()
        {
            try
            {
                coordinatesAndSound itemDatabase = database.findLocalDataBaseForLatLongAndSoundById(Convert.ToInt32(itemSelected));
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
                coordinatesAndSound itemDatabase = database.findLocalDataBaseForLatLongAndSoundById(Convert.ToInt32(((listBoxItem.Content as Grid).Children[0] as TextBlock).Text));
                byte[] soundOfText = convertStringToBytes(itemDatabase.sound);
                SoundEffect soundEffect = new SoundEffect(soundOfText, 8000, AudioChannels.Stereo);
                soundEffect.Play();
                itemSelected = listBoxItem.Content.ToString();
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


        #endregion

        #region .:.Métodos Privados.:.
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            sound.play("routemode");
        }
        #endregion


    }
}