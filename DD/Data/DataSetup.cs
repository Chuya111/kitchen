using System.Collections.Generic;

namespace DD.Data
{
    public class DiffusionData
    {
        public string prompt { get; set; }
        public string negative_prompt { get; set; } = "";
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
        public List<Controlnet_unit> controlnet_units { get; set; }

        public DiffusionData(string prompt, string Nprompt, string data, int width, int height)
        {
            this.prompt = prompt;

            this.negative_prompt = Nprompt;

            this.width = width;

            this.height = height;

            Controlnet_unit cn_seg = new Controlnet_unit(data);

            Controlnet_unit cn_canny = new Controlnet_unit(data, "canny", "control_canny-fp16 [e3fe7712]");

            this.controlnet_units = new List<Controlnet_unit>() { cn_canny };
        }

    }

    public class Controlnet_unit
    {
        public string input_image { get; set; }
        public string module { get; set; } = "none";
        public string model { get; set; } = "control_seg-fp16 [b9c1cc12]";
        public double weight { get; set; } = 1.1;
        public bool lowvram { get; set; } = false;
        public int guidance { get; set; } = 1;
        public bool guessmode { get; set; } = false;

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
