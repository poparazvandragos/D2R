using GI.Screenshot;
using Newtonsoft.Json.Linq;
using NHotkey;
using NHotkey.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace D2RInventoryTool
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
    {
        private string characterName = "";
        public string CharacterName
        {
            get => characterName;
            set
            {
                characterName = value;
                NotifyOfPropertyChange(() => CharacterName);
            }
        }

        private bool capturePlayer = true;
        public bool CapturePlayer
        {
            get => capturePlayer;
            set
            {
                capturePlayer = value;
                NotifyOfPropertyChange(() => CapturePlayer);
            }
        }

        private bool captureMerc = false;
        public bool CaptureMerc
        {
            get => captureMerc;
            set
            {
                captureMerc = value;
                NotifyOfPropertyChange(() => CaptureMerc);
            }
        }

        private bool StopProcess = false;

        private bool useMultiMonitor = false;
        public bool UseMultiMonitor
        {
            get => useMultiMonitor;
            set
            {
                useMultiMonitor = value;
                NotifyOfPropertyChange(() => UseMultiMonitor);
            }
        }

        private List<ComboBoxItem> displays;
        public List<ComboBoxItem> MonitorList
        {
            get
            {
                return displays;
            }
        }

        private ComboBoxItem monitorID;
        public ComboBoxItem MonitorID
        {
            get => monitorID;
            set
            {
                monitorID = value;
                NotifyOfPropertyChange(() => MonitorID);
            }
        }

        public ShellViewModel()
        {
            HotkeyManager.Current.AddOrReplace("AutoCapture", Key.Add, ModifierKeys.Control | ModifierKeys.Alt, AutoCapture);
            HotkeyManager.Current.AddOrReplace("CaptureScreenShot", Key.Subtract, ModifierKeys.Control | ModifierKeys.Alt, CaptureScreenShot);
            HotkeyManager.Current.AddOrReplace("ChangeCapture", Key.Multiply, ModifierKeys.Control | ModifierKeys.Alt, ChangeCapture);
            HotkeyManager.Current.AddOrReplace("TriggerStopProcess", Key.Divide, ModifierKeys.Control | ModifierKeys.Alt, TriggerStopProcess);

            displays = new List<ComboBoxItem>();
            var screens = System.Windows.Forms.Screen.AllScreens;
            foreach (var screen in screens)
            {
                displays.Add(new ComboBoxItem() { Content = screen.DeviceName, Tag = screen });
            }

            MonitorID = displays.FirstOrDefault();

            LoadJSONSettings();
        }

        private void ChangeCapture(object sender, HotkeyEventArgs e)
        {
            CaptureMerc = !CaptureMerc;
            CapturePlayer = !CaptureMerc;
        }

        private void TriggerStopProcess(object sender, HotkeyEventArgs e)
        {
            StopProcess = true;
        }

        private void LoadJSONSettings()
        {
            var jsonString = File.ReadAllText("settings.json");
            D2RHelper.settings = JObject.Parse(jsonString);
        }

        private void CaptureScreenShot(object sender, HotkeyEventArgs e)
        {
            var screen = MonitorID.Tag as System.Windows.Forms.Screen;
            try
            {
                CaptureSS(screen);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }


        private void CaptureSS(System.Windows.Forms.Screen screen)
        {
            if (string.IsNullOrWhiteSpace(CharacterName))
            {
                MessageBox.Show("Character name is empty");
                return;
            }

            


            var image = Screenshot.CaptureRegion(new Rect()
            {
                X = screen.Bounds.X,
                Y = screen.Bounds.Y,
                Width = screen.Bounds.Width,
                Height = screen.Bounds.Height
            });

            var dirPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), CharacterName, "img");
            Directory.CreateDirectory(dirPath);

            var filePath = Path.Combine(dirPath, GetImageNameFromMousePosition());
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var encoder = new JpegBitmapEncoder();
                encoder.QualityLevel = D2RHelper.settings["captureSpecs"]["quality"].Value<int>();
                var frame = BitmapFrame.Create(image.Clone());
                encoder.Frames.Add(frame);
                encoder.Save(fileStream);
            }
        }

        private void AutoCapture(object sender, HotkeyEventArgs e)
        {
            StopProcess = false;
            var screen = MonitorID.Tag as System.Windows.Forms.Screen;

            if (string.IsNullOrWhiteSpace(CharacterName))
            {
                MessageBox.Show("Character name is empty");
                return;
            }
            Task.Run(() =>
            {
                var captureWidth = D2RHelper.settings["captureSpecs"]["width"].Value<int>();
                var captureHeight = D2RHelper.settings["captureSpecs"]["height"].Value<int>();

                int cellSizeX = D2RHelper.settings["d2rScreenSpecs"]["cell"]["sizeX"].Value<int>();
                int cellSizeY = D2RHelper.settings["d2rScreenSpecs"]["cell"]["sizeY"].Value<int>();

                var specList = CaptureMerc ? D2RHelper.MercSpecs : D2RHelper.PlayerSpecs;

                MouseUtils.SetMousePosition(new Point(captureWidth / 2, captureHeight / 2));
                Thread.Sleep(100);
                try
                {
                    CaptureSS(screen);
                }
                catch { }

                foreach (var spec in specList)
                {
                    if (spec == null) continue;

                    int specStartX = spec["start"]["x"].Value<int>();
                    int specStartY = spec["start"]["y"].Value<int>();

                    int specSizeX = spec["size"]["x"].Value<int>();
                    int specSizeY = spec["size"]["y"].Value<int>();

                    for (int invX = 0; invX < specSizeX; invX++)
                        for (int invY = 0; invY < specSizeY; invY++)
                        {
                            if (StopProcess)
                            {
                                return;
                            }
                            var cellStartX = specStartX + invX * cellSizeX + invX;
                            var cellStartY = specStartY + invY * cellSizeY + invY;

                            Point mousePos = new Point(cellStartX + cellSizeX / 2, cellStartY + cellSizeY / 2);
                            //Trace.WriteLine(mousePos);
                            MouseUtils.SetMousePosition(mousePos);
                            Thread.Sleep(100);
                            try
                            {
                                CaptureSS(screen);
                            }
                            catch {
                                invY--;
                            }
                        }
                }
            });
        }

        public string GetImageNameFromMousePosition()
        {
            string imgName = "";

            var mousePosition = MouseUtils.GetCursorPosition();
            if (mousePosition.X < 0 || mousePosition.Y < 0)
            {
                throw new InvalidDataException(mousePosition.ToString());
            }

            var captureWidth = D2RHelper.settings["captureSpecs"]["width"].Value<int>();
            var captureHeight = D2RHelper.settings["captureSpecs"]["height"].Value<int>();

            int cellSizeX = D2RHelper.settings["d2rScreenSpecs"]["cell"]["sizeX"].Value<int>();
            int cellSizeY = D2RHelper.settings["d2rScreenSpecs"]["cell"]["sizeY"].Value<int>();

            var specList = CaptureMerc ? D2RHelper.MercSpecs : D2RHelper.PlayerSpecs;

            foreach (var spec in specList)
            {
                if (spec == null) continue;

                int specStartX = spec["start"]["x"].Value<int>();
                int specStartY = spec["start"]["y"].Value<int>();

                int specSizeX = spec["size"]["x"].Value<int>();
                int specSizeY = spec["size"]["y"].Value<int>();

                for (int invX = 0; invX < specSizeX; invX++)
                    for (int invY = 0; invY < specSizeY; invY++)
                    {
                        var cellStartX = specStartX + invX * cellSizeX + invX;
                        var cellStartY = specStartY + invY * cellSizeY + invY;

                        if (mousePosition.X >= cellStartX &&
                            mousePosition.X < cellStartX + cellSizeX &&
                            mousePosition.Y >= cellStartY &&
                            mousePosition.Y < cellStartY + cellSizeY
                            )
                        {
                            bool addCellIdxToImg = spec["addCellIdxToImg"].Value<bool>();
                            if (addCellIdxToImg)
                            {
                                imgName = spec["imgName"].Value<string>() + "" + invX + "" + invY + ".jpg";
                            }
                            else
                            {
                                imgName = spec["imgName"].Value<string>() + ".jpg";
                            }
                            //Trace.WriteLine("MousePosition: " + mousePosition.ToString());
                            //Trace.WriteLine("ImgName: " + imgName);
                            return imgName;
                        }
                    }
            }

            imgName = CaptureMerc ?
                D2RHelper.settings["screenSpecs"]["mercBackground"].Value<string>() + ".jpg" :
                D2RHelper.settings["screenSpecs"]["playerBackground"].Value<string>() + ".jpg";
            //Trace.WriteLine("MousePosition: " + mousePosition.ToString());
            //Trace.WriteLine("ImgName: " + imgName);
            return imgName;
        }

        public void BuildWebsite()
        {
            if (string.IsNullOrWhiteSpace(CharacterName))
            {
                MessageBox.Show("Character name is empty");
                return;
            }

            var dirPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), CharacterName);
            var zipPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Website.zip");
            dynamic shellApplication = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));

            dynamic compressedFolderContents = shellApplication.NameSpace(zipPath).Items;
            dynamic destinationFolder = shellApplication.NameSpace(dirPath);

            destinationFolder.CopyHere(compressedFolderContents);
        }

        public void LaunchWebsite()
        {
            if (string.IsNullOrWhiteSpace(CharacterName))
            {
                MessageBox.Show("Character name is empty");
                return;
            }

            var viewerPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), CharacterName, "viewer.html");

            try
            {
                System.Diagnostics.Process.Start(viewerPath);
            }
            catch
            {
            }
        }

    }
}