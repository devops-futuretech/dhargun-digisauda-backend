using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Adani.Solution.Service.Common
{
    [Serializable]
    public class ReadUncommitedTransactionScopeAttribute : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            //declare the transaction options
            var transactionOptions = new TransactionOptions();
            //set it to read uncommited
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            //create the transaction scope, passing our options in
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                //declare our context
                using (var scope = new TransactionScope())
                {
                    args.Proceed();
                    scope.Complete();
                }
            }
        }
    }
}
