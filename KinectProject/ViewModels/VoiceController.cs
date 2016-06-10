using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;
using Microsoft.Kinect.Toolkit;
using KinectProject.Utilities;

namespace KinectProject.ViewModels
{

    public class VoiceController
    {
        /// <summary>
        /// Kinect Device
        /// </summary>
        private KinectSensor Kinect;

        /// <summary>
        /// Component that info of the Kinect Audio recognizer
        /// </summary>
        private RecognizerInfo KinectRecognizerInfo;

        /// <summary>
        /// Kinect Audio recognizer Engine
        /// </summary>
        public SpeechRecognitionEngine KinectRecognizer;

        /// <summary>
        /// Kinect Audio Input Source 
        /// </summary>
        public KinectAudioSource KinectSource;

        /// <summary>
        /// Audio Stream from Kinect
        /// </summary>
        private Stream AudioStream;

        public VoiceController()
        {
        }

        private RecognizerInfo findKinectRecognizerInfo()
        {
            var recognizers = SpeechRecognitionEngine.InstalledRecognizers();

            foreach (RecognizerInfo recInfo in recognizers)
            {
                // look at each recognizer info value to find the one that works for Kinect
                if (recInfo.AdditionalInfo.ContainsKey("Kinect"))
                {
                    string details = recInfo.AdditionalInfo["Kinect"];
                    if (details == "True" && recInfo.Culture.Name == "en-US")
                    {
                        // If we get here we have found the info we want to use
                        return recInfo;
                    }
                }
            }
            return null;
        }

        private Boolean CreateSpeechEngine()
        {
            KinectRecognizerInfo = findKinectRecognizerInfo();

            if (KinectRecognizerInfo == null)
            {
                return false;
            }

            try
            {
                KinectRecognizer = new SpeechRecognitionEngine(KinectRecognizerInfo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void BuildCommands()
        {
            Choices commands = new Choices();

            commands.Add("Close");
            commands.Add("Back");
            commands.Add("Play");
            commands.Add("Pause");
            commands.Add("Resume");

            GrammarBuilder grammarBuilder = new GrammarBuilder();

            grammarBuilder.Culture = KinectRecognizerInfo.Culture;
            grammarBuilder.Append(commands);

            Grammar grammar = new Grammar(grammarBuilder);

            KinectRecognizer.LoadGrammar(grammar);
        }

        private void SetupAudio()
        {
            if (KinectDeviceInstance.SensorChooser == null) {
                KinectDeviceInstance.SensorChooser = new KinectSensorChooser();
                KinectDeviceInstance.SensorChooser.Start();
            }

            if (KinectDeviceInstance.SensorChooser.Status == ChooserStatus.SensorStarted) { 
                Kinect = KinectDeviceInstance.SensorChooser.Kinect;
                KinectSource = Kinect.AudioSource;
                KinectSource.BeamAngleMode = BeamAngleMode.Automatic;
                AudioStream = KinectSource.Start();
                KinectRecognizer.SetInputToAudioStream(AudioStream, new SpeechAudioFormatInfo(
                                                        EncodingFormat.Pcm, 16000, 16, 1,
                                                        32000, 2, null));
                KinectRecognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        public void SetupSpeechRecognition()
        {
            if (CreateSpeechEngine())
            {
                BuildCommands();

                SetupAudio();
            }
        }
    }
}
