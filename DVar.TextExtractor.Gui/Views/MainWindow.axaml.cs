using Avalonia.Controls;
using System;
using System.Collections.ObjectModel;
using System.IO;
using DVar.TextExtractor.Gui.Utils;

namespace DVar.TextExtractor.Gui.Views
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> LangDatas { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            //GetFileNamesInDirectory();
            //LangDataComboBox.SelectedItem = LangDatas[0];
        }

        private void GetFileNamesInDirectory()
        {
            string directoryPath = Data.GetTessdataDirectory();

            try
            {
                string[] files = Directory.GetFiles(directoryPath);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    LangDatas.Add(fileName.Replace(".traineddata", ""));
                }

                foreach (string data in LangDatas)
                {
                    LangDataComboBox.Items.Add(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка при получении файлов: " + e.Message);
            }
        }
    }
}