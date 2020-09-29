namespace MyPlayer.Models.Classes
{
    public class RootInfo
    {  
        public string Name { get; set; }
        public string Path { get; set; }
        public RootInfo(string name, string path)
        {
            Name = name;
            Path = path;
        }
    
    }
}
