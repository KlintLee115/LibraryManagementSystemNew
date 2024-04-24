namespace LibraryManagementSystem.Exceptions
{
    public class ItemDoesntExistError(string fieldName) : Exception($"{fieldName} doesn't exist")
    {
    }

    public class EmptyInputError(string fieldName) : Exception($"{fieldName} can't be empty")
    {
    }

    public class InputNotNumberError(string fieldName) : Exception($"{fieldName} must be number")
    {
    }
}
