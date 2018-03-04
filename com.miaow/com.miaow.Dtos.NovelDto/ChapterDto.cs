namespace com.miaow.Dtos.NovelDto
{
    public class ChapterDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int NovelId { get; set; }
        public int SortId { get; set; }
    }
}