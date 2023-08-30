using HW02.BussinessContext.FileDatabase;
using HW02.BussinessContext;
using HW02.Entities;
using HW02.IO;
using System.Text.RegularExpressions;
using HW02.LoggerContext.DB;
using HW02.AnalyticalDataContext.DB;
using HW02.AnalyticalDataContext;
using HW02.Events;

namespace HW02
{
    public class Program
    {
        public static void Main()
        {
            CategoryDBContext categoryDBContext = new CategoryDBContext();
            ProductDBContext productDBContext = new ProductDBContext(categoryDBContext);
            UserIO userIO = new UserIO();
            CategoryService categories = new CategoryService(userIO, categoryDBContext);
            ProductService products = new ProductService(userIO, productDBContext, categories);

            userIO.Command += categories.ExecuteCommand;
            userIO.Command += products.ExecuteCommand;

            LoggerDBContext loggerDBContext = new LoggerDBContext();
            LoggerListener loggerListener = new LoggerListener(loggerDBContext);

            categories.ExecutedCommand += loggerListener.CatchCommand;
            products.ExecutedCommand += loggerListener.CatchCommand;

            AnalyticalDBContext analyticalDBContext = new AnalyticalDBContext();
            AnalyticalDataListener analyticalDataListener = new AnalyticalDataListener(analyticalDBContext);

            categories.ExecutedCommand += analyticalDataListener.CatchCommand;
            products.ExecutedCommand += analyticalDataListener.CatchCommand;

            string[] seedingCommands = {"add-category Koreni", "add-category Napoje", "add-category Prislusenstvi",
                                        "add-product Zazvor 0 29", "add-product Kurkuma 0 39", "add-product Sprite 1 25",
                                        "add-product Cafe_Latte 1 49", "add-product Hmozdir 2 129", "add-product Mlynek 2 140"};

            foreach (string seedingCommand in seedingCommands)
            {
                Command command = new Command(seedingCommand);
                CommandEvent commandEvent = new CommandEvent();
                commandEvent.Command = command;
                userIO.OnCommand(commandEvent);
            }

            userIO.ReadInput();
        }
    }
}
