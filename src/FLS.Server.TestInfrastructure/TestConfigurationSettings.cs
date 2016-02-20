namespace FLS.Server.TestInfrastructure
{
    public class TestConfigurationSettings
    {
        private static TestConfigurationSettings _instance = null;

        public string TestEmailAddress { get; set; }
        
        public string TestClubAdminUsername { get; set; }
        public string TestClubAdminPassword { get; set; }

        public string TestClubUserUsername { get; set; }
        public string TestClubUserPassword { get; set; }

        private TestConfigurationSettings()
        {
            TestEmailAddress = "pschuler@galaxy-net.ch";
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
