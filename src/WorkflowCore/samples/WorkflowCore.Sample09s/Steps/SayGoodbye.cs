using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Sample09s
{    
    public class SayGoodbye : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Goodbye");
            return ExecutionResult.Next();
        }
    }
}
