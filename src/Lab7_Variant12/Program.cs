/*
 * Лабораторная работа № 7 «Техническое документирование программного проекта»
 * Вариант 12. Компания «Канцелярия-Про», канцелярские товары
 * Условие скидки: клиент не делал заказов в прошлом месяце - скидка 15 %
 * Выполнил: Чепашев Константин Степанович, группа И-43
 */
using System;
using System.Collections.Generic;
namespace Lab7_Variant12
{
	/// <summary>
	/// Данные студента для вывода в конце программы:
	/// фамилия, имя, отчество и группа.
	/// </summary>
	public static class StudentInfo
	{
		/// <summary>Фамилия, имя и отчество.</summary>
		public const string Name = "Чепашев Константин Степанович";

		/// <summary>Номер группы.</summary>
		public const string GroupNumber = "И-43";
	}

	/// <summary>
	/// Товар из каталога «Канцелярия-Про».
	/// </summary>
	public class Product
	{
		/// <summary>Артикул товара.</summary>
		public string Code { get; private set; }

		/// <summary>Наименование товара.</summary>
		public string Title { get; private set; }

		/// <summary>Цена за штуку в рублях.</summary>
		public decimal Price { get; private set; }

		/// <summary>
		/// Создаёт товар каталога.
		/// </summary>
		/// <param name="code">Артикул.</param>
		/// <param name="title">Наименование.</param>
		/// <param name="price">Цена за штуку в рублях.</param>
		public Product(string code, string title, decimal price)
		{
			Code = code;
			Title = title;
			Price = price;
		}
	}

	/// <summary>
	/// Заявка клиента: код клиента, история заказов и список товаров.
	/// </summary>
	public class ClientRequest
	{
		/// <summary>Код клиента в базе компании.</summary>
		public string ClientCode { get; private set; }

		/// <summary>Сколько заказов клиент сделал в прошлом месяце.</summary>
		public int PreviousMonthOrders { get; private set; }

		/// <summary>Товары заявки.</summary>
		public List<Product> Products { get; private set; }

		/// <summary>Количество каждого товара, в том же порядке, что и Products.</summary>
		public List<int> Amounts { get; private set; }

		/// <summary>
		/// Создаёт пустую заявку клиента.
		/// </summary>
		/// <param name="clientCode">Код клиента.</param>
		/// <param name="previousMonthOrders">Число заказов в прошлом месяце.</param>
		public ClientRequest(string clientCode, int previousMonthOrders)
		{
			ClientCode = clientCode;
			PreviousMonthOrders = previousMonthOrders;
			Products = new List<Product>();
			Amounts = new List<int>();
		}

		/// <summary>
		/// Добавляет товар в заявку.
		/// </summary>
		/// <param name="product">Товар из каталога.</param>
		/// <param name="amount">Количество, шт.</param>
		public void AddProduct(Product product, int amount)
		{
			Products.Add(product);
			Amounts.Add(amount);
		}
	}

	/// <summary>
	/// Расчёт скидки и суммы заявки по правилу варианта 12.
	/// </summary>
	public class DiscountService
	{
		/// <summary>Размер скидки в процентах.</summary>
		public const int DiscountPercent = 15;

		/// <summary>
		/// Определяет процент скидки для заявки.
		/// </summary>
		/// <param name="request">Заявка клиента.</param>
		/// <returns>15, если в прошлом месяце заказов не было, иначе 0.</returns>
		public int GetDiscountPercent(ClientRequest request)
		{
			if (request.PreviousMonthOrders == 0)
				return DiscountPercent;
			return 0;
		}

		/// <summary>
		/// Считает сумму заявки по ценам каталога.
		/// </summary>
		/// <param name="request">Заявка клиента.</param>
		/// <returns>Сумма без скидки, руб.</returns>
		public decimal GetSum(ClientRequest request)
		{
			decimal sum = 0m;
			for (int i = 0; i < request.Products.Count; i++)
				sum += request.Products[i].Price * request.Amounts[i];
			return sum;
		}

		/// <summary>
		/// Считает сумму к оплате с учётом скидки.
		/// </summary>
		/// <param name="request">Заявка клиента.</param>
		/// <returns>Сумма к оплате, руб.</returns>
		public decimal GetSumToPay(ClientRequest request)
		{
			decimal sum = GetSum(request);
			return sum - sum * GetDiscountPercent(request) / 100m;
		}
	}

	class Program
	{
		/// <summary>
		/// Печатает расчёт двух заявок и данные студента.
		/// </summary>
		/// <param name="args">Параметры запуска, не используются.</param>
		public static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			Product paper = new Product("КП-110", "Бумага офисная А4, 500 листов", 389m);
			Product pen = new Product("КП-204", "Ручка гелевая синяя", 45m);
			Product notebook = new Product("КП-317", "Тетрадь 96 листов, клетка", 120m);
			Product stapler = new Product("КП-422", "Степлер № 24/6", 310m);
			Product folder = new Product("КП-508", "Папка-регистратор 75 мм", 260m);

			ClientRequest office = new ClientRequest("К-031", 0);
			office.AddProduct(paper, 10);
			office.AddProduct(pen, 40);
			office.AddProduct(folder, 6);

			ClientRequest school = new ClientRequest("К-112", 2);
			school.AddProduct(notebook, 25);
			school.AddProduct(stapler, 2);
			school.AddProduct(pen, 30);

			DiscountService service = new DiscountService();

			Console.WriteLine("«Канцелярия-Про»: расчёт заявок на канцтовары");
			Console.WriteLine("Скидка {0} % клиентам без заказов в прошлом месяце", DiscountService.DiscountPercent);

			foreach (ClientRequest request in new ClientRequest[] { office, school }) {
				Console.WriteLine();
				Console.WriteLine("Клиент {0} (заказов в прошлом месяце: {1})",
				                  request.ClientCode, request.PreviousMonthOrders);
				for (int i = 0; i < request.Products.Count; i++) {
					Product p = request.Products[i];
					Console.WriteLine("  {0} {1}: {2} шт. по {3:N2} руб.",
					                  p.Code, p.Title, request.Amounts[i], p.Price);
				}
				Console.WriteLine("  Сумма заявки: {0:N2} руб.", service.GetSum(request));
				Console.WriteLine("  Скидка: {0} %", service.GetDiscountPercent(request));
				Console.WriteLine("  К оплате: {0:N2} руб.", service.GetSumToPay(request));
			}

			Console.WriteLine();
			Console.WriteLine("Студент: {0}, группа {1}", StudentInfo.Name, StudentInfo.GroupNumber);

			if (!Console.IsInputRedirected) {
				Console.Write("Нажмите любую клавишу . . . ");
				Console.ReadKey(true);
			}
		}
	}
}
