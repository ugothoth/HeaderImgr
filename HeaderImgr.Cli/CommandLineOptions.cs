using CommandLine;

namespace HeaderImgr.Cli
{
    public class CommandLineOptions
    {
        [Value(0, HelpText = "Input path of the image")]
        public string InputPath { get; set; }

        [Value(1, HelpText = "Path to output the image to")]
        public string OutputPath { get; set; }

        [Option('r', "radius", Required = false, HelpText = "The radius of the blur (defaults to 4)")]
        public int BlurRadius { get; set; }
    }
}
