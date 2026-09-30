using System;
using System.Windows;
using System.Windows.Controls;
using System.Numerics;

namespace TC
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			if (MainTabControl != null) MainTabControl.SelectedIndex = 0;
			if (FromUnit != null) FromUnit.SelectedIndex = 1; // Выбираем "Минуты" по умолчанию
			
		}

		// 1. Клик по кнопке-гамбургеру открывает всплывающее меню под ней
		private void MenuButton_Click(object sender, RoutedEventArgs e)
		{
			if (MenuPopup != null)
			{
				MenuPopup.IsOpen = !MenuPopup.IsOpen;

				if (MenuPopup.IsOpen)
				{
					this.PreviewMouseDown += CloseMenuOnOutsideClick;
				}
				else
				{
					this.PreviewMouseDown -= CloseMenuOnOutsideClick;
				}
			}
		}

		private void CloseMenuOnOutsideClick (object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (MenuPopup != null && !HamburgerButton.IsMouseOver && !MenuPopup.IsMouseOver)
			{
				MenuPopup.IsOpen = false;
				this.PreviewMouseDown -= CloseMenuOnOutsideClick;
			}
		}
		// 2. Клик по пункту из выпадающего списка переключает вкладку и СВОРЫВАЕТ меню обратно
		private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (MainTabControl != null && MenuListBox != null && MenuListBox.SelectedIndex >= 0)
			{
				// Переключаем основную вкладку
				MainTabControl.SelectedIndex = MenuListBox.SelectedIndex;

				// Закрываем выпадающее меню
				if (MenuPopup != null) MenuPopup.IsOpen = false;

				// Сбрасываем выделение в меню, чтобы по нему можно было кликнуть повторно
				MenuListBox.SelectedIndex = -1;
			}
		}

		// 3. Операция на Вкладке 1: Дата минус Дата
		private void CalculateDateDifference(object sender, SelectionChangedEventArgs e)
		{
			if (StartDatePicker?.SelectedDate != null && EndDatePicker?.SelectedDate != null)
			{
				DateTime start = StartDatePicker.SelectedDate.Value;
				DateTime end = EndDatePicker.SelectedDate.Value;

				DateTime tmp;
				if (end < start)
				{
					tmp = end;
					end = start;
					start = tmp;
				}

				// Считаем разницу во времени
				TimeSpan difference = end - start;
				int totalDays = Math.Abs(difference.Days);
				int years = 0;
				int months = 0;
				int days = 0;

				tmp = start;
				while (tmp.AddYears(1) <= end)
				{
					tmp = tmp.AddYears(1);
					years++;
				}	
				while (tmp.AddMonths(1) <= end)
				{
					tmp = tmp.AddMonths(1);
					months++;
				}
				days = (end-tmp).Days;

				if (DateResultText != null)
				{
					DateResultText.Text = $"{totalDays} дней \n({years} лет, {months} мес, {days} дн.)";
				}
			}
			else
			{
				DateResultText.Text = "";
			}
		}

		// 4. Операция на Вкладке 3: Конвертер единиц времени
		private void ConvertTime(object sender, SelectionChangedEventArgs e) => DoConversion();
		private void ConvertTime(object sender, TextChangedEventArgs e) => DoConversion();

		private void DoConversion()
		{
			// Проверяем, что все новые элементы интерфейса уже созданы
			if (InputValue == null || FromUnit == null ||
				ResultSeconds == null || ResultMinutes == null || ResultHours == null ||
				ResultWeeks == null || ResultMonths == null || ResultYears == null ||
				FullFormattedResult == null)
			{
				return;
			}

			string inputText = InputValue.Text.Trim();

			// Если поле ввода пустое, очищаем все результаты
			if (string.IsNullOrEmpty(inputText))
			{
				ResetConverterFields();
				return;
			}

			// Используем стандартный double, который есть во всех версиях .NET Framework
			if (!double.TryParse(inputText, out double inputNumber) || inputNumber < 0 || double.IsInfinity(inputNumber) || double.IsNaN(inputNumber))
			{
				FullFormattedResult.Text = "Введите корректное положительное число";
				return;
			}

			// Константы секунд в разных периодах
			double secondsInMinute = 60.0;
			double secondsInHour = 3600.0;
			double secondsInDay = 86400.0;
			double secondsInWeek = 604800.0;
			double secondsInMonth = 2592000.0; // 30 дней
			double secondsInYear = 31536000.0; // 365 дней

			double totalSeconds = 0;

			// 1. ПЕРЕВОДИМ ВВЕДЕННОЕ ЗНАЧЕНИЕ В СЕКУНДЫ
			int selectedIndex = FromUnit.SelectedIndex;
			switch (selectedIndex)
			{
				case 0: totalSeconds = inputNumber; break; // Секунды
				case 1: totalSeconds = inputNumber * secondsInMinute; break; // Минуты
				case 2: totalSeconds = inputNumber * secondsInHour; break; // Часы
				case 3: totalSeconds = inputNumber * secondsInWeek; break; // Недели
				case 4: totalSeconds = inputNumber * secondsInMonth; break; // Месяцы
				case 5: totalSeconds = inputNumber * secondsInYear; break; // Годы
				default: totalSeconds = inputNumber; break;
			}

			// 2. ЗАПОЛНЯЕМ ПРАВУЮ ТАБЛИЦУ (перевод во все единицы одновременно с дробями)
			ResultSeconds.Text = totalSeconds.ToString("N0");
			ResultMinutes.Text = (totalSeconds / secondsInMinute).ToString("N2");
			ResultHours.Text = (totalSeconds / secondsInHour).ToString("N2");
			ResultWeeks.Text = (totalSeconds / secondsInWeek).ToString("N2");
			ResultMonths.Text = (totalSeconds / secondsInMonth).ToString("N2");
			ResultYears.Text = (totalSeconds / secondsInYear).ToString("N2");

			// 3. РАСКЛАДЫВАЕМ НА СТРОКУ ПОЛНОГО ФОРМАТА (с отсечением дробной части для вывода целых единиц)
			double remainder = Math.Floor(totalSeconds);

			double years = Math.Floor(remainder / secondsInYear);
			remainder %= secondsInYear;

			double months = Math.Floor(remainder / secondsInMonth);
			remainder %= secondsInMonth;

			double days = Math.Floor(remainder / secondsInDay);
			remainder %= secondsInDay;

			double hours = Math.Floor(remainder / secondsInHour);
			remainder %= secondsInHour;

			double minutes = Math.Floor(remainder / secondsInMinute);
			double seconds = remainder % secondsInMinute;

			// Выводим готовую строку формата
			FullFormattedResult.Text = string.Format("{0:N0}\u00A0Лет {1}\u00A0Месяцев {2}\u00A0Дней {3}\u00A0Часов {4}\u00A0Минут {5}\u00A0Секунд",
				years, months, days, hours, minutes, seconds);
		}

		private void CopyResult_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button copyButton && copyButton.Tag is TextBlock targetTextBlock)
			{
				string textToCopy = targetTextBlock.Text;

				if (!string.IsNullOrEmpty(textToCopy) && textToCopy != "0")
				{
					// Удаляем разделительные пробелы, чтобы скопировать чистое число
					string cleanNumber = textToCopy.Replace(" ", "").Replace("\u00A0", "");

					try
					{
						Clipboard.SetText(cleanNumber);
					}
					catch (System.Runtime.InteropServices.ExternalException)
					{
						// Защита на случай, если буфер обмена временно заблокирован другой программой
					}
				}
			}
		}


		// Метод сброса полей в дефолтное состояние
		private void ResetConverterFields()
		{
			ResultSeconds.Text = "0";
			ResultMinutes.Text = "0";
			ResultHours.Text = "0";
			ResultWeeks.Text = "0";
			ResultMonths.Text = "0";
			ResultYears.Text = "0";
			FullFormattedResult.Text = "0\u00A0Лет 0\u00A0Месяцев 0\u00A0Дней 0\u00A0Часов 0\u00A0Минут 0\u00A0Секунд";
		}

		private void StartDatePicker_Loaded(object sender, RoutedEventArgs e)
		{
			var datePicker = sender as DatePicker;
						
				var datePickerTextBox = datePicker.Template.FindName("PART_TextBox", datePicker) as System.Windows.Controls.Primitives.DatePickerTextBox;
				
					datePickerTextBox.Text = string.Empty;

		}

	}
}
