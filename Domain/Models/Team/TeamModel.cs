
namespace Domain.Models
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int teamLeaderId { get; set; }
        public int l_id { get; set; }
    }
}