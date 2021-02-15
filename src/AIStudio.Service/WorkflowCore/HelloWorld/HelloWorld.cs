using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Service.WorkflowCore
{
    public class HelloWorld : OANormalStep
    {
        public string HelloName { get; set; }
        //在这中实现需要执行的方法
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            GetStep(context);

            Console.WriteLine(HelloName + " Hello world");           
            return ExecutionResult.Next();
        }
    }
}
