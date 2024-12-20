using System.Threading.Tasks;

namespace Utilities
{
    public static class TaskExtensions
    {
        public static void Forget(this Task task)
        {
            // Intentionally left empty to suppress warnings for fire-and-forget tasks
        }
    }
}
