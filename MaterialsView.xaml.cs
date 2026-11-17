using FacultyArchiveXml.Models;
using FacultyArchiveXml.Services;
using FacultyArchiveXml.Strategies;

namespace FacultyArchiveXml.Views;

public partial class MaterialsView : ContentPage
{
    private string _xmlFilePath = "";
    private string _xslPath = "";
    private string _htmlOutputPath = "";
    private XmlProcessor _processor = new XmlProcessor();
    private XmlTransformer _transformer = new XmlTransformer();

    public MaterialsView()
    {
        InitializeComponent();
        StrategyPicker.ItemsSource = new List<string> { "SAX", "DOM", "LINQ to XML" };
    }


    private async void OnSelectFileClicked(object sender, EventArgs e)
    {
        try
        {
            FilePickerFileType xmlType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".xml" } },
                { DevicePlatform.Android, new[] { "text/xml", "application/xml" } },
                { DevicePlatform.iOS, new[] { "public.xml" } },
                { DevicePlatform.MacCatalyst, new[] { "public.xml" } }
            });

            FileResult? file = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Оберіть XML-файл",
                FileTypes = xmlType
            });

            if (file == null)
                return; // користувач скасував

            _xmlFilePath = file.FullPath;

            // XSL і HTML лежать поруч з XML і мають таку саму назву
            string folder = Path.GetDirectoryName(_xmlFilePath) ?? "";
            string name = Path.GetFileNameWithoutExtension(_xmlFilePath);
            _xslPath = Path.Combine(folder, name + ".xsl");
            _htmlOutputPath = Path.Combine(folder, name + ".html");

            FileLabel.Text = _xmlFilePath;

            ResetFields();

            // "Підгрузка даних": атрибути беремо з файлу
            ShowAttributes(_processor.GetAvailableAttributes(_xmlFilePath));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private void OnStrategySelected(object sender, EventArgs e)
    {
        if (StrategyPicker.SelectedIndex == 0)
            _processor.SetStrategy(new SaxSearchStrategy());
        else if (StrategyPicker.SelectedIndex == 1)
            _processor.SetStrategy(new DomSearchStrategy());
        else if (StrategyPicker.SelectedIndex == 2)
            _processor.SetStrategy(new LinqSearchStrategy());
    }

    // Коли обрано атрибут: підвантажуємо його значення з файлу
    private void OnAttributeSelected(object sender, EventArgs e)
    {
        if (_xmlFilePath == "" || AttributePicker.SelectedItem == null)
            return;

        string attribute = AttributePicker.SelectedItem.ToString() ?? "";
        ValuePicker.ItemsSource = _processor.GetAttributeValues(_xmlFilePath, attribute);
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        if (_xmlFilePath == "")
        {
            await DisplayAlert("Увага", "Спочатку оберіть XML-файл.", "OK");
            return;
        }
        if (StrategyPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Увага", "Оберіть спосіб пошуку (SAX, DOM або LINQ).", "OK");
            return;
        }

        string keyword = (KeywordEntry.Text ?? "").Trim();
        string attribute = "";
        string value = "";

        if (keyword != "")
        {
            // Ключове слово: атрибут порожній, тому шукаємо по всіх полях
            value = keyword;
        }
        else
        {
            // Інакше точний пошук: потрібні атрибут і значення зі списку
            if (AttributePicker.SelectedItem == null || ValuePicker.SelectedItem == null)
            {
                await DisplayAlert("Увага", "Введіть ключове слово або оберіть атрибут і значення зі списку.", "OK");
                return;
            }
            attribute = AttributePicker.SelectedItem.ToString() ?? "";
            value = ValuePicker.SelectedItem.ToString() ?? "";
        }

        try
        {
            SearchCriteria criteria = new SearchCriteria(attribute, value);
            List<Material> found = _processor.Search(_xmlFilePath, criteria);
            ShowResults(found);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private async void OnTransformClicked(object sender, EventArgs e)
    {
        if (_xmlFilePath == "")
        {
            await DisplayAlert("Увага", "Спочатку оберіть XML-файл.", "OK");
            return;
        }
        if (File.Exists(_xslPath) == false)
        {
            await DisplayAlert("Увага", "Не знайдено XSL-файл:\n" + _xslPath, "OK");
            return;
        }

        try
        {
            _transformer.TransformToHtml(_xmlFilePath, _xslPath, _htmlOutputPath);
            await DisplayAlert("Готово", "HTML-файл збережено:\n" + _htmlOutputPath, "OK");

            // Відкриваємо результат у браузері
            await Launcher.Default.OpenAsync(new OpenFileRequest("Результат", new ReadOnlyFile(_htmlOutputPath)));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        ResetFields();
    }

    private async void OnExitClicked(object sender, EventArgs e)
    {
        bool sure = await ConfirmExit();
        if (sure)
            CloseApplication();
    }

    // ---- Допоміжні методи ----

    // Показує імена атрибутів у списку
    public void ShowAttributes(List<string> attributes)
    {
        AttributePicker.ItemsSource = attributes;
        ValuePicker.ItemsSource = null;
    }

    // Показує знайдені матеріали в таблиці
    public void ShowResults(List<Material> materials)
    {
        ResultsView.ItemsSource = materials;
        CountLabel.Text = "Знайдено: " + materials.Count;
    }

    // Очищає поле виводу і параметри пошуку
    public void ResetFields()
    {
        KeywordEntry.Text = "";
        AttributePicker.SelectedIndex = -1;
        ValuePicker.ItemsSource = null;
        ResultsView.ItemsSource = null;
        CountLabel.Text = "";
    }

    // Питає підтвердження виходу
    public async Task<bool> ConfirmExit()
    {
        return await DisplayAlert("Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
    }

    public void CloseApplication()
    {
        Application.Current?.Quit();
    }
}
