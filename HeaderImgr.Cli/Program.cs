using CommandLine;
using HeaderImgr.ImageProcessing.System.Drawing;
using System;
using System.Drawing;

namespace HeaderImgr.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default
                .ParseArguments<CommandLineOptions>(args)
                .WithParsed(onParse);
        }

        private static void onParse(CommandLineOptions options)
        {
            try
            {
                var bitmap = new Bitmap(options.InputPath);
                new NaiveBitmapBoxBlurrer(options.BlurRadius)
                    .Apply(bitmap)
                    .Save(options.OutputPath);

                var message = string.Format(Strings.ImageSaveSuccess, options.OutputPath);
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
