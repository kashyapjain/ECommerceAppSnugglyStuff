using Plugin.ImageEdit;
using SkiaSharp;
using SnugglyStuff.Controller;
using SnugglyStuff.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TouchTracking;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnugglyStuff.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemView : ContentPage
    {
        public static Item item { get; set; }
        public static string _AddText { get; set; }

        string path = Path.Combine(FileSystem.AppDataDirectory,
                                     item.ID.ToString() + "Final.png");
        SKImage image;

        // Bitmap and matrix for display

        SKBitmap Basebitmap;
        static SKBitmap Topbitmap;
        SKSurface surface;
        SKCanvas canvas;

        SKMatrix matrix = SKMatrix.MakeIdentity();

        // Touch information

        long touchId = -1;
        SKPoint previousPoint;

        Dictionary<long, SKPoint> touchDictionary = new Dictionary<long, SKPoint>();

        public ItemView()
        {
            InitializeComponent();

            try
            {
                var bytes = ApiController.GetImageBytes(item.ImageSrc).Result;

                using(Stream stream = new MemoryStream(bytes))
                {
                    Basebitmap = SKBitmap.Decode(stream);

                    Basebitmap = Basebitmap.Resize(new SKSizeI(Convert.ToInt32(Math.Floor(Basebitmap.Width * 2.0)),
                                                       Convert.ToInt32(Math.Floor(Basebitmap.Height * 2.0))),
                                                       SKFilterQuality.High);
                }
            }
            catch(Exception ex)
            {

            }
        }

        public async Task<Stream> UploadFile(Item item)
        {
            try
            {
                var photo = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Png,
                    PickerTitle = "Select an PNG Image"
                });

                var stream = photo.OpenReadAsync().Result;
                
                return stream;
                
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                return null;
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            try
            {

                SKImageInfo info = args.Info;
                surface = args.Surface;
                canvas = surface.Canvas;

                canvas.Clear();

                // Display the bitmap

                canvas.DrawBitmap(Basebitmap, new SKPoint());
                canvas.SetMatrix(matrix);

                if (Topbitmap != null)
                {
                    canvas.DrawBitmap(Topbitmap, new SKPoint());
                }

                image = surface.Snapshot();
            }
            catch (Exception ex)
            {

            }
        }

        private void OnTouchEffectAction(object sender, TouchTracking.TouchActionEventArgs args)
        {
            try
            {
                // Convert Xamarin.Forms point to pixels

                var pt = args.Location;

                SKPoint point =
                    new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                                (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));

                switch (args.Type)
                {
                    case TouchActionType.Pressed:
                        if (!touchDictionary.ContainsKey(args.Id))
                        {
                            // Invert the matrix
                            if (matrix.TryInvert(out SKMatrix inverseMatrix))
                            {
                                // Transform the point using the inverted matrix
                                SKPoint transformedPoint = inverseMatrix.MapPoint(point);

                                // Check if it's in the untransformed bitmap rectangle
                                SKRect rect = new SKRect(0, 0, Topbitmap.Width, Topbitmap.Height);

                                if (rect.Contains(transformedPoint))
                                {
                                    touchDictionary.Add(args.Id, point);
                                }
                            }
                        }
                        break;

                    case TouchActionType.Moved:
                        if (touchDictionary.ContainsKey(args.Id))
                        {
                            // Single-finger drag
                            if (touchDictionary.Count == 1)
                            {
                                SKPoint prevPoint = touchDictionary[args.Id];

                                // Adjust the matrix for the new position
                                matrix.TransX += point.X - prevPoint.X;
                                matrix.TransY += point.Y - prevPoint.Y;
                                canvasView.InvalidateSurface();
                            }
                            // Double-finger rotate, scale, and drag
                            else if (touchDictionary.Count >= 2)
                            {
                                // Copy two dictionary keys into array
                                long[] keys = new long[touchDictionary.Count];
                                touchDictionary.Keys.CopyTo(keys, 0);

                                // Find index non-moving (pivot) finger
                                int pivotIndex = (keys[0] == args.Id) ? 1 : 0;

                                // Get the three points in the transform
                                SKPoint pivotPoint = touchDictionary[keys[pivotIndex]];
                                SKPoint prevPoint = touchDictionary[args.Id];
                                SKPoint newPoint = point;

                                // Calculate two vectors
                                SKPoint oldVector = prevPoint - pivotPoint;
                                SKPoint newVector = newPoint - pivotPoint;

                                // Find angles from pivot point to touch points
                                float oldAngle = (float)Math.Atan2(oldVector.Y, oldVector.X);
                                float newAngle = (float)Math.Atan2(newVector.Y, newVector.X);

                                // Calculate rotation matrix
                                float angle = newAngle - oldAngle;
                                SKMatrix touchMatrix = SKMatrix.MakeRotation(angle, pivotPoint.X, pivotPoint.Y);

                                // Effectively rotate the old vector
                                float magnitudeRatio = Magnitude(oldVector) / Magnitude(newVector);
                                oldVector.X = magnitudeRatio * newVector.X;
                                oldVector.Y = magnitudeRatio * newVector.Y;

                                // Isotropic scaling!
                                float scale = Magnitude(newVector) / Magnitude(oldVector);

                                if (!float.IsNaN(scale) && !float.IsInfinity(scale))
                                {
                                    SKMatrix.PostConcat(ref touchMatrix,
                                        SKMatrix.MakeScale(scale, scale, pivotPoint.X, pivotPoint.Y));

                                    SKMatrix.PostConcat(ref matrix, touchMatrix);
                                    canvasView.InvalidateSurface();
                                }
                            }

                            // Store the new point in the dictionary
                            touchDictionary[args.Id] = point;
                        }

                        break;

                    case TouchActionType.Released:
                    case TouchActionType.Cancelled:
                        if (touchDictionary.ContainsKey(args.Id))
                        {
                            touchDictionary.Remove(args.Id);
                        }
                        break;
                }
    
            }
            catch(Exception ex)
            {

            }
        }

        float Magnitude(SKPoint point)
        {
            return (float)Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2));
        }

        private async void AddLogo(object sender, EventArgs e)
        {
            try
            {
                var stream = await UploadFile(item);

                ItemView.Topbitmap = SKBitmap.Decode(stream);

                Shell.Current.Navigation.PopAsync();

                var route = $"{nameof(ItemView)}";

                Shell.Current.GoToAsync(route);
            }
            catch(Exception ex)
            {

            }
        }

        private void AddTextMethod(object sender, EventArgs e)
        {
            try
            {
                SKBitmap TextBitmap;
                using (SKPaint textPaint = new SKPaint { TextSize = 75 })
                {
                    SKRect bounds = new SKRect();
                    textPaint.MeasureText(AddText.Text, ref bounds);

                    TextBitmap = new SKBitmap((int)bounds.Right,
                                               (int)bounds.Height);

                    using (SKCanvas bitmapCanvas = new SKCanvas(TextBitmap))
                    {
                        bitmapCanvas.Clear();
                        bitmapCanvas.DrawText(AddText.Text, 0, -bounds.Top, textPaint);
                    }
                }

                ItemView.Topbitmap = TextBitmap;

                var route = $"{nameof(ItemView)}";

                Shell.Current.GoToAsync(route);
            }
            catch(Exception ex)
            {

            }
        }

        private async void PlaceOrder(object sender, EventArgs e)
        {
            try
            {
                var data = image.Encode(SKEncodedImageFormat.Png, 80);
                //Create  Stream
                var stream = File.OpenWrite(path);
                //Save Stream
                data.SaveTo(stream);
                stream.Close();
                //Crop ScreenShot
                var ImageToCrop = await CrossImageEdit.Current.CreateImageAsync(File.OpenRead(path));

                int width = ImageToCrop.Width;
                int height = Basebitmap.Height;

                ImageToCrop.Crop(0,0,width, height);

                //Save Croped ScreenShot

                var bytes = ImageToCrop.ToPng();

                File.WriteAllBytes(path, bytes);

                item.ImageSrc = path;

                PlaceOrderView.item = item;


                var route = $"{nameof(PlaceOrderView)}";

                Shell.Current.GoToAsync(route);
            }
            catch(Exception ex)
            {

            }
        }
    }
}