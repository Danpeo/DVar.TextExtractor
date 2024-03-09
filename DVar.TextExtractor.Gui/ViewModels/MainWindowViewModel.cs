using ReactiveUI;
using System;
using System.IO;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DVar.TextExtractor.Gui.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVar.TextFromImage;
using Avalonia.Platform;
using Avalonia;
using System.Reflection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Collections.ObjectModel;

namespace DVar.TextExtractor.Gui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<string> LangDatas { get; set; } = new();


        private string _selectedLang;

        public string SelectedLang
        {
            get => _selectedLang;
            set => this.RaiseAndSetIfChanged(ref _selectedLang, value);
        }

        public ICommand OpenImageCommand { get; }
        public ICommand ExtractTextCommand { get; }
        public ICommand CopyTextCommand { get; }

        private string _extractedText = string.Empty;

        public string ExtractedText
        {
            get => _extractedText;
            set => this.RaiseAndSetIfChanged(ref _extractedText, value);
        }

        private Bitmap? _image;

        public Bitmap? Image
        {
            get => _image;
            set => this.RaiseAndSetIfChanged(ref _image, value);
        }

        private IStorageFile? _file;

        public MainWindowViewModel()
        {
            GetFileNamesInDirectory();
            if (LangDatas.Any())
                _selectedLang = LangDatas[0];
            OpenImageCommand = ReactiveCommand.Create(OpenImageAsync);
            ExtractTextCommand = ReactiveCommand.Create(ExtractTextAsync);
            CopyTextCommand = ReactiveCommand.Create(async () =>
            {
                var w = new Window();

                if (w.Clipboard != null)
                    await w.Clipboard.SetTextAsync(ExtractedText);
            });
        }

        private Task ExtractTextAsync()
        {
            var tessdataDirectory = Data.GetTessdataDirectory();

            if (_file != null)
            {
                string? path = _file.TryGetLocalPath();
                if (!string.IsNullOrEmpty(path))
                    ExtractedText = Extractor.ExtractTextFromFile(path, tessdataDirectory,
                        SelectedLang);
            }

            return Task.CompletedTask;
        }

        private async Task OpenImageAsync()
        {
            var w = new Window();

            var files = await w.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Image File",
                AllowMultiple = false,
                FileTypeFilter = new[] { FilePickerFileTypes.ImageAll },
            });

            if (files.Count >= 1)
            {
                try
                {
                    _file = files[0];
                    string? path = _file.TryGetLocalPath();
                    if (!string.IsNullOrEmpty(path))
                        Image = new Bitmap(path);
                }
                catch (FileNotFoundException)
                {
                    var box = MessageBoxManager
                        .GetMessageBoxStandard("Image Not Found!", "There is no such file!!!");

                    await box.ShowAsync();
                }
                catch (Exception ex)
                {
                    var box = MessageBoxManager
                        .GetMessageBoxStandard($"{ex.Source}", $"{ex.Message}");

                    await box.ShowAsync();
                }
            }
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
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка при получении файлов: " + e.Message);
            }
        }
    }
}