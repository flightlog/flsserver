namespace FLS.Server.Tests
{
    public class TestConfigurationSettings
    {
        private static TestConfigurationSettings _instance = null;

        public string TestEmailAddress { get; set; }

        public string TestSystemAdminUsername { get; set; }
        public string TestSystemAdminPassword { get; set; }

        public string TestClubAdminUsername { get; set; }
        public string TestClubAdminPassword { get; set; }

        public string TestClubUserUsername { get; set; }
        public string TestClubUserPassword { get; set; }

        private TestConfigurationSettings()
        {
            TestEmailAddress = "test@glider-fls.ch";
            TestSystemAdminUsername = "s";
            TestSystemAdminPassword = "s";
            TestClubAdminUsername = "testclubadmin";
            TestClubAdminPassword = "s";
            TestClubUserUsername = "testclubuser";
            TestClubUserPassword = "f";
        }

        public static TestConfigurationSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TestConfigurationSettings();
                }

                return _instance;
            }
        }
    }
}
