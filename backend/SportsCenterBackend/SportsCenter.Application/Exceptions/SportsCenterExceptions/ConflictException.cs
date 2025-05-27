namespace SportsCenter.Application.Exceptions
{
    public class ConflictException : Exception
    {
        public List<ConflictInfo> Conflicts { get; }

        public ConflictException(List<ConflictInfo> conflicts) 
            : base("There are conflicts with reservations or sport activities")
        {
            Conflicts = conflicts;
        }
    }

    public class ConflictInfo
    {
        public string Type { get; }
        public int Id { get; }
        public DateTime? Date { get; }
        public ConflictInfo(string type, int id, DateTime? date = null)
        {
            Type = type;
            Id = id;
            Date = date; //w razie potrzeby bedzie mozna dodac aby zwracalo takze kolidujaca date
        }
    }
}