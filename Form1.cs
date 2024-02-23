using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Diagnostics;
using SpeechLib;
using System.Xml;
using System.Net;
using static System.Net.WebRequestMethods;

namespace ObrianAI
{
    public partial class C35AI : Form
    {
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer speech = new SpeechSynthesizer();
        System.Media.SoundPlayer music = new System.Media.SoundPlayer();

        WebClient w = new WebClient();

        bool search = false;
        bool wake = false;
        string temp;
        string condition;
        string high;
        string low;

        //static void RunTraining()
        //{
        //    SpSharedRecoContext RC = new SpSharedRecoContext();
        //    string Title = "My App's Training";
        //    ISpeechRecognizer spRecog = RC.Recognizer;
        //    spRecog.DisplayUI(hWnd, Title, SpeechLib.SpeechStringConstants.SpeechUserTraining, "");
        //}
        public C35AI()
        {
            InitializeComponent();

            speech.SelectVoiceByHints(VoiceGender.Male);

            Choices list = new Choices();
            string[] text = System.IO.File.ReadAllLines(Environment.CurrentDirectory + "//grammar.txt");
            speech.SpeakAsync(greetRandom());

             //list.Add(new string[] 
             //{ "Hi", "Hello", "open google", "listen", "C 3 5", "hey C 3 5", "open studio binder", "open visual studio"
             //   ,"visual studio","youtube","google"
             //});
           list.Add(text);
            Grammar grammar = new Grammar(new GrammarBuilder(list));
            try
            {
                recEngine.RequestRecognizerUpdate();
                recEngine.LoadGrammar(grammar);
                recEngine.SpeechRecognized += recEngine_SpeechRecognized;
                recEngine.SetInputToDefaultAudioDevice();
                recEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch { return; }

        }
        //public String GetWeather(String input)
        //{
        //    String query = String.Format("https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='nairobi or')&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
        //    XmlDocument wData = new XmlDocument();

        //    try
        //    {
        //        wData.Load(query);
        //    }
        //    catch
        //    {
        //        MessageBox.Show("No internet connection!");
        //        return "no internet";
        //    }

        //    XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);
        //    manager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

        //    XmlNode channel = wData.SelectSingleNode("query").SelectSingleNode("results").SelectSingleNode("channel");
        //    XmlNodeList nodes = wData.SelectNodes("query/results/channel");
        //    try
        //    {
        //        temp = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value;
        //        condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value;
        //        high = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["high"].Value;
        //        low = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["low"].Value;

        //        if (input == "temp")
        //        {
        //            return temp;
        //        }
        //        if (input == "high")
        //        {
        //            return high;
        //        }
        //        if (input == "low")
        //        {
        //            return low;
        //        }
        //        if (input == "cond")
        //        {
        //            return condition;
        //        }
        //    }
        //    catch
        //    {
        //        return "Error Reciving data";
        //    }
        //    return "error";
        //}
        public string greetRandom()
        {
            wake = false;
            label5.Text = "State: Sleep";

            string[] greeting = new string[5] { 
                "welcome back sir"
                , "good to see you again sir"
                , "whats todays plan sir"
                , "greetings sir "
                , "hello sir" 
            };
            Random r = new Random();
            return greeting[r.Next(greeting.Length)];

        }
        public void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            string result = e.Result.Text;
           
            if (result == "hey listen" || result == "listen" || result =="hey C 3 5" || result == "C 3 5")
            {
                wake = true;
                label5.Text = "State: Awake";
            }

            //if (result == "wake")
            //{
            //    wake = true;
            //    label5.Text = "State: Awake";
            //}

            if (result == "sleep")
            {
                wake = false; 
                label5.Text = "State: Sleep";
            }

            if (wake == true)
            {
                //if (result == "whats the weather")
                //{
                //    //if (result == "todays weather")
                //    //{
                //    //    if (result == "weather")
                //    //    {
                //    //       
                //    //    }
                //    //}
                //    speech.SpeakAsync("The sky is," + GetWeather("cond") + ".");
                //}
                //if (result == "whats the temperature")
                //{
                //    if (result == "todays temperature")
                //    {
                //        if (result == "temperature")
                //        {
                //            speech.SpeakAsync("it is," + GetWeather("temp") + "degrees.");
                //        }
                //    }

                //}

                if (result == "search for")
                {
                    search = true;
                    speech.Speak("what do you want me to look for");
                }
                if (search)
                {
                    Process.Start("https://www.google.com/search?q=" + result );
                    search = false;
                }
                //if(result == "open text reader")
                //{
                //    Form textreader = new C35AI();
                //    textreader.Show();
                //}
                if(search == false)
                {
                    try
                    {
                        switch (result)
                        {
                            case "play":
                            case "playlist":

                                SendKeys.Send(" ");
                                break;
                            case "pause":
                                SendKeys.Send(" ");
                                break;
                            case "next":
                                SendKeys.Send("^{RIGHT}");
                                break;
                            case "previous":
                                SendKeys.Send("^{LEFT}");
                                break;
                            case ("hi"):
                                speech.SpeakAsync("hi");
                                break;
                            case ("Hello"):
                                speech.SpeakAsync("hello, my name is C 3 5, how are you?");
                                break;
                            case ("open google"):
                                speech.Speak("opening google");
                                Process.Start("https://www.google.com");
                                break;
                            case "open wikipedia":
                                Process.Start("https://www.wikipedia.org");
                                break;
                            case "open mail":
                            case "open gmail":
                                Process.Start("https://mail.google.com/");
                                break;
                            case "open maps":
                            case "open google maps":
                            case "google maps":
                                Process.Start("https://maps.google.com/");
                                break;
                            case "open pintrest":
                                Process.Start("https://www.pintrest.com/");
                                break;
                            case "open studio binder":
                            case "studio binder":
                                Process.Start("https://www.studiobinder.com/");
                                Process.Start("https://www.youtube.com/@StudioBinder");
                                break;
                            case "close google chrome":
                                Process[] close = Process.GetProcessesByName("chrome");
                                foreach (Process p in close)
                                    p.Kill();
                                break;
                            case "open youtube":
                                Process.Start("https://www.youtube.com");
                                break;
                            case "my films":
                            case "films":
                                Process.Start("https://www.youtube.com/@cee35");
                                break;
                            case "pirate bay":
                            case "open pirate bay":
                                Process.Start("https://www.tpbproxypirate.com/");
                                break;
                            case ("open notepad"):
                                speech.Speak("opening notepad");
                                Process.Start("notepad.exe");
                                break;
                            case ("bye"):
                                speech.Speak("bye sir, may you have a good day");
                                this.Close();
                                break;
                            case ("fullscreen"):
                                WindowState = FormWindowState.Maximized;
                                break;
                            case ("minimize"):
                                WindowState = FormWindowState.Minimized;
                                break;
                            case ("normal"):
                                WindowState = FormWindowState.Normal;
                                break;
                            case "open command panel":
                            case "command panel":
                                Process.Start("cmd.exe");
                                break;
                            case "spotify":
                            case "open spotify":
                                Process.Start("Spotify.exe");
                                break;
                            case "open telegram":
                            case "telegram":
                                Process.Start("Telegram.exe");
                                break;
                            case "open visual studio":
                            case "visual studio":
                                //Process.Start("devenv.exe");
                                Process.Start("Code.exe");
                                break;
                            case "open v l c":
                            case "open vlc":
                            case "vlc":
                            case "v l c":
                                Process.Start("vlc.exe");
                                break;
                            case ("close this"):
                                SendKeys.Send("%{F4}");
                                break;
                            case "what's the date":
                            case "what day is it":
                            case "what is today's date":
                            case "what is today":
                            case "date today":
                            case "today":
                                speech.SpeakAsync("today is" + DateTime.Today.ToString("dddd, MMMM d, yyyy"));
                                break;
                            case "what is the time":
                            case "what time is it":
                            case "time now":
                            case "time":
                                speech.SpeakAsync("it is" + DateTime.Now.ToShortTimeString());
                                break;
                            case "stop speaking":
                            case "stop talking":
                            case "be quiet":
                            case "silence":
                            case "shut up":
                            case "stop":
                                music.Stop();
                                speech.SpeakAsyncCancelAll();
                                break;
                            case "rong turn":
                            case "turn":
                                music.SoundLocation = "rong Turn.wav";
                                music.Play();
                                speech.Speak("");
                                break;
                            case "open browser":
                            case "browser":
                                Process.Start("chrome.exe");
                                break;
                            case "open celtx":
                            case "celtx":
                                Process.Start("celtx.exe");
                                break;
                            case "open after effects":
                                Process.Start("afterfx.exe");
                                break;
                            case "open premiere pro":
                                Process.Start("adobe premiere pro.exe");
                                break;
                            case "open davinci":
                            case "davinci":
                                Process.Start("Resolve.exe");
                                break;
                            case "open photoshop":
                                Process.Start("photoshop.exe");
                                break;
                            case "open illustrator":
                            case "illustrator":
                                Process.Start("Illustrator.exe");
                                break;
                            case "open lightroom":
                            case "lightroom":
                                Process.Start("Lightroom.exe");
                                break;
                            case "open blender":
                            case "blender":
                                Process.Start("blender-launcher.exe");
                                break;
                            case "open unity":
                            case "unity":
                                Process.Start("Unity.exe");
                                break;
                            case "open hand brake":
                            case "open handbrake":
                            case "hand brake":
                                Process.Start("HandBrake.exe");
                                break;
                            case "open soundq":
                            case "soundq":
                                Process.Start("SoundQ.exe");
                                break;
                            case "open excel":
                            case "excel":
                                Process.Start("Excel.exe");
                                break;
                            case "open powerpoint":
                            case "power point":
                                Process.Start("POWERPNT.EXE");
                                break;
                            case "open word":
                            case " word":
                                Process.Start("WinWord.exe");
                                break;
                            case "shutdown":
                            case "day off":
                            case "day well spent":
                                // Process.Start("shutdown", "/s /t 0");
                                break;
                            default:
                                // speech.Speak("Command not recognized");
                                break;
                        }
                    }
                    catch
                    {
                        return;
                    }

                }
                    //if(result == "Hello")
                    //{
                    //    result = "Hello, my name is Jarvis how can I help you";
                    //}
                    //speech.SpeakAsync(result);
                    richTextBox4.Text = result;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
               richTextBox3.Text = System.IO.File.ReadAllText(openFileDialog2.FileName);
                this.Text = openFileDialog2.SafeFileName;
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            speech.SpeakAsync(richTextBox3.Text);
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textreader_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
