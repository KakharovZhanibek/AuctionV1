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
        //Добавьте Employee и Organization в БД

        static void Main(string[] args)
        {
             Console.WriteLine("Введите название документа");
             string name = Console.ReadLine();
             Console.WriteLine("Введите путь к документу");
             string path = @"";
             path += Console.ReadLine();

             CreateLotAttachViewModel attach1 = new CreateLotAttachViewModel()
             {
                 Name = name,
                 Path = path
             };

             List<CreateLotAttachViewModel> attachments = new List<CreateLotAttachViewModel>();
             attachments.Add(attach1);

             Console.WriteLine("Введите название лота");
            string lotName = Console.ReadLine();

             Console.WriteLine("Введите описание лота");
            string lotDescription = Console.ReadLine();

             Console.WriteLine("Введите цену лота");
            decimal initialCost = Decimal.Parse(Console.ReadLine());

             Console.WriteLine("Введите ID ответственного лица лота");
            string сreatedByEmployeeId = Console.ReadLine();

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
