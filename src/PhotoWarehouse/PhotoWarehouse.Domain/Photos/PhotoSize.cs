namespace PhotoWarehouse.Domain.Photos
{
    public class PhotoSize
    {
        public int Id { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }
}
