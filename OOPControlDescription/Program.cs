using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPControlDescription
{
    class File : IComparable
    {
        public string Name { get; set; }
        public string Size { get; set; }

        public int ISize
        {
            get
            {
                string s = "";
                foreach (char c in Size)
                    if (char.IsDigit(c))
                        s += c;
                return int.Parse(s);
            }
        }

        public string Extension
        {
            get
            {
                string[] parts = Name.Split('.');
                return parts[parts.Length - 1];
            }
        }

        public void Parse(string str)
        {
            // Text:file.txt(6B);Some string content
            var data = str.Split(':')[1]; // file.txt(6B);Some string content
            ParseInternal(data.Split(';'));
        }

        protected virtual void ParseInternal(string[] data)
        {
            // [0] = file.txt(6B)
            var parts = data[0].Split('(', ')'); // [0] = file.txt [1] = 6B
            Name = parts[0];
            Size = parts[1];
        }

        public override string ToString()
        {
            return $@"  {Name}
        Extension: {Extension}
        Size: {Size}";
        }

        public int CompareTo(object obj)
        {
            var f = (File)obj;

            return ISize - f.ISize;
        }
    }
    
    class TextFile : File
    {
        public string Content { get; set; }

        protected override void ParseInternal(string[] data)
        {
            base.ParseInternal(data);
            Content = data[1];
        }

        public override string ToString()
        {
            return base.ToString() + $@"
        Content: {Content}";
        }
    }

    class ImageFile : File
    {
        public string Resolution { get; set; }

        protected override void ParseInternal(string[] data)
        {
            base.ParseInternal(data);
            Resolution = data[1];
        }

        public override string ToString()
        {
            return base.ToString() + $@"
        Resolution: {Resolution}";
        }
    }

    class MovieFile : ImageFile
    {
        public string Length { get; set; }

        protected override void ParseInternal(string[] data)
        {
            base.ParseInternal(data);
            Length = data[2];
        }

        public override string ToString()
        {
            return base.ToString() + $@"
        Length: {Length}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string text = @"Text:file.txt(6B);Some string content
Image:img.bmp(19MB);1920х1080
Text:data.txt(12B);Another string
Text:data1.txt(7B);Yet another string
Movie:logan.2017.mkv(19GB);1920х1080;2h12m";

            var strs = text.Split('\n');
            File[] result = new File[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                string type = strs[i].Split(':')[0];
                File file = null;
                switch (type)
                {
                    case "Image":
                        file = new ImageFile();
                        break;
                    case "Text":
                        file = new TextFile();
                        break;
                    case "Movie":
                        file = new MovieFile();
                        break;
                }
                file.Parse(strs[i]);
                result[i] = file;
            }

            Array.Sort(result);

            Console.WriteLine("Text files:");
            foreach (var item in result)
            {
                if (item is TextFile)
                    Console.WriteLine(item);
            }

            Console.WriteLine("Movies:");
            foreach (var item in result)
            {
                if (item is MovieFile)
                    Console.WriteLine(item);
            }

            Console.WriteLine("Images:");
            foreach (var item in result)
            {
                if (item is ImageFile && item is MovieFile == false)
                    Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
