namespace RefactoringChallenge.Data.CQRS;

public interface ICommandFactory<T, K>
{
    ICommand<T, K> Create();
    ICommand<T, K> Update();
    ICommand<T, K> Delete();
}