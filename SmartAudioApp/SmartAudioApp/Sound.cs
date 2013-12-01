using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Resources;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System.Globalization;

namespace SmartAudioApp
{
    public class Sound
    {
        #region .:.Prorpriedades.:.
        private SoundEffectInstance sound;
        private string culture;
        #endregion

        #region .:.Inicializadores.:.
        public Sound()
        {
            culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }
        #endregion

        #region .:.Métodos Públicos.:.
        public void play(string soundName)
        {
            try
            {
                sound.Stop();
            }
            catch (Exception)
            {
            }
            try
            {
                var stream = TitleContainer.OpenStream("Sounds/" + culture + "/" + soundName + ".wav");
                var effect = SoundEffect.FromStream(stream);
                sound = effect.CreateInstance();
                sound.Play();
            }
            catch(Exception)
            {
            }
        }
        #endregion
    }
}
