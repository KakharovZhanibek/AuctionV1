using AuctionDb.Models;
using AuctionDb.Repositories;
using AuctionDb.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDb
{
    class Program
    {
        static void Main(string[] args)
        {
             Console.WriteLine("Введите название документа");
             string name = "LOOOT";// Console.ReadLine();
             Console.WriteLine("Введите путь к документу");
             string path = @"C:\Users\Zhanibek\Desktop\Lafore.pdf";
             //path += Console.ReadLine();

             CreateLotAttachViewModel attach1 = new CreateLotAttachViewModel()
             {
                 Name = name,
                 Path = path
             };

             List<CreateLotAttachViewModel> attachments = new List<CreateLotAttachViewModel>();
             attachments.Add(attach1);

             Console.WriteLine("Введите название лота");
            string lotName = "LOOOTik";//Console.ReadLine();

             Console.WriteLine("Введите описание лота");
            string lotDescription = "Das is lot";//Console.ReadLine();

             Console.WriteLine("Введите цену лота");
            decimal initialCost = (decimal)45145000.45;//Decimal.Parse(Console.ReadLine());

             Console.WriteLine("Введите ID ответственного лица лота");
            string сreatedByEmployeeId = "EB7E4EF3-35DE-45A1-A7EB-A734FBCEBC16"; //Console.ReadLine();

             CreateAuctionViewModel auctionViewModel = new CreateAuctionViewModel()
             {
                 LotName = lotName,
                 LotDescription = lotDescription,
                 InitialCost = initialCost,
                 CreatedByEmployeeId = сreatedByEmployeeId,
                 LotAttachmentVMs = attachments
             };

             AccountService service = new AccountService();
             service.CreateAuction(auctionViewModel);
        }
    }
}
