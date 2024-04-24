using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models.Items
{
    public class Game(int id, string name) : ILibraryItem
    {
        public string Name { get; } = name;

        public int Id { get; } = id;

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
