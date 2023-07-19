namespace CargoManagementAPi.Models
{
    public class CargoOrderInfo
    {
        public CargoOrderInfo()
        {

        }
        public int CustId { get; set; }
        public string CustName { get; set; }
        public string CustPhNo { get; set; }
        public string CustEmail { get; set; }
        public int CargoId { get; set; }
        public string CargoName { get; set; }
        public double CargoPrice { get; set; }
    }
}
