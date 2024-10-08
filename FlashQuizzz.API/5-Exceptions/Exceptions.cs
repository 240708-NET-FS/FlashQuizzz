namespace FlashQuizzz.API.Exceptions;

public class InvalidUserException : Exception
{
    public InvalidUserException(){}
    public InvalidUserException(string message) : base(message){}
    public InvalidUserException(string message, Exception inner) : base(message, inner){}
}

public class InvalidLoginException : Exception
{
    public InvalidLoginException(){}
    public InvalidLoginException(string message) : base(message){}
    public InvalidLoginException(string message, Exception inner) : base(message, inner){}
}

public class InvalidFlashCardException : Exception
{
    public InvalidFlashCardException(){}
    public InvalidFlashCardException(string message) : base(message){}
    public InvalidFlashCardException(string message, Exception inner) : base(message, inner){}
}

public class InvalidFlashCardCategoryException : Exception
{
    public InvalidFlashCardCategoryException(){}
    public InvalidFlashCardCategoryException(string message) : base(message){}
    public InvalidFlashCardCategoryException(string message, Exception inner) : base(message, inner){}
}

public class InvalidFlashCardAnswerException : Exception
{
    public InvalidFlashCardAnswerException(){}
    public InvalidFlashCardAnswerException(string message) : base(message){}
    public InvalidFlashCardAnswerException(string message, Exception inner) : base(message, inner){}
}