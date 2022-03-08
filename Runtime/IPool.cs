namespace Padoru.ObjectPooling
{
    public interface IPool<T> where T : class
    {
        T GetObject();
        void ReturnObject(T obj);
    }
}