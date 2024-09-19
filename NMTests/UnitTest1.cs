using NMGui.Config;

namespace NMTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        string expected = "All files (*.*)|*.*|Images (*.jpg, *.jpeg, *.bmp, *.png, *.gif)|*.JPG;*.JPEG;*.BMP;*.PNG;*.GIF|Documents (*.docx, *.doc, *.pdf, *.rtf)|*.DOCX;*.DOC;*.PDF;*.RTF";
        string actual = NMGuiConfig.FileDialogFilterString;

        Assert.Equal(expected, actual);
    }
}