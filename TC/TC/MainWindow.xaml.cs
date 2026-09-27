using System;
using System.Windows;
using System.Windows.Controls;

namespace TC
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			// Настраиваем стартовые значения элементов при запуске приложения
			//if (StartDatePicker != null) StartDatePicker.SelectedDate = DateTime.Now;
			//if (EndDatePicker != null) EndDatePicker.SelectedDate = DateTime.Now.AddDays(7);
			if (MainTabControl != null) MainTabControl.SelectedIndex = 0;
			if (FromUnit != null) FromUnit.SelectedIndex = 1; // Выбираем "Минуты" по умолчанию
			if (ToUnit != null) ToUnit.SelectedIndex = 0;     // Выбираем "Секунды" по умолчанию
		}

		// 1. Клик по кнопке-гамбургеру открывает всплывающее меню под ней
		private void MenuButton_Click(object sender, RoutedEventArgs e)
		{
			if (MenuPopup != null)
			{
				// Открываем всплывающее окно меню
				MenuPopup.IsOpen = true;
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

				//сначала проверяем, что обе даты заполнены


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
		}

		// 4. Операция на Вкладке 3: Конвертер единиц времени
		private void ConvertTime(object sender, SelectionChangedEventArgs e) => DoConversion();
		private void ConvertTime(object sender, TextChangedEventArgs e) => DoConversion();

		private void DoConversion()
		{
			if (InputValue == null || FromUnit == null || ToUnit == null || ConvertResultText == null) return;

			// Проверяем, что введено именно число
			if (!double.TryParse(InputValue.Text, out double amount))
			{
				ConvertResultText.Text = "Введите число";
				return;
			}

			var fromItem = FromUnit.SelectedItem as ComboBoxItem;
			var toItem = ToUnit.SelectedItem as ComboBoxItem;

			if (fromItem != null && toItem != null)
			{
				// Считываем коэффициенты перевода из свойства Tag элементов XAML
				double fromInSeconds = Convert.ToDouble(fromItem.Tag);
				double toInSeconds = Convert.ToDouble(toItem.Tag);

				// Рассчитываем результат перевода
				double result = (amount * fromInSeconds) / toInSeconds;

				// Отображаем результат (формат "G" скрывает лишние нули в конце)
				ConvertResultText.Text = result.ToString("G");
			}
		}

		private void StartDatePicker_Loaded(object sender, RoutedEventArgs e)
		{
			var datePicker = sender as DatePicker;
						
				var datePickerTextBox = datePicker.Template.FindName("PART_TextBox", datePicker) as System.Windows.Controls.Primitives.DatePickerTextBox;
				
					datePickerTextBox.Text = string.Empty;

		}
	}
}
