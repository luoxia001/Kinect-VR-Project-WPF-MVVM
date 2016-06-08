namespace KinectProject.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Microsoft.Speech.Recognition;
    using KinectProject.ViewModels;
    using System;
    using System.Speech.Synthesis;

    /// <summary>
    /// Interaction logic for ArticleView
    /// </summary>
    public partial class ArticleView
    {
        public static readonly DependencyProperty SelectedImageProperty = DependencyProperty.Register(
            "SelectedImage", typeof(ImageSource), typeof(ArticleView), new PropertyMetadata(default(ImageSource)));

        /// <summary>
        /// VoiceController for this window.
        /// </summary>
        public VoiceController voiceController = new VoiceController();

        /// <summary>
        /// Name of the non-transitioning visual state.
        /// </summary>
        internal const string NormalState = "Normal";

        /// <summary>
        /// Name of the fade in transition.
        /// </summary>
        internal const string FadeInTransitionState = "FadeIn";

        /// <summary>
        /// Name of the fade out transition.
        /// </summary>
        internal const string FadeOutTransitionState = "FadeOut";

        /// <summary>
        /// Instance of ArticleViewModel.
        /// </summary>
        public ArticleViewModel Article = new ArticleViewModel();

        /// <summary>
        /// Instance of SpeechSynthesizer.
        /// </summary>
        private SpeechSynthesizer Speech = new SpeechSynthesizer();

        public ArticleView()
        {
            this.InitializeComponent();
            this.voiceController.SetupSpeechRecognition();
            this.voiceController.KinectRecognizer.SpeechRecognized +=
                new EventHandler<SpeechRecognizedEventArgs>(OnVoiceCommands);
        }

        /// <summary>
        /// CLR Property Wrappers for SelectedImageProperty
        /// </summary>
        public ImageSource SelectedImage
        {
            get
            {
                return (ImageSource)GetValue(SelectedImageProperty);
            }

            set
            {
                this.SetValue(SelectedImageProperty, value);
            }
        }

        /// <summary>
        /// Close the full screen view of the image
        /// </summary>
        private void OnCloseFullImage(object sender, RoutedEventArgs e)
        {
            // Always go to normal state before a transition
            VisualStateManager.GoToElementState(OverlayGrid, NormalState, false);
            VisualStateManager.GoToElementState(OverlayGrid, FadeOutTransitionState, true);
        }

        /// <summary>
        /// Recognize voice commands and excute functions
        /// Vioce Commands:
        /// 1. Close: close full screen picture
        /// 2. Play: convert text to voice
        /// 3. Pause: pause reading
        /// 4. Resume: resume reading
        /// 5. Back: go back to homescreen
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SpeechRecognizedEventArgs</param>
        private void OnVoiceCommands(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > 0.3f)
            {
                Article = (ArticleViewModel)ArticleScrollViewer.DataContext;

                switch (e.Result.Text)
                {
                    case "Close":
                        // Always go to normal state before a transition
                        VisualStateManager.GoToElementState(OverlayGrid, NormalState, false);
                        VisualStateManager.GoToElementState(OverlayGrid, FadeOutTransitionState, true);
                        break;
                    case "Play":
                        //Convert paragraphs text to voice
                        String strSpeechText = String.Empty;
                        
                        foreach (String text in Article.Paragraphs)
                        {
                            strSpeechText += text + " ";
                        }

                        if (Speech.State != SynthesizerState.Speaking)
                        {
                            Speech.SpeakAsync(strSpeechText.Trim());
                        }
                        
                        break;
                    case "Pause":
                        if (Speech.State == SynthesizerState.Speaking)
                        {
                            Speech.Pause();
                        }
                        break;
                    case "Resume":
                        if (Speech.State == SynthesizerState.Paused)
                        {
                            Speech.Resume();
                        }
                    break;
                    case "Back":
                        Article.NavigationManager.GoBack();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Overlay the full screen view of the image
        /// </summary>
        private void OnDisplayFullImage(object sender, RoutedEventArgs e)
        {
            // Always go to normal state before a transition
            this.SelectedImage = ((ContentControl)e.OriginalSource).Content as ImageSource;
            VisualStateManager.GoToElementState(OverlayGrid, NormalState, false);
            VisualStateManager.GoToElementState(OverlayGrid, FadeInTransitionState, false);
        }

        private void ContentPresenter_Unloaded(object sender, RoutedEventArgs e)
        {
            this.voiceController = null;
            Speech.Dispose();
        }
    }
}
