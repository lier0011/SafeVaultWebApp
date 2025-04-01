using SafeVaultWebApp.Tools;
using NUnit.Framework;

[TestFixture]
public class TestInputValidation
{
    [Test]
    public void TestForSQLInjection()
    {
        // Arrange
        string maliciousInput = "'; DROP TABLE Users; --";

        // Act
        bool isValid = InputValidator.IsValidInput(maliciousInput);

        // Assert
        Assert.That(isValid, Is.False, "Input validation failed to detect SQL injection.");
    }

    [Test]
    public void TestForXSS()
    {
        // Arrange
        string maliciousInput = "<script>alert('XSS');</script>";

        // Act
        bool isValid = InputValidator.IsValidInput(maliciousInput);

        // Assert
        Assert.That(isValid, Is.False, "Input validation failed to detect XSS.");
    }
}