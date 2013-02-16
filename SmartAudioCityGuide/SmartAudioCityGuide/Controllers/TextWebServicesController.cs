using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SmartAudioCityGuide.Models;
using SmartAudioCityGuide.Services;
using TranslatorService.Speech;
using System.Speech.Recognition;
using System.Speech.AudioFormat;
using System.IO;
using System.Threading;
using System.Globalization;

namespace SmartAudioCityGuide.Controllers
{
    public class TextWebServicesController : Controller
    {
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        private ILocationServices locationsServices;

        public TextWebServicesController()
        {
            locationsServices = new LocationServices(new SmartAudioCityGuideEntities());
        }

        public TextWebServicesController(ILocationServices _locationServices)
        {
            locationsServices = _locationServices;
        }
    

        public void addCommentToLocation(string streamOfComment, string latitude , string longitude)
        {
            SpeechAudioFormatInfo audioType = new SpeechAudioFormatInfo(1000,AudioBitsPerSample.Sixteen,AudioChannel.Mono);
            SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
            byte[] streamString;
            Locations location = new Locations();
            byte[] buffer = new byte[10];
            MemoryStream stream = new MemoryStream();
            using (SpeechRecognitionEngine speechRecongnizeEngine = new SpeechRecognitionEngine())
            {
                location.latitude = Convert.ToDouble(latitude);
                location.longitude = Convert.ToDouble(longitude);
                locationsServices.addLocations(location);

                streamString = serializer.Deserialize<byte[]>(streamOfComment);
                buffer = new byte[streamString.Count()];

                stream.Write(buffer, 0, buffer.Length);

                // Add a handler for the LoadGrammarCompleted event.
                speechRecongnizeEngine.LoadGrammarCompleted +=
                  new EventHandler<LoadGrammarCompletedEventArgs>(speechRecongnizeEngine_LoadGrammarCompleted);

                // Add a handler for the SpeechRecognized event.
                speechRecongnizeEngine.SpeechRecognized +=
                new EventHandler<SpeechRecognizedEventArgs>(speechRecongnizeEngine_SpeechRecognized);

                speechRecongnizeEngine.LoadGrammar(new DictationGrammar());
                speechRecongnizeEngine.SetInputToAudioStream(stream, audioType);
                speechRecongnizeEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            using (SpeechRecognizer recognizer = new SpeechRecognizer())
            {

                // Create SemanticResultValue objects that contain cities and airport codes.
                SemanticResultValue chicago = new SemanticResultValue("Chicago", "ORD");
                SemanticResultValue boston = new SemanticResultValue("Boston", "BOS");
                SemanticResultValue miami = new SemanticResultValue("Miami", "MIA");
                SemanticResultValue dallas = new SemanticResultValue("Dallas", "DFW");

                // Create a Choices object and add the SemanticResultValue objects, using
                // implicit conversion from SemanticResultValue to GrammarBuilder
                Choices cities = new Choices();
                cities.Add(new Choices(new GrammarBuilder[] { chicago, boston, miami, dallas }));

                // Build the phrase and add SemanticResultKeys.
                GrammarBuilder chooseCities = new GrammarBuilder();
                chooseCities.Append("I want to fly from");
                chooseCities.Append(new SemanticResultKey("origin", cities));
                chooseCities.Append("to");
                chooseCities.Append(new SemanticResultKey("destination", cities));

                // Build a Grammar object from the GrammarBuilder.
                Grammar bookFlight = new Grammar(chooseCities);
                bookFlight.Name = "Book Flight";

                // Add a handler for the LoadGrammarCompleted event.
                recognizer.LoadGrammarCompleted +=
                  new EventHandler<LoadGrammarCompletedEventArgs>(recognizer_LoadGrammarCompleted);

                // Add a handler for the SpeechRecognized event.
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
                // Attach event handlers for recognition events.
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(
                    SpeechRecognizedHandler);
                recognizer.EmulateRecognizeCompleted +=
                  new EventHandler<EmulateRecognizeCompletedEventArgs>(
                    EmulateRecognizeCompletedHandler);
                // Load the grammar object to the recognizer.
                recognizer.LoadGrammarAsync(bookFlight);
            }
        }

        // Handle the LoadGrammarCompleted event.
        static void speechRecongnizeEngine_LoadGrammarCompleted(object sender, LoadGrammarCompletedEventArgs e)
        {
            Console.WriteLine("Grammar loaded: " + e.Grammar.Name);
            Console.WriteLine();
        }

        // Handle the SpeechRecognized event.
        static void speechRecongnizeEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Speech recognized:  " + e.Result.Text);
            Console.WriteLine();
            Console.WriteLine("Semantic results:");
            Console.WriteLine("  The flight origin is " + e.Result.Semantics["origin"].Value);
            Console.WriteLine("  The flight destination is " + e.Result.Semantics["destination"].Value);
        }

        // Handle the SpeechRecognized event.
        static void SpeechRecognizedHandler(
          object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null)
            {
                Console.WriteLine("Recognition result = {0}",
                  e.Result.Text ?? "<no text>");
            }
            else
            {
                Console.WriteLine("No recognition result");
            }
        }

        // Handle the SpeechRecognizeCompleted event.
        static void EmulateRecognizeCompletedHandler(
          object sender, EmulateRecognizeCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                Console.WriteLine("No result generated.");
            }

        }
        // Handle the LoadGrammarCompleted event.
        static void recognizer_LoadGrammarCompleted(object sender, LoadGrammarCompletedEventArgs e)
        {
            Console.WriteLine("Grammar loaded: " + e.Grammar.Name);
            Console.WriteLine();
        }

        // Handle the SpeechRecognized event.
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Speech recognized:  " + e.Result.Text);
            Console.WriteLine();
            Console.WriteLine("Semantic results:");
            Console.WriteLine("  The flight origin is " + e.Result.Semantics["origin"].Value);
            Console.WriteLine("  The flight destination is " + e.Result.Semantics["destination"].Value);
        }
    }
}
