using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskRunner.Tasks
{
    public class UserInput
    {
        private readonly DbInitialization _dbInit;
        public UserInput(DbInitialization dbInit)
        {
            _dbInit = dbInit;
        }
        public async Task GetAsync()
        {
            Console.WriteLine("For Db Creating Press 1");

            var input = Console.ReadLine();

            if (input == "1")
            {
                await _dbInit.Run(CancellationToken.None);
            }
        }
    }
}

