// See https://aka.ms/new-console-template for more information
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

FontManager.RegisterFont(File.OpenRead("Kalpurush.ttf"));

var document = Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(21, 29.7f, Unit.Centimetre);
        page.Margin(2, Unit.Centimetre);
        page.DefaultTextStyle(x => x.FontFamily("Kalpurush").FontSize(15));
        page.Content().Element(ComposeTable);
    });
});

// instead of the standard way of generating a PDF file
document.GeneratePdf("hello.pdf");

// use the following invocation
document.ShowInPreviewer();


void ComposeTable(IContainer container)
{
    container.Table(table =>
    {
        table.ColumnsDefinition(columns =>
        {
            columns.ConstantColumn(35);
            columns.RelativeColumn(1.2f);
            columns.RelativeColumn(4);
            columns.RelativeColumn();
        });

        table.Header(header =>
        {
            header.Cell().ColumnSpan(4).Element(x => x.Border(1)).AlignCenter().Text("QuestPDF Issue 260").FontSize(20);
            header.Cell().ColumnSpan(4).Element(x => x.Border(1)).AlignCenter().Text("[Bug] Unicode text shaping capability breaks when paragraph starts with English text ").FontSize(10);
            header.Cell().Element(x => x.Border(1).Background(Colors.Grey.Lighten2).Padding(3)).AlignCenter().Text("S.N.").SemiBold();
            header.Cell().Element(x => x.Border(1).Background(Colors.Grey.Lighten2).Padding(3)).AlignCenter().Text("Text Type").SemiBold();
            header.Cell().Element(x => x.Border(1).Background(Colors.Grey.Lighten2).Padding(3)).AlignCenter().Text("Text").SemiBold();
            header.Cell().Element(x => x.Border(1).Background(Colors.Grey.Lighten2).Padding(3)).AlignCenter().Text("Result").SemiBold();
        });


        // Outputs correct result
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text(1);
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text("Bengali");
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text("বিপদ আরও বাড়াচ্ছে টানা বৃষ্টি।");
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text("Correct").FontColor(Colors.Green.Darken3);

        // Outputs correct result
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text(2);
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text("Bengali first then English");
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text("বিপদ আরও বাড়াচ্ছে টানা বৃষ্টি। Example English Text.");
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text("Correct").FontColor(Colors.Green.Darken3);

        // Outputs incorrect result
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text(3);
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text("English text first then Bengali");
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text("Example English Text. বিপদ আরও বাড়াচ্ছে টানা বৃষ্টি।");
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text("Incorrect").FontColor(Colors.Red.Darken3);

        //Outputs correct result
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text(4);
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text("English and Bengali text separated by span");
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text(x=>
        {
            x.Span("Example English Text.");
            x.Span(" বিপদ আরও বাড়াচ্ছে টানা বৃষ্টি।");
        });
        table.Cell().Element(x => x.Border(1)).AlignCenter().Text("Correct").FontColor(Colors.Green.Darken3);

        table.Cell().ColumnSpan(4).Element(x => x.Border(1)).AlignCenter().Text(
            @"Comment : The text of #3 and #4 should be same but they are not.
It seems putting ASCII text first makes HarfBuzz to interpret the whole text as ASCII.");
    });
}