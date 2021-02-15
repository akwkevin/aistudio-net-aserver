using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Service.WorkflowCore
{
    public class GoodbyeWorld : OANormalStep
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            GetStep(context);

            Console.WriteLine("Goodbye world");            
            return ExecutionResult.Next();
        }
    }
}
