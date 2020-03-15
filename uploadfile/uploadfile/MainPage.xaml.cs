using Plugin.FilePicker;
using Plugin.FileUploader;
using Plugin.FileUploader.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace uploadfile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private MediaFile _mediaFile;
        private string URL { get; set; }
        public MainPage()
        {
            InitializeComponent();
        }

        //Picture choose from device    
        private async void btnSelectPic_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
            }
            else
            {
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                if (_mediaFile == null) return;
                imageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
                UploadedUrl.Text = "Image URL:";
            }
        }

        //Upload picture button    
        private async void btnUpload_Clicked(object sender, EventArgs e)
        {
            if (_mediaFile == null)
            {
                await DisplayAlert("Error", "There was an error when trying to get your image.", "OK");
                return;
            }
            else
            {
                UploadImage(_mediaFile.GetStream());
            }
        }

        //Take picture from camera    
        private async void btnTakePic_Clicked(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":(No Camera available.)", "OK");
                return;
            }
            else
            {
                _mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "myImage.jpg"
                });

                if (_mediaFile == null) return;
                imageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                UploadedUrl.Text = "Image URL:";
            }
        }

        public void Busy()
        {
            uploadIndicator.IsVisible = true;
            uploadIndicator.IsRunning = true;
            btnSelectPic.IsEnabled = false;
            btnTakePic.IsEnabled = false;
            btnUpload.IsEnabled = false;
        }

        public void NotBusy()
        {
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
            btnSelectPic.IsEnabled = true;
            btnTakePic.IsEnabled = true;
            btnUpload.IsEnabled = true;
        }
    



        private async void UploadImage(Stream stream)
        {
            Busy();
            //upload to server here 
            NotBusy();
            await DisplayAlert("Uploaded", "Image uploaded Successfully!", "OK");
        }

        private async void btnSelectFile_Clicked(object sender, EventArgs e)
        {
          var file =await  CrossFilePicker.Current.PickFile();
            // file ready
        }
    }
}
