using System.Runtime.Serialization;

namespace ShoppingAppAPI.Exeptions.CategoryExceptions
{
    public class CategoryNotFoundException : Exception
    {
        string message;
        public CategoryNotFoundException()
        {
            message = "Category with given ID Not Found!";
        }

        public override string Message => message;
    }
}