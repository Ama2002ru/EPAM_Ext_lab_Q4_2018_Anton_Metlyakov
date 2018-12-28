namespace Task_7
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITest
    {
        string MethodName { get; }

        List<int> Init(int arraySize);

        void Run(int arraySize);

        TimeSpan? TestPerformance(int testArraySize, int testPerformanceCount);
    }
}
