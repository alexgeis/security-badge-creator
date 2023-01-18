using System;
using System.Collections.Generic;

namespace CatWorx.BadgeMaker
{
    class Program
    {
        async static Task Main(string[] args)
        {
            bool userGetAPI = Util.ChooseGetMethod();
            List<Employee> employees = new List<Employee>();
            if (userGetAPI)
            {
                employees = await PeopleFetcher.GetFromApi();
            }
            else
            {
                employees = PeopleFetcher.GetEmployees();
            }
            Util.PrintEmployees(employees);
            Util.MakeCSV(employees);
            await Util.MakeBadges(employees);
        }
    }
}
