using System.Collections.Generic;

namespace DD.Service
{
    public class DiffusionData
    {
        public string prompt { get; set; } = "Rendering floor plan of the apartment " +
            "layout,top view,white background,masterpiece, best quality, extremely detailed," +
            "best illustration, best shadow,";
        public string negative_prompt { get; set; } = "cluttered,paintings," +
            " (worst quality:2), (low quality:2), (normal quality:2).lowres,signature," +
            " blurry, drawing, sketch, poor quality, ugly, text, pixelatedow resolution," +
            " chaotic spaces,outdted design, cluttered,paintings, (worst quality2)," +
            " (low quality2),(normal quality 2), lowres,signature, blury, drawing, sketch, " +
            "pooiquality, ugly, text, pixelated, low resolution, chaotic spaces,outdated design,";
        public bool save_images { get; set; } = true;
        public int seed { get; set; } = -1;
        public int subseed { get; set; } = -1;
        public double subseed_strength { get; set; } = 0;
        public int batch_size { get; set; } = 1;
        public int n_iter { get; set; } = 1;
        public int steps { get; set; } = 20;
        public double cfg_scale { get; set; } = 7.5;
        public int width { get; set; } = 256;
        public int height { get; set; } = 256;
        public bool restore_faces { get; set; } = true;
        public int eta { get; set; } = 0;
        public string sampler_index { get; set; } = "DPM++ 2M Karras";
        public Controlnet alwayson_scripts { get; set; }

        public DiffusionData(string data)
        {

            this.alwayson_scripts = new Controlnet()
            {
                controlnet = new Args()
                {
                    args = new List<Controlnet_unit>
                {
                    new Controlnet_unit(data)
                },
                }
            };

        }

    }

    public class Args
    {
        public List<Controlnet_unit> args { get; set; }

    }

    public class Controlnet
    {
        public Args controlnet { get; set; }

    }

    public class Controlnet_unit
    {
        public string input_image { get; set; }
        public string module { get; set; } = "canny";
        public string model { get; set; } = "control_v11p_sd15_lineart [43d4be0d]";

        public Controlnet_unit(string data)
        {
            this.input_image = data;
        }

        public Controlnet_unit(string data, string module, string model)
        {
            this.input_image = data;
            this.module = module;
            this.model = model;
        }
    }
}
