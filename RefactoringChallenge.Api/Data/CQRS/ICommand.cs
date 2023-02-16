using System;
using System.Threading.Tasks;

namespace RefactoringChallenge.Data.CQRS;

public interface ICommand<T, K> : IDisposable
{
    Task<K> Execute(T data);
}