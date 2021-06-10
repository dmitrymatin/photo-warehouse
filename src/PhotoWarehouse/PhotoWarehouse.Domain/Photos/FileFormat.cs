namespace PhotoWarehouse.Domain.Photos
{
    public class FileFormat
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $".{Name}";
        }
    }
}
