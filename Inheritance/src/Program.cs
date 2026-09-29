namespace LibraraySystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StudentMember student = new StudentMember("m1", "Ahmed Ali", "01987984651");
            Book book = new Book("b1","C# Basics",2m);
            Loan loan = student.BorrowItem("l1", new DateOnly(2026, 9, 29), book);
            Console.WriteLine(loan.Status);
            Console.WriteLine(book.IsOnLoan);
            Console.WriteLine(loan.DueDate);


            Console.WriteLine();
            Console.WriteLine();


            Librarian librarian = new Librarian("s1", "Ali Hassan", "010165481887", new DateOnly(2025, 1, 1), 7000m);
            librarian.RegisterReturn(loan, new DateOnly(2026, 10, 25));
            Console.WriteLine(loan.Status);
            Console.WriteLine(book.IsOnLoan);
            Console.WriteLine(loan.LateFee);
            Console.WriteLine(student.ActiveLoanCount);


            Console.WriteLine();
            Console.WriteLine();


            PremiumMember premium = new PremiumMember("m2", "Sara Ali", "01987646542", 20d);
            DVD dvd = new DVD("cat01", "title", 2m);
            Loan premiumLoan = premium.BorrowItem("l2", new DateOnly(2026, 9, 29), dvd);
            librarian.RegisterReturn(premiumLoan, new DateOnly(2026, 10, 8));
            Console.WriteLine(premiumLoan.LateFee);
            Console.WriteLine(premium.ReadingPoints);


            Console.WriteLine();
            Console.WriteLine();


            HeadLibrarian headLibrarian = new HeadLibrarian("cat564", "Asem alh", "01033654985423", new DateOnly(2024, 1, 1), 10000m);
            Magazine magazine = new Magazine("cat02", "title02", 2m);
            headLibrarian.ChangeItemLateFee(magazine, 4m);
            headLibrarian.WithdrawItem(magazine);
            Console.WriteLine(magazine.BaseLateFee);
            Console.WriteLine(magazine.IsWithdraw);
            try
            {
                student.BorrowItem("cat3", new DateOnly(2026, 10, 1), magazine);
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine(exception.Message);
            }
            headLibrarian.RestoreItem(magazine);
            Console.WriteLine(magazine.IsWithdraw);


            Console.WriteLine();
            Console.WriteLine();

            Book book2 = new Book("B2", "Book2", 1m);
            Book book3 = new Book("B3", "Book3", 1m);
            Book book4 = new Book("B4", "Book4", 1m);
            Book book5 = new Book("B5", "Book5", 1m);

            student.BorrowItem("L4", new DateOnly(2026, 10, 1), book2);
            student.BorrowItem("L5", new DateOnly(2026, 10, 1), book3);
            student.BorrowItem("L6", new DateOnly(2026, 10, 1), book4);

            Console.WriteLine(student.ActiveLoanCount);

            try
            {
                student.BorrowItem("l7", new DateOnly(2026, 10, 1), book5);
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.WriteLine();
            Console.WriteLine();

            Book lostBook = new Book("B6", "Lost Book", 1m);
            Loan lostLoan = premium.BorrowItem("l8", new DateOnly(2026, 10, 1), lostBook);

            librarian.RegisterLost(lostLoan);

            Console.WriteLine(lostLoan.Status);

            Console.WriteLine();
            Console.WriteLine();

            Shelver shelver = new Shelver("s3", "Omar Sami", "0654649845", new DateOnly(2025, 3, 1), 6000m, "Fiction");

            Console.WriteLine(shelver.Section);

            shelver.Reassign("History");

            Console.WriteLine(shelver.Section);
            Console.WriteLine(headLibrarian.MonthlyPay);

            headLibrarian.GiveRaise(10m);

            Console.WriteLine(headLibrarian.MonthlyPay);
        }
    }
}
