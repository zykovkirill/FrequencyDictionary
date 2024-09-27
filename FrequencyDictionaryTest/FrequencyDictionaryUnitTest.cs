namespace FrequencyDictionaryTest
{
    public class FrequencyDictionaryUnitTest
    {
        static string TestText = "���� ���� ��� ��� ���������� \r\n ��� ��� ";

        [Fact]
        public void ValidatePathTest()
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, "test.txt");
            var isValid = FrequencyDictionaryLogic.ValidatePath(path);
            Assert.True(isValid);
            isValid = FrequencyDictionaryLogic.ValidatePath(null);
            Assert.False(isValid);
            isValid = FrequencyDictionaryLogic.ValidatePath(string.Empty);
            Assert.False(isValid);
        }


        [Fact]
        public void CalculateTest()
        {
            var result = FrequencyDictionaryLogic.Calculate(TestText);

            Assert.True(result["����"] == 2);
            Assert.True(result["���"] == 2);
            Assert.True(result["���"] == 2);
            Assert.True(result["����������"] == 1);
        }
    }
}