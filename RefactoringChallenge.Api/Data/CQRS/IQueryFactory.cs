namespace RefactoringChallenge.Data.CQRS;

public interface IQueryFactory<T>
{
    IQuery<T> Get();
}