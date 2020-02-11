namespace Web.Models.Read
{
    public class Read
    {
        public int ReaderId { set; get; }
        public int ID{set;get;}
        public int Chapters { set; get; }
        public int CurrentChapterNumber { set; get; }
        public string CurrentChapter { set; get; }
        public int userRating { set; get; }
    }
}
