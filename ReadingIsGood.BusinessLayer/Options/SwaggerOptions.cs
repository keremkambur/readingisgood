namespace ReadingIsGood.BusinessLayer.Options
{
    public class SwaggerOptions
    {
        public string EndPoint { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string FullName => $"{this.Name} {this.Version}";
    }
}